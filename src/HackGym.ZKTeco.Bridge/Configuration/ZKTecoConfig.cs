namespace HackGym.ZKTeco.Bridge;

public class ZKTecoConfig
{
    public string DeviceIp { get; set; } = "192.168.1.201";
    public int DevicePort { get; set; } = 4370;
    public int ConnectionTimeoutMs { get; set; } = 5000;
    public int PollingIntervalMs { get; set; } = 3000;
    public int MaxRetryAttempts { get; set; } = 5;
    public int RetryDelayMs { get; set; } = 10000;
    public bool FingerprintDuplicateCheck { get; set; } = true;
    public bool AutoSyncOnStartup { get; set; } = true;
}
