using Gym.API.Filters;
using Gym.Application.Leads.Commands.AddFollowUp;
using Gym.Application.Leads.Commands.ConvertToMember;
using Gym.Application.Leads.Commands.CreateLead;
using Gym.Application.Leads.Commands.DeleteLead;
using Gym.Application.Leads.Commands.UpdateLead;
using Gym.Application.Leads.DTOs;
using Gym.Application.Leads.Queries.GetAllLeads;
using Gym.Application.Leads.Queries.GetFollowUps;
using Gym.Application.Leads.Queries.GetLeadById;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.API.Resources;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Leads")]
public class LeadsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public LeadsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Leads.View")]
    public async Task<IActionResult> Index(string? searchTerm = null, string? statusFilter = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Leads"];
        ViewBag.SearchTerm = searchTerm;
        ViewBag.StatusFilter = statusFilter;
        LeadStatus? parsedStatus = null;
        if (Enum.TryParse<LeadStatus>(statusFilter, true, out var s))
            parsedStatus = s;
        var query = new GetAllLeadsQuery(searchTerm, parsedStatus, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }
        return View(result.Data);
    }

    [HttpGet("create")]
    [RequirePermission("Leads.Create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Lead"];
        var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
        return View();
    }

    [HttpPost("create")]
    [RequirePermission("Leads.Create")]
    public async Task<IActionResult> Create(CreateLeadCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Lead"];
        var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Lead created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    [RequirePermission("Leads.Edit")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Lead"];
        var result = await _mediator.Send(new GetLeadByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
        var command = new UpdateLeadCommand(dto.Id, dto.Name, dto.Phone, dto.Source, dto.InterestedPackageId, dto.Status, dto.NextFollowUpDate, dto.Notes);
        return View(command);
    }

    [HttpPost("edit/{id}")]
    [RequirePermission("Leads.Edit")]
    public async Task<IActionResult> Edit(Guid id, UpdateLeadCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Lead"];
        if (id != command.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"].Value;
            return View(command);
        }
        var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Lead updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    [RequirePermission("Leads.View")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Lead Details"];
        var result = await _mediator.Send(new GetLeadByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var followUpsResult = await _mediator.Send(new GetFollowUpsQuery(id), cancellationToken);
        ViewBag.FollowUps = followUpsResult.IsSuccess ? followUpsResult.Data ?? new List<LeadFollowUpDto>() : new List<LeadFollowUpDto>();
        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    [RequirePermission("Leads.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Lead"];
        var result = await _mediator.Send(new GetLeadByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpPost("delete/{id}")]
    [RequirePermission("Leads.Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteLeadCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = _localizer["Lead deleted successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("convert/{id}")]
    [RequirePermission("Leads.Convert")]
    public async Task<IActionResult> Convert(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Convert Lead to Member"];
        var leadResult = await _mediator.Send(new GetLeadByIdQuery(id), cancellationToken);
        if (leadResult.IsFailure)
        {
            TempData["Error"] = leadResult.Message;
            return RedirectToAction(nameof(Index));
        }
        var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
        return View(leadResult.Data);
    }

    [HttpPost("convert/{id}")]
    [RequirePermission("Leads.Convert")]
    public async Task<IActionResult> Convert(Guid id, ConvertToMemberCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Convert Lead to Member"];
        if (id != command.LeadId)
        {
            TempData["Error"] = _localizer["Route ID and body ID do not match"].Value;
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var leadResult = await _mediator.Send(new GetLeadByIdQuery(id), cancellationToken);
            var plansResult = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
            ViewBag.Plans = plansResult.IsSuccess ? plansResult.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();
            return View(leadResult.Data);
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
        TempData["Success"] = _localizer["Lead converted to member successfully"].Value;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost("add-follow-up/{leadId}")]
    [RequirePermission("Leads.Edit")]
    public async Task<IActionResult> AddFollowUp(Guid leadId, AddFollowUpCommand command, CancellationToken cancellationToken)
    {
        if (leadId != command.LeadId)
        {
            TempData["Error"] = _localizer["Route ID and body ID do not match"].Value;
            return RedirectToAction(nameof(Details), new { id = leadId });
        }
        if (string.IsNullOrWhiteSpace(command.Notes))
        {
            TempData["Error"] = _localizer["Notes are required"].Value;
            return RedirectToAction(nameof(Details), new { id = leadId });
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = _localizer["Follow-up added successfully"].Value;
        }
        return RedirectToAction(nameof(Details), new { id = leadId });
    }
}