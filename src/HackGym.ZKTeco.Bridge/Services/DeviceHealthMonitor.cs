using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

public class DeviceHealthMonitor : BackgroundService
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<DeviceHealthMonitor> _logger;
    private readonly BridgeWebSocketClient _wsClient;
    private readonly ZKTecoConfig _config;
    private static readonly TimeSpan HealthCheckInterval = TimeSpan.FromSeconds(30);
    private bool _lastKnownConnected;

    public DeviceHealthMonitor(
        ZKDeviceManager deviceManager,
        ILogger<DeviceHealthMonitor> logger,
        BridgeWebSocketClient wsClient,
        IOptions<ZKTecoConfig> config)
    {
        _deviceManager = deviceManager;
        _logger = logger;
        _wsClient = wsClient;
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

                if (currentlyConnected != _lastKnownConnected)
                {
                    await PushStatusAsync(currentlyConnected, stoppingToken);
                    _lastKnownConnected = currentlyConnected;
                }

                if (currentlyConnected)
                {
                    await PushStatusAsync(true, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring device health");
            }

            await Task.Delay(HealthCheckInterval, stoppingToken);
        }
    }

    private async Task PushStatusAsync(bool isConnected, CancellationToken ct)
    {
        try
        {
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

            var payload = new
            {
                enrolledUserCount = count,
                freeMemory = memory,
                firmwareVersion = firmware,
                isConnected,
                ipAddress = _config.DeviceIp,
                port = _config.DevicePort
            };

            var type = isConnected ? "heartbeat" : "device_offline";
            await _wsClient.SendMessageAsync(type, payload, ct);
            _logger.LogInformation("Device status pushed via WS: {Type} (Users: {Count}, FW: {FW})",
                type, count, firmware);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error pushing device status via WS");
        }
    }
}
