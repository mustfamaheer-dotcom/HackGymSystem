using Gym.Shared.Enums;

namespace Gym.Domain.Events;

public class DeviceStatusChangedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public Guid DeviceId { get; set; }
    public string DeviceSerial { get; set; } = string.Empty;
    public DeviceStatus OldStatus { get; set; }
    public DeviceStatus NewStatus { get; set; }
}
