namespace Gym.Application.Common.Interfaces;

public class EnrollmentResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class DeviceHealthStatus
{
    public bool IsConnected { get; set; }
    public int EnrolledUserCount { get; set; }
    public long FreeMemory { get; set; }
    public string? FirmwareVersion { get; set; }
    public long UptimeMs { get; set; }
}

public class TestConnectionResult
{
    public bool Success { get; set; }
    public long RoundTripLatencyMs { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface IZKTecoBridgeClient
{
    Task<DeviceHealthStatus> CheckHealthAsync(CancellationToken cancellationToken = default);
    Task<bool> SetUserPrivilegeAsync(string enrollmentId, int privilege, DateTime? expiryDate = null, CancellationToken cancellationToken = default);
    Task<EnrollmentResult> EnrollFingerprintAsync(string memberId, string enrollmentId, int fingerIndex, int timeoutSeconds = 60, CancellationToken cancellationToken = default);
    Task<EnrollmentResult> EnrollFaceAsync(string memberId, string enrollmentId, int timeoutSeconds = 60, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(string enrollmentId, CancellationToken cancellationToken = default);
    Task<DeviceHealthStatus> GetDeviceStatusAsync(CancellationToken cancellationToken = default);
    Task<TestConnectionResult> TestConnectionAsync(CancellationToken cancellationToken = default);
}
