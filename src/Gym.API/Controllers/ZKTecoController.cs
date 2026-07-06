using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.ZKTeco.Commands;
using Gym.Application.ZKTeco.DTOs;
using Gym.Application.ZKTeco.Queries;
using Gym.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gym.API.Controllers;

[Authorize]
public class ZKTecoController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHubContext<Hubs.AttendanceHub> _hubContext;

    public ZKTecoController(IMediator mediator, IHubContext<Hubs.AttendanceHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpGet("status")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> GetStatus(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDeviceStatusQuery(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DeviceStatusDto>.Fail(result.Message!));

        return Ok(ApiResponse<DeviceStatusDto>.Ok(result.Data!));
    }

    [HttpGet("sync-logs")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> GetSyncLogs([FromQuery] GetSyncLogsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PaginatedResult<SyncLogDto>>.Fail(result.Message!));

        return Ok(ApiResponse<PaginatedResult<SyncLogDto>>.Ok(result.Data!));
    }

    [HttpPost("reconcile")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Reconcile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReconcileUsersCommand(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReconcileResult>.Fail(result.Message!));

        return Ok(ApiResponse<ReconcileResult>.Ok(result.Data!));
    }

    [HttpPost("enroll")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> Enroll([FromBody] EnrollBiometricCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<string>.Fail(result.Message!));

        return Ok(ApiResponse<string>.Ok(result.Data!));
    }

    [HttpPost("testconnection")]
    [RequirePermission("Devices.Manage")]
    public async Task<IActionResult> TestConnection(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TestConnectionCommand(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TestConnectionResult>.Fail(result.Message!));

        return Ok(ApiResponse<TestConnectionResult>.Ok(result.Data!));
    }
}
