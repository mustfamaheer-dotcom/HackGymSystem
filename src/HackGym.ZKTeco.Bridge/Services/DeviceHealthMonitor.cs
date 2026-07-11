using System.Net.Http.Json;
using System.Text.Json.Serialization;
using HackGym.ZKTeco.Bridge;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

public class DeviceHealthMonitor : BackgroundService
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<DeviceHealthMonitor> _logger;
    private readonly HttpClient _httpClient;
    private readonly ZKTecoConfig _config;
    private static readonly TimeSpan HealthCheckInterval = TimeSpan.FromSeconds(30);
    private bool _lastKnownConnected;

    public DeviceHealthMonitor(
        ZKDeviceManager deviceManager,
        ILogger<DeviceHealthMonitor> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<ZKTecoConfig> config)
    {
        _deviceManager = deviceManager;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("MainApi");
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Device health monitor starting");
        _lastKnownConnected = _deviceManager.IsConnected;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentlyConnected = _deviceManager.IsConnected;

                if (currentlyConnected)
                {
                    var (count, memory, firmware) = _deviceManager.GetDeviceStatus();
                    _logger.LogDebug(
                        "Device health - Connected: {Connected}, Users: {Count}, FreeMem: {Memory}KB, FW: {Firmware}",
                        currentlyConnected, count, memory, firmware ?? "N/A");

                    if (_deviceManager.ConnectionInfo.ConsecutiveFailures > 10)
                    {
                        _logger.LogWarning("Device has been failing for {Count} consecutive checks",
                            _deviceManager.ConnectionInfo.ConsecutiveFailures);
                    }
                }
                else
                {
                    _logger.LogWarning("Device is offline, attempting reconnection...");
                    if (_deviceManager.Connect())
                    {
                        _logger.LogInformation("Device reconnected successfully");
                    }
                    else
                    {
                        _logger.LogWarning("Reconnection attempt failed, will retry in {Interval}s",
                            HealthCheckInterval.TotalSeconds);
                    }
                }

                // Detect state changes and push to API
                if (currentlyConnected != _lastKnownConnected)
                {
                    await PushStatusChangeAsync(currentlyConnected, stoppingToken);
                    _lastKnownConnected = currentlyConnected;
                }
                
                // Also push periodic heartbeat when connected
                if (currentlyConnected)
                {
                    await PushStatusChangeAsync(true, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring device health");
            }

            await Task.Delay(HealthCheckInterval, stoppingToken);
        }
    }

    private async Task PushStatusChangeAsync(bool isConnected, CancellationToken ct)
    {
        try
        {
            var endpoint = isConnected
                ? "/api/zkteco-attendance/heartbeat"
                : "/api/zkteco-attendance/device-offline";

            int count = 0;
            long memory = 0;
            string firmware = "";

            if (isConnected)
            {
                var (c, m, f) = _deviceManager.GetDeviceStatus();
                count = c;
                memory = m;
                firmware = f ?? "";
            }

            var payload = new DeviceInfoPayload
            {
                Model = "ZKMB2000",
                SerialNumber = string.Empty,
                FirmwareVersion = firmware,
                EnrolledUserCount = count,
                FreeMemory = memory,
                IpAddress = _config.DeviceIp,
                Port = _config.DevicePort
            };

            var response = await _httpClient.PostAsJsonAsync(endpoint, payload, ct);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Device status change pushed to API: {Status} (Users: {Count}, FW: {FW})",
                    isConnected ? "Online" : "Offline", count, firmware);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Failed to push device status change: {Status} {Error}",
                    response.StatusCode, error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error pushing device status change");
        }
    }

    private class DeviceInfoPayload
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "ZKMB2000";

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
}
