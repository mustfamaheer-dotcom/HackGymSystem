using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gym.Application.Jobs;

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
        var repo = _unitOfWork.Repository<Lead>();
        var now = DateTime.UtcNow;
        var stale = await repo.Query()
            .Where(l => l.NextFollowUpDate <= now
                && l.Status != LeadStatus.Converted
                && l.Status != LeadStatus.Lost)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} leads needing follow-up", stale.Count);

        foreach (var lead in stale)
        {
            _logger.LogInformation("Lead {LeadId} ({Name}) needs follow-up, next date: {Date}",
                lead.Id, lead.Name, lead.NextFollowUpDate);
        }
    }
}
