using System.ComponentModel.DataAnnotations;

namespace Gym.Application.Common.Interfaces;

public class ZKTecoSettings
{
    [Required, RegularExpression(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")]
    public string DeviceIp { get; set; } = "192.168.1.201";

    [Range(1, 65535)]
    public int DevicePort { get; set; } = 4370;

    [Range(1000, 30000)]
    public int ConnectionTimeoutMs { get; set; } = 5000;

    [Range(500, 30000)]
    public int PollingIntervalMs { get; set; } = 3000;

    [Range(1, 10)]
    public int MaxRetryAttempts { get; set; } = 5;

    [Range(1000, 60000)]
    public int RetryDelayMs { get; set; } = 10000;

    public bool FingerprintDuplicateCheck { get; set; } = true;
    public bool AutoSyncOnStartup { get; set; } = true;
    public bool RequireActiveSubscriptionForAttendance { get; set; } = true;
    public string BridgeBaseUrl { get; set; } = "http://localhost:50054";
}
