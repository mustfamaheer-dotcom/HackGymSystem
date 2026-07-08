using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
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
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public RenewSubscriptionCommandHandler(
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Offer> offerRepo,
        IUnitOfWork unitOfWork,
        IStringLocalizer<ApplicationResources> localizer)
    {
        _subscriptionRepo = subscriptionRepo;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _offerRepo = offerRepo;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        const int maxRetries = 3;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var result = await HandleCoreAsync(request, cancellationToken);

                if (result.IsSuccess)
                    await _unitOfWork.CommitAsync(cancellationToken);
                else
                    await _unitOfWork.RollbackAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateException) when (attempt < maxRetries - 1)
            {
                await _unitOfWork.ResetAsync(cancellationToken);
                continue;
            }
        }

        return Result<Guid>.Failure(_localizer["Failed to renew subscription. Please try again."]);
    }

    private async Task<Result<Guid>> HandleCoreAsync(RenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var previous = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .FirstOrDefaultAsync(s => s.Id == request.PreviousSubscriptionId, cancellationToken);
        if (previous == null)
            return Result<Guid>.Failure(_localizer["Previous subscription not found"]);

        var activeExists = await _subscriptionRepo.AnyAsync(
            s => s.MemberId == previous.MemberId && s.Status == SubscriptionStatus.Active && s.Id != request.PreviousSubscriptionId, cancellationToken);
        if (activeExists)
            return Result<Guid>.Failure(_localizer["Member already has an active subscription. Cannot create a new one."]);

        previous.MarkRenewed();

        var planId = request.NewPlanId ?? previous.PlanId;
        var plan = await _planRepo.GetByIdAsync(planId, cancellationToken);
        if (plan == null)
            return Result<Guid>.Failure(_localizer["Plan not found"]);
        if (!plan.IsActive)
            return Result<Guid>.Failure(_localizer["Plan is not active"]);

        decimal totalValue = plan.Price;
        Domain.Entities.Offer? offer = null;

        if (request.OfferId.HasValue)
        {
            offer = await _offerRepo.Query()
                .FirstOrDefaultAsync(o => o.Id == request.OfferId.Value, cancellationToken);
            if (offer == null)
                return Result<Guid>.Failure(_localizer["Offer not found"]);
            if (!offer.IsActive)
                return Result<Guid>.Failure(_localizer["Offer is not valid"]);

            if (offer.LinkedPackageId.HasValue && offer.LinkedPackageId != planId)
                return Result<Guid>.Failure(_localizer["Offer is not applicable to the selected plan"]);

            totalValue = offer.OfferType switch
            {
                OfferType.FixedPrice => offer.OfferPrice ?? plan.Price,
                OfferType.FreeRegistration => 0,
                _ => plan.Price
            };
        }

        if (request.AmountPaid > totalValue)
            return Result<Guid>.Failure(_localizer["Amount paid cannot exceed total subscription value"]);

        int durationDays = plan.DurationDays;
        int freeMonths = 0;
        int freeDays = 0;
        if (offer != null)
        {
            freeMonths = offer.BonusMonths ?? 0;
            freeDays = offer.BonusDays ?? 0;
        }

        var expirationDate = request.StartDate
            .AddDays(durationDays)
            .AddMonths(freeMonths)
            .AddDays(freeDays);

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
        return Result<Guid>.Success(newSubscription.Id, _localizer["Subscription renewed successfully"]);
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
