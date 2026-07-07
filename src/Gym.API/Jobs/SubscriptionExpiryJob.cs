using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Jobs;

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

        foreach (var sub in expired)
        {
            sub.MarkExpired();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
