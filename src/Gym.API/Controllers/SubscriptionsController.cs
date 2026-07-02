using Gym.API.Filters;
using Gym.API.Resources;
using Gym.Application.Common.DTOs;
using Gym.Application.Subscriptions.Commands.CreateSubscription;
using Gym.Application.Subscriptions.Commands.FreezeSubscription;
using Gym.Application.Subscriptions.Commands.RecordSubscriptionPayment;
using Gym.Application.Subscriptions.Commands.RenewSubscription;
using Gym.Application.Subscriptions.Commands.UnfreezeSubscription;
using Gym.Application.Subscriptions.DTOs;
using Gym.Application.Subscriptions.Queries.GetAllSubscriptions;
using Gym.Application.Subscriptions.Queries.GetSubscriptionById;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
public class SubscriptionsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public SubscriptionsController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Subscriptions.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PaginatedResult<SubscriptionDto>>.Fail(result.Message!));
        return Ok(ApiResponse<PaginatedResult<SubscriptionDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Subscriptions.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSubscriptionByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<SubscriptionDetailDto>.Fail(result.Message!));
        return Ok(ApiResponse<SubscriptionDetailDto>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Subscriptions.Create")]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));
        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPost("{id}/freeze")]
    [RequirePermission("Subscriptions.Freeze")]
    public async Task<IActionResult> Freeze(Guid id, [FromBody] FreezeSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new FreezeSubscriptionCommand(id, request.FreezeDays, request.Reason), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("{id}/unfreeze")]
    [RequirePermission("Subscriptions.Unfreeze")]
    public async Task<IActionResult> Unfreeze(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UnfreezeSubscriptionCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("{id}/renew")]
    [RequirePermission("Subscriptions.Renew")]
    public async Task<IActionResult> Renew(Guid id, [FromBody] RenewSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var command = new RenewSubscriptionCommand(
            id, request.NewPlanId, request.OfferId, request.AmountPaid,
            request.PaymentMethod, request.StartDate, request.AdminSignature, request.Notes);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));
        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPost("{id}/payments")]
    [RequirePermission("Subscriptions.PaymentHistory")]
    public async Task<IActionResult> RecordPayment(Guid id, [FromBody] RecordSubscriptionPaymentCommand command, CancellationToken cancellationToken)
    {
        if (id != command.SubscriptionId)
            return BadRequest(ApiResponse.Fail(_localizer["Route ID and body ID do not match"]));
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }
}

public class FreezeSubscriptionRequest
{
    public int FreezeDays { get; set; }
    public string? Reason { get; set; }
}

public class RenewSubscriptionRequest
{
    public Guid? NewPlanId { get; set; }
    public Guid? OfferId { get; set; }
    public decimal AmountPaid { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime StartDate { get; set; }
    public string? AdminSignature { get; set; }
    public string? Notes { get; set; }
}
