using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    Guid MemberId,
    Guid? PlanId,
    Guid? OfferId,
    decimal AmountPaid,
    PaymentMethod PaymentMethod,
    DateTime StartDate,
    string? AdminSignature,
    string? Notes) : IRequest<Result<Guid>>;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result<Guid>>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IRepository<Offer> _offerRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Offer> offerRepo,
        IUnitOfWork unitOfWork)
    {
        _subscriptionRepo = subscriptionRepo;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _offerRepo = offerRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepo.GetByIdAsync(request.MemberId, cancellationToken);
        if (member == null)
            return Result<Guid>.Failure("Member not found");

        Guid resolvedPlanId;
        MembershipPlan plan;

        if (request.PlanId.HasValue)
        {
            resolvedPlanId = request.PlanId.Value;
            plan = await _planRepo.GetByIdAsync(resolvedPlanId, cancellationToken);
            if (plan == null)
                return Result<Guid>.Failure("Plan not found");
            if (!plan.IsActive)
                return Result<Guid>.Failure("Plan is not active");
        }
        else if (request.OfferId.HasValue)
        {
            Domain.Entities.Offer? resolvedOffer = await _offerRepo.Query()
                .FirstOrDefaultAsync(o => o.Id == request.OfferId.Value, cancellationToken);
            if (resolvedOffer == null)
                return Result<Guid>.Failure("Offer not found");
            if (!resolvedOffer.LinkedPackageId.HasValue)
                return Result<Guid>.Failure("Selected offer does not have a linked plan");
            resolvedPlanId = resolvedOffer.LinkedPackageId.Value;
            plan = await _planRepo.GetByIdAsync(resolvedPlanId, cancellationToken);
            if (plan == null)
                return Result<Guid>.Failure("Linked plan not found");
            if (!plan.IsActive)
                return Result<Guid>.Failure("Linked plan is not active");
        }
        else
        {
            return Result<Guid>.Failure("Either plan or offer must be selected");
        }

        var hasActive = await _subscriptionRepo.AnyAsync(s =>
            s.MemberId == request.MemberId && s.Status == SubscriptionStatus.Active, cancellationToken);
        if (hasActive)
            return Result<Guid>.Failure("Member already has an active subscription");

        decimal totalValue = plan.Price;
        Domain.Entities.Offer? offer = null;

        if (request.OfferId.HasValue)
        {
            offer = await _offerRepo.Query()
                .FirstOrDefaultAsync(o => o.Id == request.OfferId.Value, cancellationToken);
            if (offer == null)
                return Result<Guid>.Failure("Offer not found");
            if (!offer.IsActive)
                return Result<Guid>.Failure("Offer is not active");
            if (offer.StartDate > DateTime.UtcNow || offer.EndDate < DateTime.UtcNow)
                return Result<Guid>.Failure("Offer is not within its valid date range");

            if (offer.LinkedPackageId.HasValue && offer.LinkedPackageId != resolvedPlanId)
                return Result<Guid>.Failure("Offer is not applicable to the selected plan");

            totalValue = offer.OfferType switch
            {
                OfferType.FixedPrice => offer.OfferPrice ?? plan.Price,
                OfferType.BonusDuration => plan.Price,
                OfferType.ExtraFreeze => plan.Price,
                OfferType.FreeRegistration => plan.Price,
                OfferType.Custom => plan.Price,
                _ => plan.Price
            };
        }

        if (totalValue < 0)
            totalValue = 0;

        if (request.AmountPaid > totalValue)
            return Result<Guid>.Failure("Amount paid cannot exceed total subscription value");

        int durationDays = plan.DurationDays;
        int freeMonths = 0;

        if (offer != null && offer.OfferType == OfferType.BonusDuration)
        {
            if (offer.BonusMonths.HasValue)
                freeMonths = offer.BonusMonths.Value;
            else if (offer.BonusDays.HasValue)
                durationDays += offer.BonusDays.Value;
        }

        var expirationDate = request.StartDate.AddDays(durationDays).AddMonths(freeMonths);

        var lastReceipt = await _subscriptionRepo.Query()
            .OrderByDescending(s => s.ReceiptNumber)
            .Select(s => s.ReceiptNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNum = 1;
        if (lastReceipt != null && lastReceipt.StartsWith("REC-"))
        {
            int.TryParse(lastReceipt[4..], out nextNum);
            nextNum++;
        }
        var receiptNumber = $"REC-{nextNum:D6}";

        var subscription = new Domain.Entities.Subscription(
            receiptNumber, request.MemberId, resolvedPlanId, totalValue,
            request.AmountPaid, request.PaymentMethod, request.StartDate, expirationDate, request.OfferId)
        {
            AdminSignature = request.AdminSignature,
            Notes = request.Notes
        };

        await _subscriptionRepo.AddAsync(subscription, cancellationToken);

        if (request.AmountPaid > 0)
        {
            var payment = new SubscriptionPayment(
                subscription.Id, request.AmountPaid, request.PaymentMethod,
                subscription.RemainingBalance, null, null,
                request.Notes);
            await _unitOfWork.Repository<SubscriptionPayment>().AddAsync(payment, cancellationToken);
        }

        var logAction = offer != null ? "Created with Offer" : "Created";
        var logDesc = offer != null
            ? $"Subscription {receiptNumber} created for {member.FullName} with offer {offer.OfferTitle}"
            : $"Subscription {receiptNumber} created for {member.FullName}";
        var log = new SubscriptionTransactionLog(subscription.Id, logAction, logDesc);
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscription.Id, "Subscription created successfully");
    }
}

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public CreateSubscriptionCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.MemberId)
            .NotEmpty().WithMessage(_localizer["Member is required"]);
        RuleFor(v => v.PlanId)
            .NotEmpty().When(v => v.OfferId == null)
            .WithMessage(_localizer["Plan is required when no offer is selected"]);
        RuleFor(v => v.OfferId)
            .NotEmpty().When(v => v.PlanId == null)
            .WithMessage(_localizer["Offer is required when no plan is selected"]);
        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage(_localizer["Start date is required"]);
        RuleFor(v => v.AmountPaid)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Amount paid must be 0 or greater"]);
        RuleFor(v => v.PaymentMethod)
            .IsInEnum().WithMessage(_localizer["Invalid payment method"]);
    }
}
