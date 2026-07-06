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

                var events = _deviceManager.GetNewLogs();
                _logger.LogDebug("Polled {Count} attendance events", events.Count);

                var now = DateTime.UtcNow;
                foreach (var evt in events)
                {
                    var dedupKey = $"{evt.EnrollmentId}_{evt.Timestamp:O}_{evt.Direction}";
                    if (_processedLogs.TryGetValue(dedupKey, out var cachedTime) && (now - cachedTime) < _dedupWindow)
                        continue;

                    _processedLogs[dedupKey] = now;

                    try
                    {
                        await ProcessAttendanceEvent(evt, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process attendance event for {EnrollmentId}", evt.EnrollmentId);
                    }
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
                return;

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
}
