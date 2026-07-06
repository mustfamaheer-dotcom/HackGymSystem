using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class SyncAuditLog : BaseEntity
{
    public SyncEventType EventType { get; set; }
    public SyncDirection Direction { get; set; }
    public string? EntityId { get; set; }
    public string? Payload { get; set; }
    public SyncStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
