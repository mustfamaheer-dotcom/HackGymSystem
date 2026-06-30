using Gym.Application.Dashboard.Queries.GetDetailedDashboard;
using Gym.Application.Dashboard.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Dashboard")]
public class DashboardMvcController : Controller
{
    private readonly IMediator _mediator;

    public DashboardMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Dashboard Analysis";

        var result = await _mediator.Send(new GetDetailedDashboardQuery(), cancellationToken);

        if (result.IsFailure)
            return View(new DetailedDashboardDto());

        return View(result.Data);
    }
}
