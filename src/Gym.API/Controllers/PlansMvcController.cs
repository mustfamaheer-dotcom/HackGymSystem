using Gym.API.Filters;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.MembershipPlans.Commands.CreatePlan;
using Gym.Application.MembershipPlans.Commands.UpdatePlan;
using Gym.Application.MembershipPlans.Commands.DeletePlan;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.Application.MembershipPlans.Queries.GetPlanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Plans")]
public class PlansMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public PlansMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Plans.View")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Plans"];
        var query = new GetAllPlansQuery { Page = page, PageSize = pageSize, SearchTerm = searchTerm };
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }
        ViewBag.SearchTerm = searchTerm;
        return View(result.Data);
    }

    [HttpGet("create")]
    [RequirePermission("Plans.Create")]
    public IActionResult Create()
    {
        ViewData["Title"] = _localizer["New Plan"];
        return View();
    }

    [HttpPost("create")]
    [RequirePermission("Plans.Create")]
    public async Task<IActionResult> Create(CreatePlanCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Plan"];
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Plan created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    [RequirePermission("Plans.Edit")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Plan"];
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var command = new UpdatePlanCommand(dto.Id, dto.Name, dto.Price, dto.DurationDays, dto.MaxVisits, dto.FreezeDays, dto.Description, dto.IsActive);
        return View(command);
    }

    [HttpPost("edit/{id}")]
    [RequirePermission("Plans.Edit")]
    public async Task<IActionResult> Edit(Guid id, UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Plan"];
        if (id != command.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"].Value;
            return View(command);
        }
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Plan updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    [RequirePermission("Plans.View")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Plan Details"];
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    [RequirePermission("Plans.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Plan"];
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpPost("delete/{id}")]
    [RequirePermission("Plans.Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeletePlanCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = _localizer["Plan deleted successfully"].Value;
        return RedirectToAction(nameof(Index));
    }
}
