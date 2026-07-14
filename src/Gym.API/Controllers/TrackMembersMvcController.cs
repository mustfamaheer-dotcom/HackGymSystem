using Gym.API.Filters;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace Gym.API.Controllers;

[Authorize]
[Route("TrackMembers")]
public class TrackMembersMvcController : Controller
{
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<Device> _deviceRepo;
    private readonly IRepository<AttendanceSummary> _summaryRepo;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<TrackMembersMvcController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public TrackMembersMvcController(
        IRepository<Member> memberRepo,
        IRepository<Attendance> attendanceRepo,
        IRepository<Device> deviceRepo,
        IRepository<AttendanceSummary> summaryRepo,
        IDeviceMemberMappingRepository mappingRepo,
        IZKTecoBridgeClient bridgeClient,
        IStringLocalizer<SharedResources> localizer,
        ILogger<TrackMembersMvcController> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _memberRepo = memberRepo;
        _attendanceRepo = attendanceRepo;
        _deviceRepo = deviceRepo;
        _summaryRepo = summaryRepo;
        _mappingRepo = mappingRepo;
        _bridgeClient = bridgeClient;
        _localizer = localizer;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [RequirePermission("Attendance.View")]
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Track Members"];

        var start = DateTime.UtcNow.Date;
        var end = start.AddDays(1);
        var lateThreshold = new DateTime(start.Year, start.Month, start.Day, 9, 15, 0, DateTimeKind.Utc);

        // Single round-trip: get all counts in one SQL query (avoids SQLite
        // lock contention from parallel CountAsync calls).
        var stats = await _memberRepo.Query()
            .AsNoTracking()
            .Select(m => new
            {
                TotalMembers = _memberRepo.Query().Count(x => !x.IsDeleted),
                CheckedInToday = _attendanceRepo.Query().Count(a => a.CheckIn >= start && a.CheckIn < end),
                LateToday = _attendanceRepo.Query().Count(a => a.CheckIn >= start && a.CheckIn < end && a.CheckIn > lateThreshold),
                DevicesOnline = _deviceRepo.Query().Count(d => d.IsActive && d.Status == Shared.Enums.DeviceStatus.Online)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var totalMembers = stats?.TotalMembers ?? 0;
        var checkedInToday = stats?.CheckedInToday ?? 0;

        var todayAttendances = await _attendanceRepo.Query()
            .AsNoTracking()
            .Where(a => a.CheckIn >= start && a.CheckIn < end)
            .OrderByDescending(a => a.CheckIn)
            .Take(50)
            .Select(a => new
            {
                Id = a.Id,
                MemberId = a.MemberId,
                MemberName = a.Member.FullName,
                MemberCode = a.Member.Code,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                DeviceName = a.Device != null ? a.Device.Name : "MANUAL",
                IsManual = a.IsManual
            })
            .ToListAsync(cancellationToken);

        ViewBag.TotalMembers = totalMembers;
        ViewBag.CheckedInToday = checkedInToday;
        ViewBag.AbsentToday = totalMembers - checkedInToday;
        ViewBag.LateToday = stats?.LateToday ?? 0;
        ViewBag.DevicesOnline = stats?.DevicesOnline ?? 0;
        ViewBag.TotalRecordsToday = checkedInToday;
        ViewBag.OnLeaveToday = 0;
        ViewBag.TodayAttendances = todayAttendances;

        return View();
    }

    [HttpGet("get-live-data")]
    public async Task<IActionResult> GetLiveData(CancellationToken cancellationToken)
    {
        var start = DateTime.UtcNow.Date;
        var end = start.AddDays(1);
        var lateThreshold = new DateTime(start.Year, start.Month, start.Day, 9, 15, 0, DateTimeKind.Utc);

        // Single round-trip aggregate — SQLite handles this far better than
        // 4-5 parallel CountAsync calls that fight for the read lock.
        var stats = await _memberRepo.Query()
            .AsNoTracking()
            .Select(m => new
            {
                TotalMembers = _memberRepo.Query().Count(x => !x.IsDeleted),
                CheckedInToday = _attendanceRepo.Query().Count(a => a.CheckIn >= start && a.CheckIn < end),
                LateToday = _attendanceRepo.Query().Count(a => a.CheckIn >= start && a.CheckIn < end && a.CheckIn > lateThreshold),
                DevicesOnline = _deviceRepo.Query().Count(d => d.IsActive && d.Status == Shared.Enums.DeviceStatus.Online)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var totalMembers = stats?.TotalMembers ?? 0;
        var checkedInToday = stats?.CheckedInToday ?? 0;

        var todayAttendances = await _attendanceRepo.Query()
            .AsNoTracking()
            .Where(a => a.CheckIn >= start && a.CheckIn < end)
            .OrderByDescending(a => a.CheckIn)
            .Take(50)
            .Select(a => new
            {
                id = a.Id,
                memberId = a.MemberId,
                memberName = a.Member.FullName,
                memberCode = a.Member.Code,
                checkIn = a.CheckIn,
                checkOut = a.CheckOut,
                deviceName = a.Device != null ? a.Device.Name : "MANUAL",
                isManual = a.IsManual
            })
            .ToListAsync(cancellationToken);

        return Json(new
        {
            totalMembers,
            checkedInToday,
            absentToday = totalMembers - checkedInToday,
            lateToday = stats?.LateToday ?? 0,
            devicesOnline = stats?.DevicesOnline ?? 0,
            totalRecordsToday = checkedInToday,
            onLeaveToday = 0,
            todayAttendances,
            bridgeStatus = (object?)null
        });
    }

    [HttpGet("get-employees")]
    public async Task<IActionResult> GetEmployees(CancellationToken cancellationToken)
    {
        var employees = await _memberRepo.Query()
            .Where(m => !m.IsDeleted)
            .Select(m => new
            {
                id = m.Id,
                code = m.Code,
                name = m.FullName,
                phone = m.PhoneNumber,
                hasActiveSub = m.Subscriptions.Any(s => s.Status == Shared.Enums.SubscriptionStatus.Active && s.ExpirationDate > DateTime.UtcNow),
                isEnrolled = false
            })
            .ToListAsync(cancellationToken);

        return Json(employees);
    }

    [HttpGet("get-devices")]
    public async Task<IActionResult> GetDevices(CancellationToken cancellationToken)
    {
        var devices = await _deviceRepo.Query()
            .Where(d => d.IsActive)
            .Select(d => new
            {
                id = d.Id,
                name = d.Name,
                ipAddress = d.IPAddress,
                port = d.Port,
                model = d.Model,
                serialNumber = d.SerialNumber,
                status = d.Status.ToString(),
                lastConnectedAt = d.LastConnectedAt,
                firmwareVersion = d.FirmwareVersion
            })
            .ToListAsync(cancellationToken);

        return Json(devices);
    }

    [HttpPost("test-device")]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> TestDevice(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _bridgeClient.TestConnectionAsync(cancellationToken);
            return Json(new
            {
                success = result.Success,
                latencyMs = result.RoundTripLatencyMs,
                errorMessage = result.ErrorMessage ?? (result.Success ? null : "Connection test failed - is the bridge running?")
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Test device connection failed");
            return Json(new
            {
                success = false,
                latencyMs = 0L,
                errorMessage = $"Bridge unreachable: {ex.Message}. Start the HackGym.ZKTeco.Bridge service."
            });
        }
    }

    [HttpPost("sync-from-device")]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> SyncFromDevice(CancellationToken cancellationToken)
    {
        try
        {
            // Trigger the bridge to sync users and device info
            var result = await _bridgeClient.CheckHealthAsync(cancellationToken);
            
            if (!result.IsConnected)
            {
                return Json(new { success = false, error = "Bridge not connected to device" });
            }

            // The bridge's AttendancePollingWorker will handle user sync on next cycle
            // We can also trigger it manually via the bridge's reconciliation endpoint
            
            return Json(new { success = true, message = "Sync triggered - bridge will pull users from device on next poll cycle" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to trigger device sync");
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpPost("import-all-from-device")]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> ImportAllFromDevice(CancellationToken cancellationToken)
    {
        try
        {
            var bridgeUrl = _configuration.GetValue<string>("ZKTecoBridge:GrpcUrl") ?? "http://localhost:50054";
            using var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync($"{bridgeUrl}/zkteco.bridge.ZKTecoBridge/ReconcileUsers",
                new StringContent("{}", System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, error = $"Bridge call failed: {response.StatusCode}" });
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            if (!data.GetProperty("success").GetBoolean())
            {
                return Json(new { success = false, error = "Bridge failed to read device users" });
            }

            var userIds = data.GetProperty("usersChecked").GetInt32();
            return Json(new { success = true, message = $"Found {userIds} users on device. Use /sync-users to import them.", usersFound = userIds });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to import from device");
            return Json(new { success = false, error = ex.Message });
        }
    }

    private async Task<object> GetBridgeStatusAsync(CancellationToken ct)
    {
        try
        {
            var health = await _bridgeClient.CheckHealthAsync(ct);
            return new
            {
                connected = health.IsConnected,
                enrolledUsers = health.EnrolledUserCount,
                freeMemory = health.FreeMemory,
                firmwareVersion = health.FirmwareVersion ?? "",
                uptimeMs = health.UptimeMs,
                error = (string?)null
            };
        }
        catch (Exception ex)
        {
            return new
            {
                connected = false,
                enrolledUsers = 0,
                freeMemory = 0L,
                firmwareVersion = "",
                uptimeMs = 0L,
                error = $"Bridge offline: {ex.Message}"
            };
        }
    }

    [HttpGet("ManualTesting")]
    [AllowAnonymous]
    public async Task<IActionResult> ManualTesting(CancellationToken cancellationToken)
    {
        // Single bridge health call (was 3 — at line 321, 338, and inside
        // GetBridgeStatusAsync — each ~700ms, totaling ~2.1s page load).
        DeviceHealthStatus? deviceHealth = null;
        try
        {
            deviceHealth = await _bridgeClient.CheckHealthAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Bridge health check failed on ManualTesting page");
        }
        var isDeviceOnline = deviceHealth?.IsConnected ?? false;

        var employees = await _memberRepo.Query()
            .Where(m => !m.IsDeleted)
            .Select(m => new
            {
                id = m.Id,
                code = m.Code,
                name = m.FullName,
                phone = m.PhoneNumber
            })
            .Take(20)
            .ToListAsync(cancellationToken);

        var viewModel = new
        {
            PageTitle = _localizer["Manual Testing"],
            IsDeviceOnline = isDeviceOnline,
            DeviceInfo = new
            {
                Ip = _configuration["ZKTeco:DeviceIp"] ?? "192.168.1.201",
                Port = _configuration.GetValue<int>("ZKTeco:DevicePort", 4370)
            },
            Employees = employees,
            DeviceStatus = new
            {
                connected = isDeviceOnline,
                enrolledUsers = deviceHealth?.EnrolledUserCount ?? 0,
                freeMemory = deviceHealth?.FreeMemory ?? 0L,
                firmwareVersion = deviceHealth?.FirmwareVersion ?? "",
                uptimeMs = deviceHealth?.UptimeMs ?? 0L,
                error = deviceHealth == null ? "Bridge offline" : (string?)null
            }
        };

        return View("ManualTesting", viewModel);
    }
}
