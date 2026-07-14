using System.Text.Json;
using System.Text.Json.Serialization;
using Gym.API.Filters;
using Gym.API.Hubs;
using Gym.API.Services;
using Gym.API.WebSockets;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Attendances.Commands.CheckOut;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Gym.API.Controllers;

[TypeFilter(typeof(DeviceApiKeyAttribute))]
[DisableRateLimiting]
[IgnoreAntiforgeryToken]
[Route("api/zkteco-attendance")]
public class ZKTecoAttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IRepository<Device> _deviceRepo;
    private readonly IOptions<ZKTecoSettings> _zktecoConfig;
    private readonly IRepository<Attendance> _attendanceRepo;
    private readonly IRepository<Subscription> _subscriptionRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IHubContext<AttendanceHub> _hubContext;
    private readonly IZKTecoBridgeClient _bridgeClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ZKTecoAttendanceController> _logger;
    private readonly AttendancePushService _attendancePush;
    private readonly IConfiguration _configuration;

    public ZKTecoAttendanceController(
        IMediator mediator,
        IDeviceMemberMappingRepository mappingRepo,
        IRepository<Device> deviceRepo,
        IOptions<ZKTecoSettings> zktecoConfig,
        IRepository<Attendance> attendanceRepo,
        IRepository<Subscription> subscriptionRepo,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IHubContext<AttendanceHub> hubContext,
        IZKTecoBridgeClient bridgeClient,
        IHttpClientFactory httpClientFactory,
        ILogger<ZKTecoAttendanceController> logger,
        AttendancePushService attendancePush,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _mappingRepo = mappingRepo;
        _deviceRepo = deviceRepo;
        _zktecoConfig = zktecoConfig;
        _attendanceRepo = attendanceRepo;
        _subscriptionRepo = subscriptionRepo;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _hubContext = hubContext;
        _bridgeClient = bridgeClient;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _attendancePush = attendancePush;
        _configuration = configuration;
    }

    [HttpPost("push")]
    public async Task<IActionResult> PushAttendance([FromBody] DeviceAttendancePushRequest request, CancellationToken ct)
    {
        var payload = new AttendancePushPayload
        {
            EnrollmentId = request.EnrollmentId,
            Timestamp = request.Timestamp,
            Direction = request.Direction,
            VerifyMethod = request.VerifyMethod
        };

        var result = await _attendancePush.ProcessAttendanceAsync(payload, ct);

        if (result.Success)
            return result.Type == "check-in"
                ? Ok(ApiResponse<Guid>.Ok(result.AttendanceId!.Value))
                : Ok(ApiResponse.Ok("Check-out recorded"));

        if (result.Error?.Contains("mapping") == true)
            return NotFound(ApiResponse.Fail(result.Error));
        return BadRequest(ApiResponse.Fail(result.Error!));
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health(CancellationToken ct)
    {
        var bridgeOk = await _bridgeClient.CheckHealthAsync(ct);
        return Ok(new
        {
            service = "ZKTeco Attendance",
            bridgeConnected = bridgeOk,
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("push-batch")]
    public async Task<IActionResult> PushBatch([FromBody] List<DeviceAttendancePushRequest> requests, CancellationToken ct)
    {
        var results = new List<object>();
        var successCount = 0;
        var failCount = 0;

        foreach (var request in requests)
        {
            try
            {
                var mapping = await _mappingRepo.GetByEnrollmentIdAsync(request.EnrollmentId, ct);
                if (mapping == null)
                {
                    failCount++;
                    results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = "No member mapping found" });
                    continue;
                }

                var hasActiveSub = await _subscriptionRepo.AnyAsync(
                    s => s.MemberId == mapping.MemberId
                      && s.Status == SubscriptionStatus.Active
                      && s.ExpirationDate > DateTime.UtcNow, ct);

                if (!hasActiveSub && _zktecoConfig.Value.RequireActiveSubscriptionForAttendance)
                {
                    failCount++;
                    results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = "No active subscription" });
                    continue;
                }

                var device = await _deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == _zktecoConfig.Value.DeviceIp, ct);

                if (request.Direction == 0)
                {
                    var result = await _mediator.Send(new CheckInCommand(mapping.MemberId, false, device?.Id, request.Timestamp), ct);
                    if (result.IsFailure)
                    {
                        failCount++;
                        results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = result.Message });
                    }
                    else
                    {
                        successCount++;
                        results.Add(new { enrollmentId = request.EnrollmentId, success = true, type = "check-in" });
                    }
                }
                else
                {
                    var existing = await _attendanceRepo.FirstOrDefaultAsync(
                        a => a.MemberId == mapping.MemberId && a.CheckIn.Date == request.Timestamp.Date && a.CheckOut == null, ct);
                    if (existing == null)
                    {
                        failCount++;
                        results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = "No active check-in found" });
                        continue;
                    }

                    var result = await _mediator.Send(new CheckOutCommand(existing.Id, device?.Id, request.Timestamp), ct);
                    if (result.IsFailure)
                    {
                        failCount++;
                        results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = result.Message });
                    }
                    else
                    {
                        successCount++;
                        results.Add(new { enrollmentId = request.EnrollmentId, success = true, type = "check-out" });
                    }
                }
            }
            catch (Exception ex)
            {
                failCount++;
                results.Add(new { enrollmentId = request.EnrollmentId, success = false, error = ex.Message });
            }
        }

        return Ok(new
        {
            success = true,
            totalProcessed = requests.Count,
            successCount,
            failCount,
            details = results
        });
    }

    [HttpPost("device-info")]
    public async Task<IActionResult> PushDeviceInfo([FromBody] DeviceInfoPushRequest request, CancellationToken ct)
    {
        var device = await _deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == request.IpAddress, ct);

        if (device == null)
        {
            device = new Device(request.Model, request.IpAddress, request.Port, request.Model, request.SerialNumber);
            device.UpdateFirmware(request.FirmwareVersion);
            device.MarkOnline();
            await _deviceRepo.AddAsync(device, ct);
        }
        else
        {
            device.UpdateFirmware(request.FirmwareVersion);
            device.MarkOnline();
            if (device.SerialNumber != request.SerialNumber)
            {
                device.SerialNumber = request.SerialNumber;
            }
            _deviceRepo.Update(device);
        }

        return Ok(ApiResponse.Ok("Device info updated"));
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromBody] DeviceInfoPushRequest request, CancellationToken ct)
    {
        var device = await _deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == request.IpAddress, ct);

        if (device == null)
        {
            device = new Device(request.Model, request.IpAddress, request.Port, request.Model, request.SerialNumber);
            device.UpdateFirmware(request.FirmwareVersion);
            device.MarkOnline();
            await _deviceRepo.AddAsync(device, ct);
        }
        else
        {
            device.MarkOnline();
            _deviceRepo.Update(device);
        }

        return Ok(ApiResponse.Ok("Heartbeat received"));
    }

    [HttpPost("device-offline")]
    public async Task<IActionResult> DeviceOffline([FromBody] DeviceInfoPushRequest request, CancellationToken ct)
    {
        var device = await _deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == request.IpAddress, ct);

        if (device != null)
        {
            device.MarkOffline();
            _deviceRepo.Update(device);
        }

        return Ok(ApiResponse.Ok("Device marked offline"));
    }

    [HttpPost("sync-users")]
    public async Task<IActionResult> SyncUsers([FromBody] List<DeviceUserSyncRequest> request, CancellationToken ct)
    {
        var results = new List<object>();
        var syncedCount = 0;
        var skippedCount = 0;
        var createdCount = 0;

        var lastCode = await _memberRepo.Query().IgnoreQueryFilters().MaxAsync(m => (int?)m.Code, ct) ?? 0;

        foreach (var user in request)
        {
            var existingMapping = await _mappingRepo.GetByEnrollmentIdAsync(user.EnrollmentId, ct);

            if (existingMapping != null)
            {
                skippedCount++;
                results.Add(new { enrollmentId = user.EnrollmentId, action = "skipped", reason = "Already mapped to member" });
                continue;
            }

            // Check if member already exists with this name (fuzzy match)
            var existingMember = await _memberRepo.FirstOrDefaultAsync(m => m.FullName == user.Name && !m.IsDeleted, ct);
            
            Guid memberId;
            if (existingMember != null)
            {
                memberId = existingMember.Id;
            }
            else
            {
                // Create new member from device user
                var receiptNumber = $"AUTO-{user.EnrollmentId}";
                var phoneNumber = $"D{user.EnrollmentId}";
                var registrationDate = DateTime.UtcNow;
                lastCode++;

                var newMember = new Member(receiptNumber, user.Name, phoneNumber, registrationDate) { Code = lastCode, NationalId = phoneNumber };
                await _memberRepo.AddAsync(newMember, ct);
                memberId = newMember.Id;
                createdCount++;
            }

            // Create device member mapping
            var mapping = new DeviceMemberMapping(memberId, user.EnrollmentId, BiometricType.Face, null);
            await _mappingRepo.SaveMappingAsync(mapping, ct);

            syncedCount++;
            results.Add(new { enrollmentId = user.EnrollmentId, action = "synced", memberId = memberId, memberName = user.Name });
        }

        return Ok(new
        {
            success = true,
            totalProcessed = request.Count,
            createdCount,
            syncedCount,
            skippedCount,
            details = results
        });
    }

    [HttpPost("import-all-from-device")]
    public async Task<IActionResult> ImportAllFromDevice(CancellationToken ct)
    {
        try
        {
            // Call bridge to get all user IDs
            var bridgeUrl = _configuration.GetValue<string>("ZKTecoBridge:GrpcUrl") ?? "http://localhost:50054";
            var response = await _httpClientFactory.CreateClient().PostAsync($"{bridgeUrl}/zkteco.bridge.ZKTecoBridge/ReconcileUsers",
                new StringContent("{}", System.Text.Encoding.UTF8, "application/json"), ct);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(ApiResponse.Fail($"Bridge call failed: {response.StatusCode}"));
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var data = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            if (!data.GetProperty("success").GetBoolean())
            {
                return BadRequest(ApiResponse.Fail("Bridge failed to read device users"));
            }

            var userIds = data.GetProperty("usersChecked").GetInt32();

            return Ok(new { success = true, message = $"Found {userIds} users on device. Use /sync-users to import them.", usersFound = userIds });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to import from device");
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }
}

public class DeviceAttendancePushRequest
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

public class DeviceInfoPushRequest
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

public class DeviceUserSyncRequest
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
