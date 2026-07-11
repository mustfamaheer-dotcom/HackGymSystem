namespace Gym.Domain.Events;

public class AttendanceSyncedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public string DeviceSerial { get; set; } = string.Empty;
    public int RecordsCount { get; set; }
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
}
