using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Services.ZKTeco;

public class SyncAuditService : ISyncAuditService
{
    private readonly Data.GymDbContext _context;
    private readonly ILogger<SyncAuditService> _logger;

    public SyncAuditService(Data.GymDbContext context, ILogger<SyncAuditService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LogAsync(SyncAuditEntry entry, CancellationToken cancellationToken = default)
    {
        try
        {
            var log = new SyncAuditLog
            {
                EventType = entry.EventType,
                Direction = entry.Direction,
                EntityId = entry.EntityId,
                Payload = entry.Payload,
                Status = entry.Status,
                ErrorMessage = entry.ErrorMessage
            };

            _context.SyncAuditLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);

            if (entry.Status == SyncStatus.Failed)
                _logger.LogWarning("Sync audit: {EventType} {Direction} [{Status}] - {Error}",
                    entry.EventType, entry.Direction, entry.Status, entry.ErrorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist sync audit log");
        }
    }
}
