using System.Text.Json.Serialization;
using Gym.API.Filters;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Attendances.Commands.CheckOut;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.API.Controllers;

[AllowAnonymous]
[DeviceApiKey]
public class ZKTecoAttendanceController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IDeviceMemberMappingRepository _mappingRepo;
    private readonly IRepository<Device> _deviceRepo;
    private readonly IOptions<ZKTecoSettings> _zktecoConfig;
    private readonly IRepository<Attendance> _attendanceRepo;

    public ZKTecoAttendanceController(
        IMediator mediator,
        IDeviceMemberMappingRepository mappingRepo,
        IRepository<Device> deviceRepo,
        IOptions<ZKTecoSettings> zktecoConfig,
        IRepository<Attendance> attendanceRepo)
    {
        _mediator = mediator;
        _mappingRepo = mappingRepo;
        _deviceRepo = deviceRepo;
        _zktecoConfig = zktecoConfig;
        _attendanceRepo = attendanceRepo;
    }

    [HttpPost("push")]
    public async Task<IActionResult> PushAttendance([FromBody] DeviceAttendancePushRequest request, CancellationToken ct)
    {
        var mapping = await _mappingRepo.GetByEnrollmentIdAsync(request.EnrollmentId, ct);
        if (mapping == null)
            return NotFound(ApiResponse.Fail($"No member mapping found for enrollment ID '{request.EnrollmentId}'"));

        var device = await _deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == _zktecoConfig.Value.DeviceIp, ct);

        if (request.Direction == 0)
        {
            var result = await _mediator.Send(new CheckInCommand(mapping.MemberId, false, device?.Id), ct);
            if (result.IsFailure)
                return BadRequest(ApiResponse<Guid>.Fail(result.Message!));
            return Ok(ApiResponse<Guid>.Ok(result.Data!));
        }
        else
        {
            var existing = await _attendanceRepo.FirstOrDefaultAsync(
                a => a.MemberId == mapping.MemberId && a.CheckIn.Date == request.Timestamp.Date && a.CheckOut == null, ct);
            if (existing == null)
                return NotFound(ApiResponse.Fail("No active check-in found for check-out"));

            var result = await _mediator.Send(new CheckOutCommand(existing.Id, device?.Id), ct);
            if (result.IsFailure)
                return BadRequest(ApiResponse.Fail(result.Message!));
            return Ok(ApiResponse.Ok("Check-out recorded"));
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
