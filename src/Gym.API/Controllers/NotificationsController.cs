using Gym.Application.Notifications.Commands.CreateNotification;
using Gym.Application.Notifications.Commands.MarkNotificationFailed;
using Gym.Application.Notifications.Commands.MarkNotificationSent;
using Gym.Application.Notifications.DTOs;
using Gym.Application.Notifications.Queries.GetAllNotifications;
using Gym.Application.Notifications.Queries.GetMemberNotifications;
using Gym.Application.Notifications.Queries.GetNotificationById;
using Gym.Application.Notifications.Queries.GetPendingNotifications;
using Gym.Application.Common.DTOs;
using Gym.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class NotificationsController : BaseController
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotifications([FromQuery] GetAllNotificationsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PaginatedResult<NotificationDto>>.Fail(result.Message!));

        return Ok(ApiResponse<PaginatedResult<NotificationDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNotificationByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<NotificationDto>.Fail(result.Message!));

        return Ok(ApiResponse<NotificationDto>.Ok(result.Data!));
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingNotifications(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPendingNotificationsQuery(), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<List<NotificationDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<NotificationDto>>.Ok(result.Data!));
    }

    [HttpGet("by-member/{memberId}")]
    public async Task<IActionResult> GetMemberNotifications(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberNotificationsQuery(memberId), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<List<NotificationDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<NotificationDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPost("{id}/mark-sent")]
    public async Task<IActionResult> MarkNotificationSent(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MarkNotificationSentCommand(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok());
    }

    [HttpPost("{id}/mark-failed")]
    public async Task<IActionResult> MarkNotificationFailed(Guid id, [FromBody] MarkNotificationFailedBody body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MarkNotificationFailedCommand(id, body.Error), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok());
    }
}

public record MarkNotificationFailedBody(string Error);
