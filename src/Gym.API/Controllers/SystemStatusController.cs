using Gym.API.Filters;
using Gym.API.Hubs;
using Gym.API.Services;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Repositories;
using Gym.Infrastructure.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gym.API.Controllers;

[Authorize]
[Route("api/system")]
[ApiController]
public class SystemStatusController : BaseController
{
    private readonly SystemHealthMonitor _healthMonitor;
    private readonly DeviceConnectionManager _connectionManager;
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<AttendanceHub> _hubContext;

    public SystemStatusController(
        SystemHealthMonitor healthMonitor,
        DeviceConnectionManager connectionManager,
        IZKTecoBridgeClient bridgeClient,
        IConfiguration configuration,
        IHubContext<AttendanceHub> hubContext)
    {
        _healthMonitor = healthMonitor;
        _connectionManager = connectionManager;
        _bridgeClient = bridgeClient;
        _configuration = configuration;
        _hubContext = hubContext;
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSystemHealth()
    {
        var report = _healthMonitor.GetLatestReport();
        if (report == null)
        {
            return Ok(ApiResponse<object>.Ok(new
            {
                timestamp = DateTime.UtcNow,
                overallStatus = "Starting",
                message = "Health monitor still initializing"
            }));
        }

        return Ok(ApiResponse<object>.Ok(report));
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSystemStatus()
    {
        DeviceHealthStatus? bridgeHealth = null;
        
        try
        {
            bridgeHealth = await _bridgeClient.CheckHealthAsync(CancellationToken.None);
        }
        catch { /* ignore */ }

        var status = new
        {
            timestamp = DateTime.UtcNow,
            overallStatus = "Operational",
            services = new[]
            {
                new { name = "Gym API", status = "Operational", port = 5000, endpoint = "" },
                new { name = "ZKTeco Bridge", status = "Operational", port = 50054, endpoint = "" },
                new { name = "SignalR Hub", status = "Operational", port = 0, endpoint = "/hubs/attendance" },
                new { name = "Next.js Dashboard", status = "Free", port = 3000, endpoint = "" }
            },
            deviceStatus = new
            {
                isConnected = _connectionManager.IsConnected,
                ip = _configuration["ZKTeco:DeviceIp"] ?? "192.168.1.201",
                port = _configuration.GetValue<int>("ZKTeco:DevicePort", 4370),
                bridgeConnected = bridgeHealth?.IsConnected ?? false
            },
            stats = new
            {
                totalMembers = await GetTotalMembersAsync(),
                checkedInToday = await GetCheckedInTodayAsync(),
                deviceConnected = _connectionManager.IsConnected
            },
            features = new[]
            {
                new { name = "Real-time Attendance", status = "Active" },
                new { name = "Manual Testing", status = "Available" },
                new { name = "Device Health Monitoring", status = "Active" },
                new { name = "System Diagnostics", status = "Active" }
            }
        };

        return Ok(ApiResponse<object>.Ok(status));
    }

    [HttpGet("manual-testing")]
    [AllowAnonymous]
    public IActionResult GetManualTestingInfo()
    {
        var manualTestingInfo = new
        {
            enabled = true,
            description = "Manual attendance recording when ZK device is offline",
            endpoints = new[]
            {
                new { method = "POST", path = "/api/TrackMembers/manual-check-in", description = "Record manual check-in" },
                new { method = "POST", path = "/api/TrackMembers/manual-check-out", description = "Record manual check-out" },
                new { method = "GET", path = "/api/TrackMembers/manual-status", description = "Get manual testing status" }
            },
            usageExample = new
            {
                checkIn = new
                {
                    endpoint = "/api/TrackMembers/manual-check-in",
                    method = "POST",
                    body = "{ \"memberId\": \"your-member-id-here\" }",
                    description = "Record a manual check-in for a member"
                },
                checkOut = new
                {
                    endpoint = "/api/TrackMembers/manual-check-out",
                    method = "POST",
                    body = "{ \"memberId\": \"your-member-id-here\" }",
                    description = "Record a manual check-out for a member"
                }
            }
        };

        return Ok(ApiResponse<object>.Ok(manualTestingInfo));
    }

    [HttpGet("diagnostics")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDiagnostics()
    {
        var bridgeHealth = (DeviceHealthStatus?)null;
        try
        {
            bridgeHealth = await _bridgeClient.CheckHealthAsync(CancellationToken.None);
        }
        catch { /* ignore */ }

        var deviceManagerInfo = new
        {
            isConnected = _connectionManager.IsConnected,
            circuitState = _connectionManager.CircuitState.ToString(),
            config = new
            {
                deviceIp = _configuration["ZKTeco:DeviceIp"] ?? "192.168.1.201",
                devicePort = _configuration.GetValue<int>("ZKTeco:DevicePort", 4370),
                connectionTimeoutMs = _configuration.GetValue<int>("ZKTeco:ConnectionTimeoutMs", 5000)
            }
        };

        var signalRInfo = new
        {
            hubEndpoint = "/hubs/attendance",
            activeConnections = 0 // Would need connection tracking to get actual count
        };

        var diagnostics = new
        {
            timestamp = DateTime.UtcNow,
            deviceManager = deviceManagerInfo,
            bridge = bridgeHealth,
            signalR = signalRInfo,
            environment = new
            {
                aspnetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                machineName = Environment.MachineName,
                osVersion = Environment.OSVersion.ToString(),
                dotnetVersion = Environment.Version.ToString()
            },
            troubleshooting = new
            {
                commonIssues = new[]
                {
                    "Device unreachable: Check physical connection and IP address",
                    "Port 4370 blocked: Verify firewall rules",
                    "Device powered off: Check power supply",
                    "Network segmentation: Ensure device and server are on same network"
                }
            }
        };

        return Ok(ApiResponse<object>.Ok(diagnostics));
    }

    private async Task<int> GetTotalMembersAsync()
    {
        try
        {
            using var scope = HttpContext.RequestServices.CreateScope();
            var memberRepo = scope.ServiceProvider.GetRequiredService<IRepository<Member>>();
            return await memberRepo.CountAsync(m => !m.IsDeleted, CancellationToken.None);
        }
        catch { return 0; }
    }

    private async Task<int> GetCheckedInTodayAsync()
    {
        try
        {
            using var scope = HttpContext.RequestServices.CreateScope();
            var attendanceTracking = scope.ServiceProvider.GetRequiredService<IAttendanceTrackingRepository>();
            return await attendanceTracking.GetTodayCheckInCountAsync(CancellationToken.None);
        }
        catch { return 0; }
    }
}