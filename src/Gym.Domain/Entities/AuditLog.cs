using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Action { get; set; }
    public string EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }

    public User? User { get; set; }

    private AuditLog() { }

    public AuditLog(Guid? userId, string action, string entityType,
        string? entityId, string? oldValues, string? newValues, string? ipAddress)
    {
        UserId = userId;
        Action = action;
        EntityType = entityType;
        EntityId = entityId;
        OldValues = oldValues;
        NewValues = newValues;
        IpAddress = ipAddress;
    }
}
