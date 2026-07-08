using Gym.API.Filters;
using Gym.API.Hubs;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Attendances.Commands.CheckOut;
using Gym.Application.Attendances.Commands.CreateManualAttendance;
using Gym.Application.Attendances.DTOs;
using Gym.Application.Attendances.Queries.GetAllAttendances;
using Gym.Application.Attendances.Queries.GetAttendanceById;
using Gym.Application.Attendances.Queries.GetMemberAttendances;
using Gym.Application.Attendances.Queries.GetTodayAttendances;
using Gym.Application.Common.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize]
public class AttendancesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHubContext<AttendanceHub> _hubContext;
    private readonly IRepository<Member> _memberRepo;

    public AttendancesController(IMediator mediator, IHubContext<AttendanceHub> hubContext, IRepository<Member> memberRepo)
    {
        _mediator = mediator;
        _hubContext = hubContext;
        _memberRepo = memberRepo;
    }

    [HttpGet]
    [RequirePermission("Attendance.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAttendancesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PaginatedResult<AttendanceDto>>.Fail(result.Message!));

        return Ok(ApiResponse<PaginatedResult<AttendanceDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Attendance.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAttendanceByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<AttendanceDto>.Fail(result.Message!));

        return Ok(ApiResponse<AttendanceDto>.Ok(result.Data!));
    }

    [HttpGet("by-member/{memberId}")]
    [RequirePermission("Attendance.View")]
    public async Task<IActionResult> GetByMember(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberAttendancesQuery(memberId), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<List<AttendanceDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<AttendanceDto>>.Ok(result.Data!));
    }

    [HttpGet("today")]
    [RequirePermission("Attendance.View")]
    public async Task<IActionResult> GetToday(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTodayAttendancesQuery(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<List<AttendanceDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<AttendanceDto>>.Ok(result.Data!));
    }

    [HttpPost("check-in")]
    [RequirePermission("Attendance.Create")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));

        var member = await _memberRepo.Query()
            .Include(m => m.Package)
            .FirstOrDefaultAsync(m => m.Id == command.MemberId, cancellationToken);

        await _hubContext.Clients.All.SendAsync("AttendancePushed", new
        {
            memberId = command.MemberId,
            memberName = member?.FullName ?? "",
            imagePath = member?.ImagePath ?? "",
            packageName = member?.Package?.Name ?? "",
            phoneNumber = member?.PhoneNumber ?? "",
            timestamp = DateTime.UtcNow,
            type = "check-in",
            attendanceId = result.Data!
        }, cancellationToken);

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPost("check-out")]
    [RequirePermission("Attendance.Create")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("manual")]
    [RequirePermission("Attendance.Manage")]
    public async Task<IActionResult> CreateManual([FromBody] CreateManualAttendanceCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }
}
