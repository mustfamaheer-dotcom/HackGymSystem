using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.MembershipPlans.Commands.CreatePlan;
using Gym.Application.MembershipPlans.Commands.UpdatePlan;
using Gym.Application.MembershipPlans.Commands.DeletePlan;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.Application.MembershipPlans.Queries.GetPlanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Plans")]
public class PlansMvcController : Controller
{
    private readonly IMediator _mediator;

    public PlansMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Plans";
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
    public IActionResult Create()
    {
        ViewData["Title"] = "New Plan";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreatePlanCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Plan";
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = "Plan created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Plan";
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var command = new UpdatePlanCommand(dto.Id, dto.Name, dto.Price, dto.DurationDays, dto.MaxVisits, dto.FreezeDays, dto.Description);
        return View(command);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdatePlanCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Plan";
        if (id != command.Id)
        {
            TempData["Error"] = "Route ID and form ID do not match";
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
        TempData["Success"] = "Plan updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Plan Details";
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Delete Plan";
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeletePlanCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = "Plan deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
