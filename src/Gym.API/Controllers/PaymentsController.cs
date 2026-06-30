using Gym.Application.Common.DTOs;
using Gym.Application.Payments.Commands.CreatePayment;
using Gym.Application.Payments.DTOs;
using Gym.Application.Payments.Commands.DeletePayment;
using Gym.Application.Payments.Queries.GetAllPayments;
using Gym.Application.Payments.Queries.GetMemberPayments;
using Gym.Application.Payments.Queries.GetPaymentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class PaymentsController : BaseController
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPaymentsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve payments"));

        return Ok(ApiResponse<PaginatedResult<PaymentDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPaymentByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Payment not found"));

        return Ok(ApiResponse<PaymentDto>.Ok(result.Data!));
    }

    [HttpGet("by-member/{memberId}")]
    public async Task<IActionResult> GetMemberPayments(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberPaymentsQuery(memberId), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve member payments"));

        return Ok(ApiResponse<List<PaymentDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create payment"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeletePaymentCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to delete payment"));

        return Ok(ApiResponse.Ok());
    }
}
