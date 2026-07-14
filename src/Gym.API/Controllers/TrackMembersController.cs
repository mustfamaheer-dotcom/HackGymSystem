using Gym.API.Filters;
using Gym.API.Hubs;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Attendances.Commands.CheckOut;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Events;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Events;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Caching;
using Gym.Infrastructure.Repositories;
using Gym.Infrastructure.Resilience;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize]
public class TrackMembersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Device> _deviceRepo;
    private readonly IRepository<AttendanceSummary> _summaryRepo;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IHubContext<AttendanceHub> _hubContext;
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly IEventPublisher _eventPublisher;
    private readonly ITrackMembersCache _cache;
    private readonly IAttendanceTrackingRepository _attendanceTracking;
    private readonly IDeviceTrackingRepository _deviceTracking;
    private readonly DeviceConnectionManager _connectionManager;
    private readonly ILogger<TrackMembersController> _logger;
    private readonly IConfiguration _configuration;

    public TrackMembersController(
        IMediator mediator,
        IRepository<Attendance> attendanceRepo,
        IRepository<Member> memberRepo,
        IRepository<Device> deviceRepo,
        IRepository<AttendanceSummary> summaryRepo,
        IDeviceMemberMappingRepository mappingRepo,
        IHubContext<AttendanceHub> hubContext,
        IZKTecoBridgeClient bridgeClient,
        IEventPublisher eventPublisher,
        ITrackMembersCache cache,
        IAttendanceTrackingRepository attendanceTracking,
        IDeviceTrackingRepository deviceTracking,
        DeviceConnectionManager connectionManager,
        ILogger<TrackMembersController> logger,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _attendanceRepo = attendanceRepo;
        _memberRepo = memberRepo;
        _deviceRepo = deviceRepo;
        _summaryRepo = summaryRepo;
        _mappingRepo = mappingRepo;
        _hubContext = hubContext;
        _bridgeClient = bridgeClient;
        _eventPublisher = eventPublisher;
        _cache = cache;
        _attendanceTracking = attendanceTracking;
        _deviceTracking = deviceTracking;
        _connectionManager = connectionManager;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var totalMembers = await _cache.GetTodayCheckInCountAsync(
            async (token) => await _memberRepo.CountAsync(m => !m.IsDeleted, token), ct);

        var checkedInToday = await _attendanceTracking.GetTodayCheckInCountAsync(ct);
        var absentToday = await _memberRepo.CountAsync(m => !m.IsDeleted, ct) - checkedInToday;
        var lateToday = await _attendanceTracking.GetTodayLateCountAsync(ct);
        var devicesOnline = await _deviceTracking.GetOnlineCountAsync(ct);

        return Ok(ApiResponse<object>.Ok(new
        {
            totalMembers,
            checkedInToday,
            absentToday,
            lateToday,
            devicesOnline,
            totalRecordsToday = checkedInToday,
            onLeaveToday = 0,
            lastUpdated = DateTime.UtcNow
        }));
    }

    [HttpGet("live-feed")]
    public async Task<IActionResult> GetLiveFeed(CancellationToken ct)
    {
        var attendances = await _attendanceTracking.GetTodayAttendancesAsync(100, ct);

        var result = attendances.Select(a => new
        {
            id = a.Id,
            memberId = a.MemberId,
            memberName = a.Member?.FullName ?? "",
            memberCode = a.Member?.Code,
            checkIn = a.CheckIn,
            checkOut = a.CheckOut,
            deviceName = a.Device?.Name ?? "MANUAL",
            isManual = a.IsManual
        }).ToList();

        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees(CancellationToken ct)
    {
        var employees = await _cache.GetActiveMembersAsync(
            async (token) => await _memberRepo.Query()
                .Include(m => m.Subscriptions.Where(s => s.Status == SubscriptionStatus.Active))
                .Where(m => !m.IsDeleted)
                .ToListAsync(token), ct);

        var result = employees.Select(m => new
        {
            id = m.Id,
            code = m.Code,
            name = m.FullName,
            phone = m.PhoneNumber,
            hasActiveSub = m.Subscriptions.Any(s => s.Status == SubscriptionStatus.Active && s.ExpirationDate > DateTime.UtcNow)
        }).ToList();

        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> ManualCheckIn([FromBody] TrackMemberCheckInRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckInCommand(request.MemberId, true, null, DateTime.UtcNow), ct);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        var member = await _cache.GetMemberByIdAsync(
            request.MemberId,
            async (token) => await _memberRepo.Query()
                .Include(m => m.Package)
                .FirstOrDefaultAsync(m => m.Id == request.MemberId, token), ct);

        await _hubContext.Clients.All.SendAsync("AttendancePushed", new
        {
            memberId = request.MemberId,
            memberName = member?.FullName ?? "",
            imagePath = member?.ImagePath ?? "",
            packageName = member?.Package?.Name ?? "",
            phoneNumber = member?.PhoneNumber ?? "",
            timestamp = DateTime.UtcNow,
            type = "check-in",
            attendanceId = result.Data!
        }, ct);

        _cache.InvalidateAttendanceCache();

        return Ok(ApiResponse<object>.Ok(new { attendanceId = result.Data!, type = "check-in" }));
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> ManualCheckOut([FromBody] TrackMemberCheckOutRequest request, CancellationToken ct)
    {
        var existing = await _attendanceRepo.FirstOrDefaultAsync(
            a => a.MemberId == request.MemberId && a.CheckIn.Date == DateTime.UtcNow.Date && a.CheckOut == null, ct);

        if (existing == null)
            return BadRequest(ApiResponse.Fail("No active check-in found for this member today"));

        var result = await _mediator.Send(new CheckOutCommand(existing.Id, null, DateTime.UtcNow), ct);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        await _hubContext.Clients.All.SendAsync("AttendancePushed", new
        {
            memberId = request.MemberId,
            timestamp = DateTime.UtcNow,
            type = "check-out",
            attendanceId = existing.Id
        }, ct);

        _cache.InvalidateAttendanceCache();

        return Ok(ApiResponse<object>.Ok(new { attendanceId = existing.Id, type = "check-out" }));
    }

    [HttpPost("simulate")]
    public async Task<IActionResult> SimulateAttendance([FromBody] SimulateRequest request, CancellationToken ct)
    {
        var mapping = await _mappingRepo.GetByEnrollmentIdAsync(request.EnrollmentId, ct);
        if (mapping == null)
            return BadRequest(ApiResponse.Fail($"No member mapping found for enrollment ID '{request.EnrollmentId}'"));

        var timestamp = request.Timestamp ?? DateTime.UtcNow;

        if (request.Direction == 0)
        {
            var result = await _mediator.Send(new CheckInCommand(mapping.MemberId, false, null, timestamp), ct);
            if (result.IsFailure)
                return BadRequest(ApiResponse.Fail(result.Message!));

            var member = await _cache.GetMemberByIdAsync(
                mapping.MemberId,
                async (token) => await _memberRepo.Query()
                    .Include(m => m.Package)
                    .FirstOrDefaultAsync(m => m.Id == mapping.MemberId, token), ct);

            await _hubContext.Clients.All.SendAsync("AttendancePushed", new
            {
                memberId = mapping.MemberId,
                memberName = member?.FullName ?? "",
                imagePath = member?.ImagePath ?? "",
                packageName = member?.Package?.Name ?? "",
                phoneNumber = member?.PhoneNumber ?? "",
                timestamp,
                type = "check-in",
                attendanceId = result.Data!
            }, ct);

            _cache.InvalidateAttendanceCache();

            return Ok(ApiResponse<object>.Ok(new { attendanceId = result.Data!, type = "check-in" }));
        }
        else
        {
            var existing = await _attendanceRepo.FirstOrDefaultAsync(
                a => a.MemberId == mapping.MemberId && a.CheckIn.Date == timestamp.Date && a.CheckOut == null, ct);

            if (existing == null)
                return BadRequest(ApiResponse.Fail("No active check-in found for check-out"));

            var result = await _mediator.Send(new CheckOutCommand(existing.Id, null, timestamp), ct);
            if (result.IsFailure)
                return BadRequest(ApiResponse.Fail(result.Message!));

            await _hubContext.Clients.All.SendAsync("AttendancePushed", new
            {
                memberId = mapping.MemberId,
                timestamp,
                type = "check-out",
                attendanceId = existing.Id
            }, ct);

            _cache.InvalidateAttendanceCache();

            return Ok(ApiResponse<object>.Ok(new { attendanceId = existing.Id, type = "check-out" }));
        }
    }

    [HttpGet("device-health")]
    public async Task<IActionResult> GetDeviceHealth(CancellationToken ct)
    {
        var circuitState = _connectionManager.CircuitState;

        DeviceHealthStatus? bridgeHealth = null;
        try
        {
            bridgeHealth = await _bridgeClient.CheckHealthAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get bridge health");
        }

        // Use live bridge health data — the DeviceConnectionManager caches
        // the result of its last ConnectAsync() call, which can be stale
        // (stays false if the bridge started after the API).
        var isConnected = bridgeHealth?.IsConnected ?? _connectionManager.IsConnected;

        return Ok(ApiResponse<object>.Ok(new
        {
            circuitBreakerState = circuitState.ToString(),
            isConnected,
            bridge = bridgeHealth != null ? new
            {
                bridgeHealth.IsConnected,
                bridgeHealth.EnrolledUserCount,
                bridgeHealth.FreeMemory,
                bridgeHealth.FirmwareVersion,
                bridgeHealth.UptimeMs
            } : null,
            timestamp = DateTime.UtcNow
        }));
    }

    [HttpPost("test-connection")]
    public async Task<IActionResult> TestConnection(CancellationToken ct)
    {
        var result = await _connectionManager.ConnectAsync(ct);
        return Ok(ApiResponse<object>.Ok(new
        {
            success = result,
            circuitState = _connectionManager.CircuitState.ToString(),
            timestamp = DateTime.UtcNow
        }));
    }

    // ==================== MANUAL TESTING APIS ====================
    
    /// <summary>
    /// Manual check-in by member ID (for testing when device is offline)
    /// </summary>
    [HttpPost("manual-check-in")]
    public async Task<IActionResult> ManualCheckInTest([FromBody] TrackMemberCheckInRequest request, CancellationToken ct)
    {
        // Validate member exists and is active
        var member = await _memberRepo.FirstOrDefaultAsync(m => m.Id == request.MemberId && !m.IsDeleted, ct);
        if (member == null)
            return BadRequest(ApiResponse.Fail("Member not found"));

        // Check if member already checked in today
        var existing = await _attendanceRepo.FirstOrDefaultAsync(
            a => a.MemberId == request.MemberId && a.CheckIn.Date == DateTime.UtcNow.Date && a.CheckOut == null, ct);
        
        if (existing != null)
            return BadRequest(ApiResponse.Fail("Member already checked in today"));

        // Create manual attendance record
        var result = await _mediator.Send(new CheckInCommand(request.MemberId, true, null, DateTime.UtcNow), ct);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        // Fetch member with package for SignalR payload
        var memberWithPackage = await _cache.GetMemberByIdAsync(
            request.MemberId,
            async (token) => await _memberRepo.Query()
                .Include(m => m.Package)
                .FirstOrDefaultAsync(m => m.Id == request.MemberId, token), ct);

        // Push real-time update via SignalR
        await _hubContext.Clients.All.SendAsync("AttendancePushed", new
        {
            memberId = request.MemberId,
            memberName = member.FullName,
            imagePath = member.ImagePath ?? "",
            packageName = memberWithPackage?.Package?.Name ?? "",
            phoneNumber = member.PhoneNumber ?? "",
            timestamp = DateTime.UtcNow,
            type = "check-in",
            attendanceId = result.Data!,
            deviceName = "MANUAL"
        }, ct);

        _cache.InvalidateAttendanceCache();

        _logger.LogInformation("Manual check-in recorded for member {MemberId} ({MemberName})", request.MemberId, member.FullName);

        return Ok(ApiResponse<object>.Ok(new 
        { 
            attendanceId = result.Data!, 
            type = "check-in",
            memberName = member.FullName,
            message = "Manual check-in recorded successfully",
            timestamp = DateTime.UtcNow
        }));
    }

    /// <summary>
    /// Manual check-out by member ID (for testing when device is offline)
    /// </summary>
    [HttpPost("manual-check-out")]
    public async Task<IActionResult> ManualCheckOutTest([FromBody] TrackMemberCheckOutRequest request, CancellationToken ct)
    {
        // Validate member exists
        var member = await _memberRepo.FirstOrDefaultAsync(m => m.Id == request.MemberId && !m.IsDeleted, ct);
        if (member == null)
            return BadRequest(ApiResponse.Fail("Member not found"));

        // Find active check-in for today
        var existing = await _attendanceRepo.FirstOrDefaultAsync(
            a => a.MemberId == request.MemberId && a.CheckIn.Date == DateTime.UtcNow.Date && a.CheckOut == null, ct);

        if (existing == null)
            return BadRequest(ApiResponse.Fail("No active check-in found for this member today"));

        // Process check-out
        var result = await _mediator.Send(new CheckOutCommand(existing.Id, null, DateTime.UtcNow), ct);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        // Push real-time update via SignalR
        await _hubContext.Clients.All.SendAsync("AttendancePushed", new
        {
            memberId = request.MemberId,
            memberName = member.FullName,
            timestamp = DateTime.UtcNow,
            type = "check-out",
            attendanceId = existing.Id,
            deviceName = "MANUAL"
        }, ct);

        _cache.InvalidateAttendanceCache();

        _logger.LogInformation("Manual check-out recorded for member {MemberId} ({MemberName})", request.MemberId, member.FullName);

        return Ok(ApiResponse<object>.Ok(new 
        { 
            attendanceId = existing.Id, 
            type = "check-out",
            memberName = member.FullName,
            message = "Manual check-out recorded successfully",
            timestamp = DateTime.UtcNow
        }));
    }

    /// <summary>
    /// Get system status for manual testing dashboard
    /// </summary>
    [HttpGet("manual-status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetManualStatus(CancellationToken ct)
    {
        var isDeviceOnline = _connectionManager.IsConnected;
        var bridgeHealth = (DeviceHealthStatus?)null;
        
        try
        {
            bridgeHealth = await _bridgeClient.CheckHealthAsync(ct);
        }
        catch { /* ignore */ }

        var totalMembers = await _memberRepo.CountAsync(m => !m.IsDeleted, ct);
        var checkedInToday = await _attendanceTracking.GetTodayCheckInCountAsync(ct);
        var attendanceToday = await _attendanceTracking.GetTodayAttendancesAsync(50, ct);

        return Ok(ApiResponse<object>.Ok(new
        {
            timestamp = DateTime.UtcNow,
            deviceOnline = isDeviceOnline,
            bridgeConnected = bridgeHealth?.IsConnected ?? false,
            bridgeStatus = bridgeHealth,
            deviceInfo = new
            {
                ip = _configuration["ZKTeco:DeviceIp"] ?? "192.168.1.201",
                port = int.Parse(_configuration["ZKTeco:DevicePort"] ?? "4370"),
                isConnected = _connectionManager.IsConnected
            },
            stats = new
            {
                totalMembers,
                checkedInToday = await _attendanceTracking.GetTodayCheckInCountAsync(ct),
                absentToday = totalMembers - (await _attendanceTracking.GetTodayCheckInCountAsync(ct)),
                lateToday = await _attendanceTracking.GetTodayLateCountAsync(ct),
                devicesOnline = await _deviceTracking.GetOnlineCountAsync(ct)
            },
            recentAttendance = attendanceToday.Select(a => new
            {
                id = a.Id,
                memberId = a.MemberId,
                memberName = a.Member?.FullName ?? "",
                memberCode = a.Member?.Code,
                checkIn = a.CheckIn,
                checkOut = a.CheckOut,
                deviceName = a.Device?.Name ?? "MANUAL",
                isManual = a.IsManual
            }).ToList(),
            mode = _connectionManager.IsConnected ? "device" : "manual",
            message = _connectionManager.IsConnected 
                ? "Device online - automatic attendance active" 
                : "Device offline - manual mode available"
        }));
    }

}

public class TrackMemberCheckInRequest
{
    public Guid MemberId { get; set; }
}

public class TrackMemberCheckOutRequest
{
    public Guid MemberId { get; set; }
}

public class SimulateRequest
{
    public string EnrollmentId { get; set; } = string.Empty;
    public int Direction { get; set; }
    public DateTime? Timestamp { get; set; }
}
