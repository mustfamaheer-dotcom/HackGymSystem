using Gym.Application.Common.DTOs;
using Gym.Application.MembershipPlans.Commands.CreatePlan;
using Gym.Application.MembershipPlans.Commands.DeletePlan;
using Gym.Application.MembershipPlans.Commands.TogglePlanStatus;
using Gym.Application.MembershipPlans.Commands.UpdatePlan;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.MembershipPlans.Queries.GetActivePlans;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.Application.MembershipPlans.Queries.GetPlanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
public class PlansController : BaseController
{
    private readonly IMediator _mediator;

    public PlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlans([FromQuery] GetAllPlansQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve plans"));

        return Ok(ApiResponse<PaginatedResult<PlanDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlanById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPlanByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Plan not found"));

        return Ok(ApiResponse<PlanDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActivePlans(CancellationToken cancellationToken)
    {
        var query = new GetActivePlansQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve active plans"));

        return Ok(ApiResponse<List<PlanDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create plan"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.Fail("Route ID and body ID do not match"));

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to update plan"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlan(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePlanCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to delete plan"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> TogglePlanStatus(Guid id, [FromBody] TogglePlanStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new TogglePlanStatusCommand(id, request.IsActive);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to toggle plan status"));

        return Ok(ApiResponse.Ok(result.Message));
    }
}

public class TogglePlanStatusRequest
{
    public bool IsActive { get; set; }
}
