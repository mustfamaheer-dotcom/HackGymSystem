using System.Text.Json;
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
using Gym.Application.Leads.Queries.GetLeadStats;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.MembershipPlans.Queries.GetAllPlans;
using Gym.API.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Leads")]
public class LeadsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepository<WhatsAppTemplate> _templateRepo;
    private readonly IRepository<Offer> _offerRepo;

    public LeadsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer, IRepository<WhatsAppTemplate> templateRepo, IRepository<Offer> offerRepo)
    {
        _mediator = mediator;
        _localizer = localizer;
        _templateRepo = templateRepo;
        _offerRepo = offerRepo;
    }

    [HttpGet]
    [RequirePermission("Leads.View")]
    public async Task<IActionResult> Index(string? searchTerm = null, string? statusFilter = null, string? genderFilter = null, string? sourceFilter = null, Guid? packageFilter = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Leads"];
        ViewBag.SearchTerm = searchTerm;
        ViewBag.StatusFilter = statusFilter;
        ViewBag.GenderFilter = genderFilter;
        ViewBag.SourceFilter = sourceFilter;
        ViewBag.PackageFilter = packageFilter;

        LeadStatus? parsedStatus = null;
        if (Enum.TryParse<LeadStatus>(statusFilter, true, out var s))
            parsedStatus = s;

        Gender? parsedGender = null;
        if (Enum.TryParse<Gender>(genderFilter, true, out var g))
            parsedGender = g;

        LeadSource? parsedSource = null;
        if (Enum.TryParse<LeadSource>(sourceFilter, true, out var src))
            parsedSource = src;

        var query = new GetAllLeadsQuery(searchTerm, parsedStatus, parsedGender, parsedSource, packageFilter, null, null, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        var statsResult = await _mediator.Send(new GetLeadStatsQuery(), cancellationToken);
        ViewBag.LeadStats = statsResult.IsSuccess ? statsResult.Data : null;

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        var activeOffers = await _offerRepo.Query().Where(o => o.IsActive).OrderBy(o => o.OfferTitle).ToListAsync(cancellationToken);
        ViewBag.ActiveOffersJson = JsonSerializer.Serialize(activeOffers.Select(o => new { o.OfferTitle, o.OfferType, o.OfferPrice, o.BonusMonths, o.BonusDays, o.ExtraFreezeDays }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        var allPlans = await _mediator.Send(new GetAllPlansQuery { PageSize = 1000 }, cancellationToken);
        ViewBag.Plans = allPlans.IsSuccess ? allPlans.Data?.Items ?? new List<PlanDto>() : new List<PlanDto>();

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
        var command = new UpdateLeadCommand(dto.Id, dto.Name, dto.Phone, dto.Source, dto.InterestedPackageId, dto.Status, dto.NextFollowUpDate, dto.Notes, dto.Email, dto.Gender);
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

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        var activeOffers = await _offerRepo.Query().Where(o => o.IsActive).OrderBy(o => o.OfferTitle).ToListAsync(cancellationToken);
        ViewBag.ActiveOffersJson = JsonSerializer.Serialize(activeOffers.Select(o => new { o.OfferTitle, o.OfferType, o.OfferPrice, o.BonusMonths, o.BonusDays, o.ExtraFreezeDays }), new JsonSerializerOptions { PropertyNamingPolicy = null });

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