using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class DeviceLog : BaseEntity
{
    public Guid DeviceId { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }

    public Device Device { get; set; } = null!;

    private DeviceLog() { }

    public DeviceLog(Guid deviceId, string level, string message, string? details = null)
    {
        DeviceId = deviceId;
        Level = level;
        Message = message;
        Details = details;
    }
}
