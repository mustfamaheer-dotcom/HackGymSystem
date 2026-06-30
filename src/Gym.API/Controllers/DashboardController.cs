using Gym.Application.Dashboard.Queries.GetDashboard;
using Gym.Application.Dashboard.DTOs;
using Gym.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class DashboardController : BaseController
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardQuery(), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<DashboardDto>.Fail(result.Message!));

        return Ok(ApiResponse<DashboardDto>.Ok(result.Data!));
    }
}
