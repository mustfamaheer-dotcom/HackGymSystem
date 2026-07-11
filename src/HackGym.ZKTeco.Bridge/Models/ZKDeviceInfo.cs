namespace HackGym.ZKTeco.Bridge.Models;

public class ZKDeviceInfo
{
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string FirmwareVersion { get; set; } = string.Empty;
    public int EnrolledUserCount { get; set; }
    public long FreeMemory { get; set; }
}

public class ZKUserInfo
{
    public string EnrollmentId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Privilege { get; set; }
    public bool Enabled { get; set; }
    public bool IsAdmin => Privilege > 0;
}
