using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Device : BaseEntity
{
    public string Name { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; } = 4370;
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? FirmwareVersion { get; set; }
    public bool IsActive { get; set; } = true;
    public DeviceStatus Status { get; set; } = DeviceStatus.Offline;
    public DateTime? LastConnectedAt { get; set; }

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<DeviceLog> DeviceLogs { get; set; } = new List<DeviceLog>();

    private Device() { }

    public Device(string name, string ipAddress, int port, string? model, string? serialNumber)
    {
        Name = name;
        IPAddress = ipAddress;
        Port = port;
        Model = model;
        SerialNumber = serialNumber;
    }

    public void UpdateConnectionInfo(string ipAddress, int port)
    {
        IPAddress = ipAddress;
        Port = port;
        MarkUpdated();
    }

    public void UpdateFirmware(string? firmwareVersion)
    {
        FirmwareVersion = firmwareVersion;
        MarkUpdated();
    }

    public void MarkOnline()
    {
        Status = DeviceStatus.Online;
        LastConnectedAt = DateTime.UtcNow;
        MarkUpdated();
    }

    public void MarkOffline()
    {
        Status = DeviceStatus.Offline;
        MarkUpdated();
    }

    public void MarkError()
    {
        Status = DeviceStatus.Error;
        MarkUpdated();
    }

    public void ToggleActive(bool active)
    {
        IsActive = active;
        MarkUpdated();
    }
}
