namespace Gym.Application.ZKTeco.DTOs;

public class DeviceStatusDto
{
    public bool IsConnected { get; set; }
    public int EnrolledUserCount { get; set; }
    public long FreeMemory { get; set; }
    public string? FirmwareVersion { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTime? LastConnectedAt { get; set; }
    public long UptimeMs { get; set; }
    public int MaxFingerprints { get; set; } = 3000;
    public int MaxFaces { get; set; } = 500;
}
