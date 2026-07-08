using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gym.Application.Jobs;

public class SubscriptionExpiryJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubscriptionExpiryJob> _logger;

    public SubscriptionExpiryJob(IUnitOfWork unitOfWork, ILogger<SubscriptionExpiryJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var repo = _unitOfWork.Repository<Subscription>();
        var expired = await repo.Query()
            .Where(s => (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Frozen)
                && s.ExpirationDate <= now)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Expiring {Count} subscriptions", expired.Count);

        var notificationRepo = _unitOfWork.Repository<Notification>();
        foreach (var sub in expired)
        {
            sub.MarkExpired();
            repo.Update(sub);
            await notificationRepo.AddAsync(new Notification(
                sub.MemberId,
                "Subscription Expired",
                "Your subscription has expired. Please renew to regain access."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
