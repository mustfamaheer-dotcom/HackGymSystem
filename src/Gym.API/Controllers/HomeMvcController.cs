using Gym.Application.Dashboard.Queries.GetDashboard;
using Gym.Application.Dashboard.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class HomeMvcController : Controller
{
    private readonly IMediator _mediator;

    public HomeMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("Home")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Home";

        var result = await _mediator.Send(new GetDashboardQuery(), cancellationToken);

        if (result.IsFailure)
            return View(new DashboardDto());

        return View(result.Data);
    }
}
