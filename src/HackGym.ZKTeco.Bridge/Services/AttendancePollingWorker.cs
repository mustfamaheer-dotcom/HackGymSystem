using System.Net.Http.Json;
using System.Text.Json.Serialization;
using HackGym.ZKTeco.Bridge.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

public class AttendancePollingWorker : BackgroundService
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<AttendancePollingWorker> _logger;
    private readonly ZKTecoConfig _config;
    private readonly HttpClient _httpClient;
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
        IHttpClientFactory httpClientFactory)
    {
        _deviceManager = deviceManager;
        _logger = logger;
        _config = config.Value;
        _httpClient = httpClientFactory.CreateClient("MainApi");
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

                // On first connect (or reconnect), sync device info and users
                if (!_deviceInfoSynced)
                {
                    await SyncDeviceInfoAsync(stoppingToken);
                }
                if (!_usersSynced)
                {
                    await SyncUsersAsync(stoppingToken);
                }

                // Periodic heartbeat - push device status to API every 30s
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
                    }
                }

                _lastLogTimestamp = maxTimestamp;

                // Clear device attendance logs after successful sync
                if (processedCount > 0)
                {
                    await ClearDeviceAttendanceAsync();
                }

                // Evict stale keys periodically (every ~300 events)
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

            var payload = new DeviceInfoPayload
            {
                Model = deviceInfo.Model,
                SerialNumber = deviceInfo.SerialNumber,
                FirmwareVersion = deviceInfo.FirmwareVersion,
                EnrolledUserCount = deviceInfo.EnrolledUserCount,
                FreeMemory = deviceInfo.FreeMemory,
                IpAddress = _config.DeviceIp,
                Port = _config.DevicePort
            };

            var response = await _httpClient.PostAsJsonAsync("/api/zkteco-attendance/device-info", payload, ct);
            if (response.IsSuccessStatusCode)
            {
                _deviceInfoSynced = true;
                _logger.LogInformation("Device info synced: {Model}, SN: {Serial}, FW: {Firmware}",
                    deviceInfo.Model, deviceInfo.SerialNumber, deviceInfo.FirmwareVersion);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Device info sync failed: {Status} {Error}", response.StatusCode, error);
            }
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
            var payload = new DeviceInfoPayload
            {
                Model = "ZKMB2000",
                SerialNumber = "",
                FirmwareVersion = firmware ?? "",
                EnrolledUserCount = count,
                FreeMemory = memory,
                IpAddress = _config.DeviceIp,
                Port = _config.DevicePort
            };

            var response = await _httpClient.PostAsJsonAsync("/api/zkteco-attendance/heartbeat", payload, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Heartbeat push failed: {Status} {Error}", response.StatusCode, error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error pushing heartbeat");
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

            var payload = users.Select(u => new UserSyncPayload
            {
                EnrollmentId = u.EnrollmentId,
                Name = u.Name,
                Privilege = u.Privilege,
                Enabled = u.Enabled
            }).ToList();

            var response = await _httpClient.PostAsJsonAsync("/api/zkteco-attendance/sync-users", payload, ct);
            if (response.IsSuccessStatusCode)
            {
                _usersSynced = true;
                _logger.LogInformation("Synced {Count} users from device", users.Count);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("User sync failed: {Status} {Error}", response.StatusCode, error);
            }
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
        var payload = new AttendanceEventPayload
        {
            EnrollmentId = evt.EnrollmentId,
            Timestamp = evt.Timestamp,
            Direction = evt.Direction,
            VerifyMethod = (int)evt.Method
        };

        var response = await _httpClient.PostAsJsonAsync("/api/zkteco-attendance/push", payload, ct);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            _logger.LogWarning("Attendance push failed for {EnrollmentId}: {Status} {Error}",
                evt.EnrollmentId, response.StatusCode, error);
        }
    }

    private async Task ConnectWithRetryAsync(CancellationToken ct)
    {
        var attempts = 0;
        while (!ct.IsCancellationRequested && attempts < _config.MaxRetryAttempts)
        {
            if (_deviceManager.Connect())
            {
                _deviceInfoSynced = false;
                _usersSynced = false;
                return;
            }

            attempts++;
            _logger.LogWarning("Connection attempt {Attempt}/{Max} failed", attempts, _config.MaxRetryAttempts);
            await Task.Delay(_config.RetryDelayMs, ct);
        }

        _logger.LogError("Failed to connect to device after {Max} attempts", _config.MaxRetryAttempts);
    }

    private async Task ReconnectWithBackoffAsync(CancellationToken ct)
    {
        _logger.LogInformation("Attempting reconnection...");
        _deviceManager.RecordFailure();
        var delay = _deviceManager.ConnectionInfo.CurrentBackoffDelay;

        await Task.Delay(delay, ct);

        if (_deviceManager.Connect())
        {
            _lastLogTimestamp = DateTime.MinValue;
            _deviceInfoSynced = false;
            _usersSynced = false;
            _logger.LogInformation("Reconnected successfully after {Delay}", delay);
        }
    }

    private class AttendanceEventPayload
    {
        [JsonPropertyName("enrollmentId")]
        public string EnrollmentId { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("direction")]
        public int Direction { get; set; }

        [JsonPropertyName("verifyMethod")]
        public int VerifyMethod { get; set; }
    }

    private class DeviceInfoPayload
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonPropertyName("enrolledUserCount")]
        public int EnrolledUserCount { get; set; }

        [JsonPropertyName("freeMemory")]
        public long FreeMemory { get; set; }

        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public int Port { get; set; }
    }

    private class UserSyncPayload
    {
        [JsonPropertyName("enrollmentId")]
        public string EnrollmentId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("privilege")]
        public int Privilege { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
