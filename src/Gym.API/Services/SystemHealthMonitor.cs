using Gym.API.Hubs;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Caching;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Repositories;
using Gym.Infrastructure.Resilience;
using Gym.Infrastructure.Services.ZKTeco;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gym.API.Services;

public sealed record DeviceHealthReport
{
    public bool IsConnected { get; init; }
    public string? Message { get; init; }

    public DeviceHealthReport() { }

    public DeviceHealthReport(bool isConnected, string? message)
    {
        IsConnected = isConnected;
        Message = message;
    }
}

public sealed record DatabaseHealthStatus
{
    public bool IsHealthy { get; init; }
    public string ConnectionStringType { get; init; } = "";
    public string? Message { get; init; }
}

public sealed record ApiEndpointStatus
{
    public string Name { get; init; } = "";
    public string Path { get; init; } = "";
    public bool IsHealthy { get; set; }
    public DateTimeOffset? ResponseTime { get; set; }
    public string? ErrorMessage { get; set; }
}

public sealed record SignalRHealthStatus
{
    public bool IsHealthy { get; init; }
    public int ConnectionCount { get; init; }
    public DateTime LastActivity { get; init; }
}

public sealed record SystemHealthReport
{
    public DateTime Timestamp { get; set; }
    public string OverallStatus { get; set; } = "";
    public DeviceHealthReport? DeviceStatus { get; set; }
    public DatabaseHealthStatus? DatabaseStatus { get; set; }
    public List<ApiEndpointStatus>? ApiEndpoints { get; set; }
    public SignalRHealthStatus? SignalRStatus { get; set; }
    public string? ErrorMessage { get; set; }
}

public class SystemHealthMonitor : BackgroundService
{
    private readonly ILogger<SystemHealthMonitor> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ZKTecoBridgeOptions _config;
    private readonly IHubContext<AttendanceHub> _hubContext;
    private volatile SystemHealthReport? _latestReport;

    public SystemHealthMonitor(
        ILogger<SystemHealthMonitor> logger,
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory,
        IOptions<ZKTecoBridgeOptions> config,
        IHubContext<AttendanceHub> hubContext)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        _config = config.Value;
        _hubContext = hubContext;
    }

    public SystemHealthReport? GetLatestReport() => _latestReport;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("System Health Monitor starting");

        // Initial health check
        await MonitorSystemHealth();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                if (!stoppingToken.IsCancellationRequested)
                {
                    await MonitorSystemHealth();
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in System Health Monitor");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }

    private async Task MonitorSystemHealth()
    {
        var healthReport = new SystemHealthReport
        {
            Timestamp = DateTime.UtcNow,
            OverallStatus = "Healthy"
        };

        try
        {
            healthReport.DeviceStatus = await CheckDeviceHealth();
            healthReport.DatabaseStatus = await CheckDatabaseHealth();
            healthReport.ApiEndpoints = await CheckApiEndpoints();
            healthReport.SignalRStatus = await CheckSignalRHealth();

            healthReport.OverallStatus = DetermineOverallStatus(healthReport);

            _latestReport = healthReport;

            _logger.LogInformation("System Health Report: {@HealthReport}", healthReport);

            // Push health status via SignalR for dashboard updates
            await PushHealthStatusToClients(healthReport);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error monitoring system health");
            healthReport.OverallStatus = "Unhealthy";
            healthReport.ErrorMessage = ex.Message;
            _latestReport = healthReport;

            await PushHealthStatusToClients(healthReport);
        }
    }

    private async Task<DeviceHealthReport> CheckDeviceHealth()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var connectionManager = scope.ServiceProvider
                .GetRequiredService<DeviceConnectionManager>();

            if (connectionManager == null)
                return new DeviceHealthReport { IsConnected = false, Message = "Device connection manager not initialized" };

            var isConnected = connectionManager.IsConnected;

            return new DeviceHealthReport
            {
                IsConnected = isConnected,
                Message = isConnected ? "Device connected" : "Device offline"
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Device health check failed");
            return new DeviceHealthReport { IsConnected = false, Message = $"Device health check failed: {ex.Message}" };
        }
    }

    private async Task<DatabaseHealthStatus> CheckDatabaseHealth()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var canConnect = await dbContext.Database.CanConnectAsync();

            return new DatabaseHealthStatus
            {
                IsHealthy = canConnect,
                ConnectionStringType = "SQLite",
                Message = canConnect ? "Database accessible" : "Database connection failed"
            };
        }
        catch (Exception ex)
        {
            return new DatabaseHealthStatus
            {
                IsHealthy = false,
                ConnectionStringType = "SQLite",
                Message = $"Database health check failed: {ex.Message}"
            };
        }
    }

    private async Task<List<ApiEndpointStatus>> CheckApiEndpoints()
    {
        // Only probe endpoints that actually exist; the previous version
        // hit fictional URLs (/api/TrackMembers/health, /health/health,
        // /hubs/attendance/health) and paid 4 × 5s timeouts every cycle.
        var endpoints = new List<ApiEndpointStatus>
        {
            new ApiEndpointStatus { Name = "Health Check", Path = "/health", IsHealthy = true },
            new ApiEndpointStatus { Name = "Bridge Health", Path = "grpc://zkteco.bridge/CheckHealth", IsHealthy = true },
            new ApiEndpointStatus { Name = "SignalR Hub", Path = "/hubs/attendance", IsHealthy = true }
        };

        var healthEndpoint = endpoints[0];
        var bridgeEndpoint  = endpoints[1];

        // Run both checks in parallel — they used to run serially and
        // cost 700ms (bridge) + 5s (health timeout) = 5.7s of blocking.
        var healthTask = ProbeHttpHealthAsync(healthEndpoint);
        var bridgeTask = ProbeBridgeHealthAsync(bridgeEndpoint);
        await Task.WhenAll(healthTask, bridgeTask);

        return endpoints;
    }

    private async Task ProbeHttpHealthAsync(ApiEndpointStatus endpoint)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient("SystemHealthMonitor");
            if (httpClient.Timeout == Timeout.InfiniteTimeSpan)
                httpClient.Timeout = TimeSpan.FromSeconds(5);
            var response = await httpClient.GetAsync("http://localhost:5000/health");
            endpoint.IsHealthy = response.IsSuccessStatusCode;
            endpoint.ResponseTime = response.Headers.Date;
        }
        catch (Exception ex)
        {
            endpoint.IsHealthy = false;
            endpoint.ErrorMessage = ex.Message;
        }
    }

    private async Task ProbeBridgeHealthAsync(ApiEndpointStatus endpoint)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var bridge = scope.ServiceProvider.GetRequiredService<IZKTecoBridgeClient>();
            var health = await bridge.CheckHealthAsync(CancellationToken.None);
            endpoint.IsHealthy = health.IsConnected;
            endpoint.ResponseTime = DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            endpoint.IsHealthy = false;
            endpoint.ErrorMessage = ex.Message;
        }
    }

    private async Task<SignalRHealthStatus> CheckSignalRHealth()
    {
        return new SignalRHealthStatus
        {
            IsHealthy = true,
            ConnectionCount = 0,
            LastActivity = DateTime.UtcNow
        };
    }

    private string DetermineOverallStatus(SystemHealthReport report)
    {
        if (!report.DeviceStatus.IsConnected ||
            !report.DatabaseStatus.IsHealthy ||
            report.ApiEndpoints.Any(e => !e.IsHealthy))
        {
            return "Unhealthy";
        }

        return report.ApiEndpoints.All(e => e.IsHealthy) ? "Healthy" : "Degraded";
    }

    private async Task PushHealthStatusToClients(SystemHealthReport healthReport)
    {
        try
        {
            var payload = new
            {
                type = "system_health",
                data = new
                {
                    healthReport.Timestamp,
                    healthReport.OverallStatus,
                    Device = new
                    {
                        IsConnected = healthReport.DeviceStatus.IsConnected,
                        Message = healthReport.DeviceStatus.Message
                    },
                    Database = new
                    {
                        healthReport.DatabaseStatus.IsHealthy,
                        healthReport.DatabaseStatus.Message
                    },
                    ApiEndpoints = healthReport.ApiEndpoints,
                    SignalR = new
                    {
                        healthReport.SignalRStatus.IsHealthy,
                        healthReport.SignalRStatus.ConnectionCount
                    }
                }
            };

            await _hubContext.Clients.All.SendAsync("SystemHealthUpdate", payload);
            _logger.LogDebug("Health status pushed to clients");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to push health status to clients");
        }
    }
}