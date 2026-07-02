using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Subscriptions.Commands.UnfreezeSubscription;

public record UnfreezeSubscriptionCommand(Guid Id) : IRequest<Result>;

public class UnfreezeSubscriptionCommandHandler : IRequestHandler<UnfreezeSubscriptionCommand, Result>
{
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public UnfreezeSubscriptionCommandHandler(
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IUnitOfWork unitOfWork)
    {
        _subscriptionRepo = subscriptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnfreezeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepo.Query()
            .Include(s => s.Member)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subscription == null)
            return Result.Failure("Subscription not found");

        if (subscription.Status != SubscriptionStatus.Frozen)
            return Result.Failure("Only frozen subscriptions can be unfrozen");

        var unfreezeDate = DateTime.UtcNow;

        var lastFreeze = await _unitOfWork.Repository<SubscriptionFreezeHistory>()
            .FirstOrDefaultAsync(h => h.SubscriptionId == request.Id && h.UnfreezeDate == default, cancellationToken);
        if (lastFreeze != null)
            lastFreeze.SetUnfreezeDate(unfreezeDate);

        subscription.Unfreeze();

        var log = new SubscriptionTransactionLog(
            subscription.Id, "Unfrozen",
            $"Subscription {subscription.ReceiptNumber} unfrozen");
        await _unitOfWork.Repository<SubscriptionTransactionLog>().AddAsync(log, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Subscription unfrozen successfully");
    }
}
