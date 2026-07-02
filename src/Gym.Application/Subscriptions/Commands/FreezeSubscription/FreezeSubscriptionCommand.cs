using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Commands.FreezeSubscription;

public record FreezeSubscriptionCommand(Guid Id, int FreezeDays, string? Reason) : IRequest<Result>;

public class FreezeSubscriptionCommandHandler : IRequestHandler<FreezeSubscriptionCommand, Result>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IUnitOfWork _unitOfWork;

    public FreezeSubscriptionCommandHandler(
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IRepository<MembershipPlan> planRepo,
        IUnitOfWork unitOfWork)
    {
        _subscriptionRepo = subscriptionRepo;
        _planRepo = planRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(FreezeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subscription == null)
            return Result.Failure("Subscription not found");

        if (subscription.Status != SubscriptionStatus.Active)
            return Result.Failure("Only active subscriptions can be frozen");

        if (subscription.Status == SubscriptionStatus.Frozen)
            return Result.Failure("Subscription is already frozen");

        var plan = await _planRepo.GetByIdAsync(subscription.PlanId, cancellationToken);
        if (plan != null && plan.FreezeDays.HasValue && request.FreezeDays > plan.FreezeDays.Value)
            return Result.Failure($"Maximum freeze days for this plan is {plan.FreezeDays.Value}");

        var freezeStart = DateTime.UtcNow;
        var freezeEnd = freezeStart.AddDays(request.FreezeDays);

        subscription.Freeze(freezeStart, freezeEnd, request.FreezeDays, request.Reason);

        var freezeHistory = new SubscriptionFreezeHistory(
            subscription.Id, freezeStart, freezeEnd, request.FreezeDays, request.Reason);
        await _unitOfWork.Repository<SubscriptionFreezeHistory>().AddAsync(freezeHistory, cancellationToken);

        var log = new SubscriptionTransactionLog(
            subscription.Id, "Frozen",
            $"Subscription {subscription.ReceiptNumber} frozen for {request.FreezeDays} days. Reason: {request.Reason ?? "N/A"}");
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Subscription frozen successfully");
    }
}

public class FreezeSubscriptionCommandValidator : AbstractValidator<FreezeSubscriptionCommand>
{
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public FreezeSubscriptionCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        _localizer = localizer;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage(_localizer["Subscription ID is required"]);
        RuleFor(v => v.FreezeDays)
            .GreaterThan(0).WithMessage(_localizer["Freeze days must be greater than 0"]);
    }
}
