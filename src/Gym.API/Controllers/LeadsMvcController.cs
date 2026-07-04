using System.Text.Json;
using ClosedXML.Excel;
using Gym.API.Filters;
using Gym.Application.Common.Interfaces;
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
using Gym.API;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    private readonly IExcelImportService _excelImportService;
    private readonly IRepository<Lead> _leadRepository;
    private readonly IRepository<WhatsAppTemplate> _templateRepo;
    private readonly IRepository<Offer> _offerRepo;
    private readonly IWebHostEnvironment _env;

    public LeadsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer, IExcelImportService excelImportService, IRepository<Lead> leadRepository, IRepository<WhatsAppTemplate> templateRepo, IRepository<Offer> offerRepo, IWebHostEnvironment env)
    {
        _mediator = mediator;
        _localizer = localizer;
        _excelImportService = excelImportService;
        _leadRepository = leadRepository;
        _templateRepo = templateRepo;
        _offerRepo = offerRepo;
        _env = env;
    }

    [HttpGet]
    [RequirePermission("Leads.View")]
    public async Task<IActionResult> Index(string? searchTerm = null, string? statusFilter = null, string? genderFilter = null, string? sourceFilter = null, Guid? packageFilter = null, DateTime? dateFrom = null, DateTime? dateTo = null, DateTime? nextFollowUpFrom = null, DateTime? nextFollowUpTo = null, bool? hasFollowUp = null, string? sortBy = null, bool sortDescending = false, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Leads"];
        ViewBag.SearchTerm = searchTerm;
        ViewBag.StatusFilter = statusFilter;
        ViewBag.GenderFilter = genderFilter;
        ViewBag.SourceFilter = sourceFilter;
        ViewBag.PackageFilter = packageFilter;
        ViewBag.DateFrom = dateFrom;
        ViewBag.DateTo = dateTo;
        ViewBag.NextFollowUpFrom = nextFollowUpFrom;
        ViewBag.NextFollowUpTo = nextFollowUpTo;
        ViewBag.HasFollowUp = hasFollowUp;
        ViewBag.SortBy = sortBy;
        ViewBag.SortDescending = sortDescending;

        LeadStatus? parsedStatus = null;
        if (Enum.TryParse<LeadStatus>(statusFilter, true, out var s))
            parsedStatus = s;

        Gender? parsedGender = null;
        if (Enum.TryParse<Gender>(genderFilter, true, out var g))
            parsedGender = g;

        LeadSource? parsedSource = null;
        if (Enum.TryParse<LeadSource>(sourceFilter, true, out var src))
            parsedSource = src;

        var query = new GetAllLeadsQuery(searchTerm, parsedStatus, parsedGender, parsedSource, packageFilter, dateFrom, dateTo, nextFollowUpFrom, nextFollowUpTo, hasFollowUp, sortBy, sortDescending, page, pageSize);
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
        TempData["Success"] = string.Format(_localizer["Lead '{0}' has been added successfully"].Value, command.Name);
        return RedirectToAction(nameof(Index));
    }

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
        TempData["Success"] = string.Format(_localizer["Lead '{0}' has been updated successfully"].Value, command.Name);
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

    [RequirePermission("Leads.View")]
    [HttpGet("import")]
    public IActionResult Import()
    {
        ViewData["Title"] = _localizer["Import Leads"];
        return View();
    }

    [RequirePermission("Leads.View")]
    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Import Leads"];

        if (file is null || file.Length == 0)
        {
            TempData["Error"] = _localizer["Please select a file to upload"].Value;
            return View();
        }

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = _localizer["File must be an .xlsx file"].Value;
            return View();
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var result = await _excelImportService.ImportLeadsAsync(stream, file.FileName, cancellationToken);

        return View("ImportResult", result);
    }

    [RequirePermission("Leads.View")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken)
    {
        var leads = await _leadRepository.Query()
            .Include(l => l.InterestedPackage)
            .IgnoreQueryFilters()
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Leads");

        var headers = new (int Col, string Label)[]
        {
            (1,  "Name"),
            (2,  "Phone"),
            (3,  "Email"),
            (4,  "Gender"),
            (5,  "Source"),
            (6,  "Status"),
            (7,  "Package"),
            (8,  "Notes"),
            (9,  "Next Follow Up"),
            (10, "Created At"),
        };

        foreach (var (col, label) in headers)
        {
            sheet.Cell(1, col).Value = label;
        }

        var headerRow = sheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#F0AD4E");
        headerRow.Style.Font.FontColor = XLColor.White;

        var row = 2;
        foreach (var l in leads)
        {
            sheet.Cell(row, 1).Value = l.Name;
            sheet.Cell(row, 2).Value = l.Phone;
            sheet.Cell(row, 3).Value = l.Email;
            sheet.Cell(row, 4).Value = l.Gender?.ToString();
            sheet.Cell(row, 5).Value = l.Source.ToString();
            sheet.Cell(row, 6).Value = l.Status.ToString();
            sheet.Cell(row, 7).Value = l.InterestedPackage?.Name;
            sheet.Cell(row, 8).Value = l.Notes;
            sheet.Cell(row, 9).Value = l.NextFollowUpDate?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 10).Value = l.CreatedAt.ToString("yyyy-MM-dd HH:mm");
            row++;
        }

        sheet.Columns().AdjustToContents();

        var fileName = $"Leads_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
        var exportDir = Path.Combine(_env.ContentRootPath, "Exported Excel Sheets");
        Directory.CreateDirectory(exportDir);
        var filePath = Path.Combine(exportDir, fileName);
        workbook.SaveAs(filePath);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        TempData["Success"] = string.Format(_localizer["Leads exported to {0}"].Value, filePath);

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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