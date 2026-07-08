using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.DailySessions.Commands.CreateDailySession;
using Gym.Application.DailySessions.DTOs;
using Gym.Application.DailySessions.Queries.GetAllDailySessions;
using Gym.API;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("DailySessions")]
public class DailySessionsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DailySessionsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [RequirePermission("Attendance.View")]
    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm = null, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Daily Sessions"];
        ViewBag.SearchTerm = searchTerm;
        ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
        ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");

        var query = new GetAllDailySessionsQuery(searchTerm, dateFrom, dateTo, page, pageSize ?? PaginationRequest.DefaultPageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(new PaginatedResult<DailySessionDto>());
        }

        return View(result.Data);
    }

    [RequirePermission("Attendance.Create")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = _localizer["New Daily Session"];
        return View();
    }

    [RequirePermission("Attendance.Create")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDailySessionCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Daily Session"];
        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }

        TempData["Success"] = string.Format(_localizer["Daily session for '{0}' has been recorded"].Value, command.Name);
        return RedirectToAction(nameof(Index));
    }
}
