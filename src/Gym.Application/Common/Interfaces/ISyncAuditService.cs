using Gym.Shared.Enums;

namespace Gym.Application.Common.Interfaces;

public class SyncAuditEntry
{
    public SyncEventType EventType { get; set; }
    public SyncDirection Direction { get; set; }
    public string? EntityId { get; set; }
    public string? Payload { get; set; }
    public SyncStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface ISyncAuditService
{
    Task LogAsync(SyncAuditEntry entry, CancellationToken cancellationToken = default);
}
