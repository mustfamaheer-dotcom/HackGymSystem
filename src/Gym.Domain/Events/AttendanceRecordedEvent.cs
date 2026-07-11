namespace Gym.Domain.Events;

public class AttendanceRecordedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AttendanceTimestamp { get; set; }
    public string? DeviceSerial { get; set; }
    public string Method { get; set; } = string.Empty;
    public bool IsManual { get; set; }
    public string Direction { get; set; } = string.Empty;
}
