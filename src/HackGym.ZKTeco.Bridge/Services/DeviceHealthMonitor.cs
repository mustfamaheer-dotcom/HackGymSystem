using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HackGym.ZKTeco.Bridge.Services;

public class DeviceHealthMonitor : BackgroundService
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<DeviceHealthMonitor> _logger;
    private static readonly TimeSpan HealthCheckInterval = TimeSpan.FromSeconds(30);

    public DeviceHealthMonitor(ZKDeviceManager deviceManager, ILogger<DeviceHealthMonitor> logger)
    {
        _deviceManager = deviceManager;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Device health monitor starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_deviceManager.IsConnected)
                {
                    var (count, memory, firmware) = _deviceManager.GetDeviceStatus();
                    _logger.LogInformation(
                        "Device health - Connected: {Connected}, Users: {Count}, FreeMem: {Memory}KB, FW: {Firmware}",
                        _deviceManager.IsConnected, count, memory, firmware ?? "N/A");

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring device health");
            }

            await Task.Delay(HealthCheckInterval, stoppingToken);
        }
    }
}
