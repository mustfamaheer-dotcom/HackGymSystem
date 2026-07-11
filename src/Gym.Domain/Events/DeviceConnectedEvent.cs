namespace Gym.Domain.Events;

public class DeviceConnectedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public Guid DeviceId { get; set; }
    public string DeviceSerial { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
}
