using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Commands.RenewSubscription;

public record RenewSubscriptionCommand(
    Guid PreviousSubscriptionId,
    Guid? NewPlanId,
    Guid? OfferId,
    decimal AmountPaid,
    PaymentMethod PaymentMethod,
    DateTime StartDate,
    string? AdminSignature,
    string? Notes) : IRequest<Result<Guid>>;

public class RenewSubscriptionCommandHandler : IRequestHandler<RenewSubscriptionCommand, Result<Guid>>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IRepository<Offer> _offerRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RenewSubscriptionCommandHandler(
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

    public async Task<Result<Guid>> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var previous = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .FirstOrDefaultAsync(s => s.Id == request.PreviousSubscriptionId, cancellationToken);
        if (previous == null)
            return Result<Guid>.Failure("Previous subscription not found");

        previous.Renew(
            request.NewPlanId ?? previous.PlanId,
            request.OfferId,
            0, 0, request.PaymentMethod,
            request.StartDate, DateTime.UtcNow, "");

        var planId = request.NewPlanId ?? previous.PlanId;
        var plan = await _planRepo.GetByIdAsync(planId, cancellationToken);
        if (plan == null)
            return Result<Guid>.Failure("Plan not found");
        if (!plan.IsActive)
            return Result<Guid>.Failure("Plan is not active");

        decimal totalValue = plan.Price;
        Domain.Entities.Offer? offer = null;

        if (request.OfferId.HasValue)
        {
            offer = await _offerRepo.Query()
                .FirstOrDefaultAsync(o => o.Id == request.OfferId.Value, cancellationToken);
            if (offer == null)
                return Result<Guid>.Failure("Offer not found");
            if (!offer.IsActive || offer.StartDate > DateTime.UtcNow || offer.EndDate < DateTime.UtcNow)
                return Result<Guid>.Failure("Offer is not valid");

            if (offer.LinkedPackageId.HasValue && offer.LinkedPackageId != planId)
                return Result<Guid>.Failure("Offer is not applicable to the selected plan");

            totalValue = offer.OfferType == OfferType.FixedPrice
                ? (offer.OfferPrice ?? plan.Price)
                : plan.Price;
        }

        if (request.AmountPaid > totalValue)
            return Result<Guid>.Failure("Amount paid cannot exceed total subscription value");

        int durationDays = plan.DurationDays;
        int freeMonths = 0;
        if (offer != null && offer.OfferType == OfferType.BonusDuration)
        {
            if (offer.BonusMonths.HasValue) freeMonths = offer.BonusMonths.Value;
            else if (offer.BonusDays.HasValue) durationDays += offer.BonusDays.Value;
        }

        var expirationDate = request.StartDate.AddDays(durationDays).AddMonths(freeMonths);

        var lastReceipt = await _subscriptionRepo.Query()
            .OrderByDescending(s => s.ReceiptNumber)
            .Select(s => s.ReceiptNumber)
            .FirstOrDefaultAsync(cancellationToken);
        int nextNum = 1;
        if (lastReceipt != null && lastReceipt.StartsWith("REC-") && int.TryParse(lastReceipt[4..], out var parsed))
            nextNum = parsed + 1;
        var receiptNumber = $"REC-{nextNum:D6}";

        var newSubscription = new Domain.Entities.Subscription(
            receiptNumber, previous.MemberId, planId, totalValue,
            request.AmountPaid, request.PaymentMethod, request.StartDate, expirationDate, request.OfferId)
        {
            AdminSignature = request.AdminSignature,
            Notes = request.Notes
        };

        await _subscriptionRepo.AddAsync(newSubscription, cancellationToken);

        if (request.AmountPaid > 0)
        {
            var payment = new SubscriptionPayment(
                newSubscription.Id, request.AmountPaid, request.PaymentMethod,
                newSubscription.RemainingBalance, null, null, request.Notes);
            await _unitOfWork.Repository<SubscriptionPayment>().AddAsync(payment, cancellationToken);
        }

        var desc = $"Subscription renewed from {previous.ReceiptNumber} to {receiptNumber}";
        if (offer != null) desc += $" with offer {offer.OfferTitle}";

        var log = new SubscriptionTransactionLog(newSubscription.Id, "Renewed", desc);
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        var prevLog = new SubscriptionTransactionLog(previous.Id, "Renewed",
            $"Subscription renewed - new receipt {receiptNumber}");
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(prevLog, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(newSubscription.Id, "Subscription renewed successfully");
    }
}

public class RenewSubscriptionCommandValidator : AbstractValidator<RenewSubscriptionCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public RenewSubscriptionCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.PreviousSubscriptionId)
            .NotEmpty().WithMessage(_localizer["Previous subscription ID is required"]);
        RuleFor(v => v.AmountPaid)
            .GreaterThanOrEqualTo(0).WithMessage(_localizer["Amount paid must be 0 or greater"]);
        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage(_localizer["Start date is required"]);
        RuleFor(v => v.PaymentMethod)
            .IsInEnum().WithMessage(_localizer["Invalid payment method"]);
    }
}
