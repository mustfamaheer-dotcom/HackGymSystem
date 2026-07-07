using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gym.Application.Jobs;

public class SubscriptionRenewalReminderJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubscriptionRenewalReminderJob> _logger;

    public SubscriptionRenewalReminderJob(IUnitOfWork unitOfWork, ILogger<SubscriptionRenewalReminderJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddDays(7);
        var repo = _unitOfWork.Repository<Subscription>();
        var expiring = await repo.Query()
            .Where(s => s.Status == SubscriptionStatus.Active
                && s.ExpirationDate <= cutoff
                && s.ExpirationDate > DateTime.UtcNow)
            .Include(s => s.Member)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} subscriptions expiring within 7 days", expiring.Count);

        var notificationRepo = _unitOfWork.Repository<Notification>();
        foreach (var sub in expiring)
        {
            await notificationRepo.AddAsync(new Notification(
                sub.MemberId,
                "Subscription Expiring Soon",
                $"Your subscription expires on {sub.ExpirationDate:yyyy-MM-dd}. Please renew."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
