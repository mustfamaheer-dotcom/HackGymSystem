using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.DailySessions.Commands.CreateDailySession;
using Gym.Application.DailySessions.DTOs;
using Gym.Application.DailySessions.Queries.GetAllDailySessions;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.API;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    [RequirePermission("DailySessions.View")]
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

    [RequirePermission("DailySessions.Create")]
    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Daily Session"];
        await LoadPlansAndPaymentMethodsAsync(cancellationToken);
        return View(new CreateDailySessionCommand(null!, null!, default, null, 0, 0, default));
    }

    [RequirePermission("DailySessions.Create")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDailySessionCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Daily Session"];
        if (!ModelState.IsValid)
        {
            await LoadPlansAndPaymentMethodsAsync(cancellationToken);
            return View(command);
        }

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            await LoadPlansAndPaymentMethodsAsync(cancellationToken);
            return View(command);
        }

        TempData["Success"] = string.Format(_localizer["Daily session for '{0}' has been recorded"].Value, command.Name);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadPlansAndPaymentMethodsAsync(CancellationToken cancellationToken)
    {
        var plansResult = await _mediator.Send(new GetAllPlansQuery(), cancellationToken);
        if (plansResult.IsSuccess)
            ViewBag.Plans = new SelectList(plansResult.Data.Items, "Id", "Name");

        var methods = Enum.GetValues<PaymentMethod>()
            .Select(m => new SelectListItem(_localizer[m.ToString()].Value, m.ToString()));
        ViewBag.PaymentMethods = new SelectList(methods, "Value", "Text");
    }

    [RequirePermission("DailySessions.Create")]
    [HttpGet("get-plan-price")]
    public async Task<IActionResult> GetPlanPrice(Guid planId, CancellationToken cancellationToken)
    {
        var repo = HttpContext.RequestServices.GetRequiredService<IRepository<MembershipPlan>>();
        var plan = await repo.GetByIdAsync(planId, cancellationToken);
        if (plan is null)
            return Json(new { price = (decimal?)null });
        return Json(new { price = plan.Price });
    }
}
