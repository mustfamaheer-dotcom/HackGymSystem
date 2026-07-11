using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Logging;

public static class DeviceLoggingExtensions
{
    public static void LogDeviceConnected(this ILogger logger, string serial, string ip, int port)
    {
        logger.LogInformation("[DEVICE] Connected: {Serial} at {IP}:{Port}", serial, ip, port);
    }

    public static void LogDeviceDisconnected(this ILogger logger, string serial, string reason)
    {
        logger.LogWarning("[DEVICE] Disconnected: {Serial} - {Reason}", serial, reason);
    }

    public static void LogDeviceStatusChanged(this ILogger logger, string serial, string oldStatus, string newStatus)
    {
        logger.LogInformation("[DEVICE] Status changed: {Serial} {OldStatus} -> {NewStatus}",
            serial, oldStatus, newStatus);
    }

    public static void LogAttendanceProcessed(this ILogger logger, Guid memberId, string memberName, string status, DateTime timestamp)
    {
        logger.LogInformation(
            "[ATTENDANCE] Member {MemberId} ({MemberName}) {Status} at {Timestamp:HH:mm:ss}",
            memberId, memberName, status, timestamp);
    }

    public static void LogSyncResult(this ILogger logger, string serial, int inserted, int skipped)
    {
        logger.LogInformation(
            "[SYNC] {Serial}: {Inserted} inserted, {Skipped} skipped",
            serial, inserted, skipped);
    }

    public static void LogDeviceError(this ILogger logger, string serial, Exception ex)
    {
        logger.LogError(ex, "[ERROR] Device {Serial} error", serial);
    }

    public static void LogBridgePush(this ILogger logger, string enrollmentId, string direction, bool success)
    {
        logger.LogInformation(
            "[BRIDGE] Push {EnrollmentId} {Direction}: {Success}",
            enrollmentId, direction, success ? "OK" : "FAILED");
    }

    public static void LogCircuitBreakerState(this ILogger logger, string deviceSerial, string state)
    {
        logger.LogWarning("[CIRCUIT] Device {Serial}: Circuit breaker is {State}", deviceSerial, state);
    }

    public static void LogRetryAttempt(this ILogger logger, int attempt, int maxRetries, int delayMs, Exception? ex = null)
    {
        if (ex != null)
            logger.LogWarning(ex, "[RETRY] Attempt {Attempt}/{MaxRetries} failed. Retrying in {DelayMs}ms...",
                attempt, maxRetries, delayMs * attempt);
        else
            logger.LogWarning("[RETRY] Attempt {Attempt}/{MaxRetries}. Waiting {DelayMs}ms...",
                attempt, maxRetries, delayMs * attempt);
    }
}
