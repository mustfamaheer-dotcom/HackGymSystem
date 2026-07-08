using Gym.Application.Dashboard.Queries.GetDetailedDashboard;
using Gym.Application.Dashboard.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Filters;
using Gym.API;
using Microsoft.Extensions.Localization;
using Gym.Domain.Interfaces;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize]
[Route("Dashboard")]
public class DashboardMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepository<Member> _memberRepo;

    public DashboardMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer, IRepository<Member> memberRepo)
    {
        _mediator = mediator;
        _localizer = localizer;
        _memberRepo = memberRepo;
    }

    [RequirePermission("Dashboard.View")]
    [HttpGet]
    public async Task<IActionResult> Index(int? year = null, int? month = null, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Dashboard Analysis"];

        var result = await _mediator.Send(new GetDetailedDashboardQuery(year, month, from, to), cancellationToken);

        if (result.IsFailure)
            return View(new DetailedDashboardDto());

        ViewBag.FilterYear = year;
        ViewBag.FilterMonth = month;
        ViewBag.FilterFrom = from;
        ViewBag.FilterTo = to;

        // Compute available years from data
        var firstYear = await _memberRepo.Query().MinAsync(m => (int?)m.CreatedAt.Year, cancellationToken) ?? DateTime.UtcNow.Year;
        var years = Enumerable.Range(firstYear, DateTime.UtcNow.Year - firstYear + 1).ToList();
        ViewBag.AvailableYears = years;

        return View(result.Data);
    }
}
