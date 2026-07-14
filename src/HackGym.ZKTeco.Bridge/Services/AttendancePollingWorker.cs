using HackGym.ZKTeco.Bridge.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

public class AttendancePollingWorker : BackgroundService
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<AttendancePollingWorker> _logger;
    private readonly ZKTecoConfig _config;
    private readonly BridgeWebSocketClient _wsClient;
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, DateTime> _processedLogs = new();
    private readonly TimeSpan _dedupWindow = TimeSpan.FromHours(1);
    private DateTime _lastLogTimestamp = DateTime.MinValue;
    private bool _deviceInfoSynced;
    private bool _usersSynced;
    private DateTime _lastHeartbeat = DateTime.MinValue;
    private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromSeconds(30);

    public AttendancePollingWorker(
        ZKDeviceManager deviceManager,
        ILogger<AttendancePollingWorker> logger,
        IOptions<ZKTecoConfig> config,
        BridgeWebSocketClient wsClient)
    {
        _deviceManager = deviceManager;
        _logger = logger;
        _config = config.Value;
        _wsClient = wsClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Attendance polling worker starting");

        await ConnectWithRetryAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!_deviceManager.IsConnected)
                {
                    await ReconnectWithBackoffAsync(stoppingToken);
                    continue;
                }

                if (!_deviceInfoSynced)
                {
                    await SyncDeviceInfoAsync(stoppingToken);
                }
                if (!_usersSynced)
                {
                    await SyncUsersAsync(stoppingToken);
                }

                if ((DateTime.UtcNow - _lastHeartbeat) >= HeartbeatInterval)
                {
                    await PushHeartbeatAsync(stoppingToken);
                    _lastHeartbeat = DateTime.UtcNow;
                }

                var events = _deviceManager.GetNewLogs();
                _logger.LogDebug("Polled {Count} attendance events (last timestamp: {Last})", events.Count, _lastLogTimestamp);

                var maxTimestamp = _lastLogTimestamp;
                var now = DateTime.UtcNow;
                var processedCount = 0;
                var failedCount = 0;
                foreach (var evt in events)
                {
                    if (evt.Timestamp <= _lastLogTimestamp)
                        continue;

                    if (evt.Timestamp > maxTimestamp)
                        maxTimestamp = evt.Timestamp;

                    var dedupKey = $"{evt.EnrollmentId}_{evt.Timestamp:O}_{evt.Direction}";
                    if (_processedLogs.TryGetValue(dedupKey, out var cachedTime) && (now - cachedTime) < _dedupWindow)
                        continue;

                    _processedLogs[dedupKey] = now;

                    try
                    {
                        await ProcessAttendanceEvent(evt, stoppingToken);
                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process attendance event for {EnrollmentId}", evt.EnrollmentId);
                        failedCount++;
                    }
                }

                _lastLogTimestamp = maxTimestamp;

                if (processedCount > 0 && failedCount == 0)
                {
                    await ClearDeviceAttendanceAsync();
                }
                else if (failedCount > 0)
                {
                    _logger.LogWarning("Skipping log clear: {Failed}/{Total} events failed to push",
                        failedCount, processedCount + failedCount);
                }

                if (_processedLogs.Count > 1000)
                {
                    var cutoff = now - _dedupWindow;
                    foreach (var kvp in _processedLogs)
                        if (kvp.Value < cutoff)
                            _processedLogs.TryRemove(kvp.Key, out _);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in attendance polling loop");
            }

            await Task.Delay(_config.PollingIntervalMs, stoppingToken);
        }
    }

    private async Task SyncDeviceInfoAsync(CancellationToken ct)
    {
        try
        {
            var deviceInfo = _deviceManager.GetDeviceInfo();
            if (deviceInfo == null)
            {
                _logger.LogWarning("Failed to get device info");
                return;
            }

            var payload = new
            {
                model = deviceInfo.Model,
                serialNumber = deviceInfo.SerialNumber,
                firmwareVersion = deviceInfo.FirmwareVersion,
                enrolledUserCount = deviceInfo.EnrolledUserCount,
                freeMemory = deviceInfo.FreeMemory,
                ipAddress = _config.DeviceIp,
                port = _config.DevicePort
            };

            await _wsClient.SendMessageAsync("sync_device_info", payload, ct);
            _deviceInfoSynced = true;
            _logger.LogInformation("Device info synced via WS: {Model}, SN: {Serial}, FW: {Firmware}",
                deviceInfo.Model, deviceInfo.SerialNumber, deviceInfo.FirmwareVersion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing device info");
        }
    }

    private async Task PushHeartbeatAsync(CancellationToken ct)
    {
        try
        {
            var (count, memory, firmware) = _deviceManager.GetDeviceStatus();
            var payload = new
            {
                enrolledUserCount = count,
                freeMemory = memory,
                firmwareVersion = firmware ?? "",
                isConnected = true,
                ipAddress = _config.DeviceIp,
                port = _config.DevicePort
            };

            await _wsClient.SendMessageAsync("heartbeat", payload, ct);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error pushing heartbeat via WS");
        }
    }

    private async Task SyncUsersAsync(CancellationToken ct)
    {
        try
        {
            var users = _deviceManager.GetAllUsersWithDetails();
            if (users.Count == 0)
            {
                _logger.LogWarning("No users found on device");
                _usersSynced = true;
                return;
            }

            var payload = users.Select(u => new
            {
                enrollmentId = u.EnrollmentId,
                name = u.Name,
                privilege = u.Privilege,
                enabled = u.Enabled
            }).ToList();

            await _wsClient.SendMessageAsync("sync_users", payload, ct);
            _usersSynced = true;
            _logger.LogInformation("Synced {Count} users from device via WS", users.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing users from device");
        }
    }

    private async Task ClearDeviceAttendanceAsync()
    {
        try
        {
            var cleared = _deviceManager.ClearAttendanceLogs();
            if (cleared)
                _logger.LogDebug("Device attendance logs cleared after sync");
            else
                _logger.LogWarning("Failed to clear device attendance logs");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing device attendance logs");
        }
    }

    private async Task ProcessAttendanceEvent(ZKAttendanceEvent evt, CancellationToken ct)
    {
        _logger.LogInformation("Processing attendance event: EnrollmentId={EnrollmentId}, Timestamp={Timestamp}, Direction={Direction}, Method={Method}",
            evt.EnrollmentId, evt.Timestamp, evt.Direction, evt.Method);

        var payload = new
        {
            enrollmentId = evt.EnrollmentId,
            timestamp = evt.Timestamp,
            direction = evt.Direction,
            verifyMethod = (int)evt.Method
        };

        var ack = await _wsClient.SendMessageAsync("attendance_push", payload, ct, waitForAck: true, ackTimeoutMs: 8000);
        if (ack == null || !ack.Success)
        {
            var err = ack?.Error ?? "WebSocket send failed";
            if (err.Contains("already checked in", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Attendance event already processed by API for EnrollmentId={EnrollmentId} — clearing device log to break retry loop", evt.EnrollmentId);
                return;
            }
            _logger.LogWarning("Attendance push FAILED for EnrollmentId={EnrollmentId}: {Error}", evt.EnrollmentId, err);
            throw new InvalidOperationException($"API rejected attendance push: {err}");
        }
        _logger.LogInformation("attendance_push confirmed by API for EnrollmentId={EnrollmentId}", evt.EnrollmentId);
    }

    private async Task ConnectWithRetryAsync(CancellationToken ct)
    {
        // Retry forever with capped exponential backoff. The previous version
        // gave up after MaxRetryAttempts and the worker sat idle — the page
        // would never recover even after the device came back online.
        var attempt = 0;
        while (!ct.IsCancellationRequested)
        {
            attempt++;
            if (_deviceManager.Connect())
            {
                _deviceInfoSynced = false;
                _usersSynced = false;
                _logger.LogInformation("Connected to device after {Attempt} attempt(s)", attempt);
                return;
            }

            var delay = Math.Min(300, Math.Pow(2, Math.Min(attempt, 8)) * 2);
            _logger.LogWarning("Connection attempt {Attempt} failed — retrying in {Delay}s", attempt, (int)delay);
            await Task.Delay(TimeSpan.FromSeconds((int)delay), ct);
        }
    }

    private async Task ReconnectWithBackoffAsync(CancellationToken ct)
    {
        // Keep trying — the previous version only attempted ONE reconnect per
        // poll cycle. If it failed, the worker would spin in a tight loop
        // calling RecordFailure() every 3 seconds, growing the backoff to
        // 300s which made the page appear dead for 5 minutes.
        _deviceManager.RecordFailure();
        var delay = _deviceManager.ConnectionInfo.CurrentBackoffDelay;

        _logger.LogInformation("Reconnecting to device (delay {Delay})", delay);

        if (_deviceManager.Connect())
        {
            _lastLogTimestamp = DateTime.MinValue;
            _deviceInfoSynced = false;
            _usersSynced = false;
            _logger.LogInformation("Reconnected to device after {Delay}s", delay.TotalSeconds);
        }
    }
}
