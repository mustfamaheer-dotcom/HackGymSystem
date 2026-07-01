using Gym.Application.Dashboard.Queries.GetDashboard;
using Gym.Application.Dashboard.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class HomeMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public HomeMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [Route("Home")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Home"];

        var result = await _mediator.Send(new GetDashboardQuery(), cancellationToken);

        if (result.IsFailure)
            return View(new DashboardDto());

        return View(result.Data);
    }
}
