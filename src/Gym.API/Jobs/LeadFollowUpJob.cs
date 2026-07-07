using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Jobs;

public class LeadFollowUpJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LeadFollowUpJob> _logger;

    public LeadFollowUpJob(IUnitOfWork unitOfWork, ILogger<LeadFollowUpJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Member>();
        var stale = await repo.Query()
            .Where(m => m.Subscriptions.All(s => s.Status != SubscriptionStatus.Active))
            .Include(m => m.Subscriptions)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} leads/members needing follow-up", stale.Count);

        foreach (var member in stale)
        {
            _logger.LogInformation("Lead {MemberId} ({Name}) has no active subscription",
                member.Id, member.FullName);
        }
    }
}
