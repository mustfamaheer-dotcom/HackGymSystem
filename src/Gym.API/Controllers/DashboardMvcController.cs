using Gym.Application.Dashboard.Queries.GetDetailedDashboard;
using Gym.Application.Dashboard.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Dashboard")]
public class DashboardMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DashboardMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Dashboard Analysis"];

        var result = await _mediator.Send(new GetDetailedDashboardQuery(), cancellationToken);

        if (result.IsFailure)
            return View(new DetailedDashboardDto());

        return View(result.Data);
    }
}
