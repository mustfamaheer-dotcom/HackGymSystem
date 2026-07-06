namespace HackGym.ZKTeco.Bridge.Models;

public class DeviceConnectionInfo
{
    public bool IsConnected { get; set; }
    public int MachineNumber { get; set; } = 1;
    public int EnrolledUserCount { get; set; }
    public long FreeMemory { get; set; }
    public string? FirmwareVersion { get; set; }
    public DateTime LastConnectedAt { get; set; }
    public int ConsecutiveFailures { get; set; }
    public TimeSpan CurrentBackoffDelay { get; set; } = TimeSpan.FromSeconds(10);
}
