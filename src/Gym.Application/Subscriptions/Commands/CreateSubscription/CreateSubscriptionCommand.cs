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

        // Check for existing active subscription
        var activeSub = await _subscriptionRepo.Query()
            .FirstOrDefaultAsync(s => s.MemberId == request.MemberId && s.Status == SubscriptionStatus.Active, cancellationToken);

        // If member has active sub and no offer selected → reject
        if (activeSub != null && !request.OfferId.HasValue)
            return Result<Guid>.Failure("Member already has an active subscription");

        // If member has active sub and offer is selected → extend the active sub
        if (activeSub != null && request.OfferId.HasValue)
            return await HandleExtendAsync(activeSub, member, request, cancellationToken);

        // --- No active subscription → create a new one ---
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
            var resolvedOffer = await _offerRepo.Query()
                .FirstOrDefaultAsync(o => o.Id == request.OfferId.Value, cancellationToken);
            if (resolvedOffer == null)
                return Result<Guid>.Failure("Offer not found");

            if (resolvedOffer.LinkedPackageId.HasValue)
            {
                resolvedPlanId = resolvedOffer.LinkedPackageId.Value;
                plan = await _planRepo.GetByIdAsync(resolvedPlanId, cancellationToken);
                if (plan == null)
                    return Result<Guid>.Failure("Linked plan not found");
                if (!plan.IsActive)
                    return Result<Guid>.Failure("Linked plan is not active");
            }
            else
            {
                // Offer has no linked plan — pick the first active plan as FK reference
                plan = await _planRepo.Query()
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);
                if (plan == null)
                    return Result<Guid>.Failure("No active plan found to link the offer to");
                resolvedPlanId = plan.Id;
            }
        }
        else
        {
            return Result<Guid>.Failure("Either plan or offer must be selected");
        }

        return await HandleNewAsync(member, plan, resolvedPlanId, request, cancellationToken);
    }

    private async Task<Result<Guid>> HandleExtendAsync(
        Domain.Entities.Subscription activeSub, Member member,
        CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepo.Query()
            .FirstOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);
        if (offer == null)
            return Result<Guid>.Failure("Offer not found");
        if (!offer.IsActive)
            return Result<Guid>.Failure("Offer is not active");
        if (offer.StartDate > DateTime.UtcNow || offer.EndDate < DateTime.UtcNow)
            return Result<Guid>.Failure("Offer is not within its valid date range");

        // Extend the active subscription's expiration by offer's bonus months/days
        activeSub.ExpirationDate = activeSub.ExpirationDate
            .AddMonths(offer.BonusMonths ?? 0)
            .AddDays(offer.BonusDays ?? 0);

        _subscriptionRepo.Update(activeSub);

        // Record payment against the active subscription
        if (request.AmountPaid > 0)
        {
            activeSub.RecordPayment(request.AmountPaid);

            var payment = new SubscriptionPayment(
                activeSub.Id, request.AmountPaid, request.PaymentMethod,
                activeSub.RemainingBalance, null, null,
                request.Notes);
            await _unitOfWork.Repository<SubscriptionPayment>().AddAsync(payment, cancellationToken);
        }

        // Log the extension
        var log = new SubscriptionTransactionLog(
            activeSub.Id, "Extended with Offer",
            $"Subscription {activeSub.ReceiptNumber} extended by " +
            $"{(offer.BonusMonths > 0 ? $"{offer.BonusMonths} month(s) " : "")}" +
            $"{(offer.BonusDays > 0 ? $"{offer.BonusDays} day(s) " : "")}" +
            $"via offer {offer.OfferTitle}");
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(activeSub.Id, "Subscription extended with offer successfully");
    }

    private async Task<Result<Guid>> HandleNewAsync(
        Member member, MembershipPlan plan, Guid resolvedPlanId,
        CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
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
        }

        // Calculate total value based on offer type
        var totalValue = CalculateTotalValue(plan.Price, offer);
        if (totalValue < 0) totalValue = 0;
        if (request.AmountPaid > totalValue)
            return Result<Guid>.Failure("Amount paid cannot exceed total subscription value");

        // Calculate expiration: plan duration + offer bonuses
        var durationDays = plan.DurationDays;
        var bonusMonths = offer?.BonusMonths ?? 0;
        var bonusDays = offer?.BonusDays ?? 0;

        var expirationDate = request.StartDate
            .AddDays(durationDays)
            .AddMonths(bonusMonths)
            .AddDays(bonusDays);

        // Minimum 1 month duration
        if (expirationDate == request.StartDate)
            expirationDate = request.StartDate.AddMonths(1);

        // Generate receipt number
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

    private static decimal CalculateTotalValue(decimal planPrice, Domain.Entities.Offer? offer)
    {
        if (offer == null) return planPrice;

        return offer.OfferType switch
        {
            OfferType.FixedPrice => offer.OfferPrice ?? planPrice,
            OfferType.FreeRegistration => 0,
            _ => planPrice // BonusDuration, ExtraFreeze, Custom — plan price unchanged
        };
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
