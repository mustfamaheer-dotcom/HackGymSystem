using Gym.API.ViewModels;
using Gym.Application.Common.Interfaces;
using Gym.Application.Members.DTOs;
using Gym.Application.Members.Import;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Members")]
public class MembersMvcController : Controller
{
    private readonly IMemberService _memberService;
    private readonly IExcelImportService _excelImportService;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IRepository<Membership> _membershipRepository;
    private readonly IRepository<Attendance> _attendanceRepository;

    public MembersMvcController(IMemberService memberService, IExcelImportService excelImportService,
        IRepository<MembershipPlan> planRepository, IRepository<Membership> membershipRepository,
        IRepository<Attendance> attendanceRepository)
    {
        _memberService = memberService;
        _excelImportService = excelImportService;
        _planRepository = planRepository;
        _membershipRepository = membershipRepository;
        _attendanceRepository = attendanceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Members";

        var result = await _memberService.GetAllAsync(page, pageSize, searchTerm, sortBy, sortDescending, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SortBy = sortBy;
        ViewBag.SortDescending = sortDescending;

        return View(result.Data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Member";
        return View(new CreateMemberDto());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateMemberDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Member";

        var result = await _memberService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = "Member created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Member";
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var result = await _memberService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var member = result.Data!;
        var dto = new UpdateMemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            PhoneNumber = member.PhoneNumber,
            Nationality = member.Nationality,
            NationalId = member.NationalId,
            Email = member.Email,
            DateOfBirth = member.DateOfBirth,
            Gender = member.Gender?.ToString(),
            Notes = member.Notes,
            Company = member.Company,
            Address = member.Address,
            Weight = member.Weight,
            HasDisease = member.HasDisease,
            DiseaseType = member.DiseaseType,
            ReferralSource = member.ReferralSource,
            PackageId = member.PackageId,
            SubscriptionPrice = member.SubscriptionPrice,
            PaidAmount = member.PaidAmount,
            SubscriptionDurationMonths = member.SubscriptionDurationMonths,
            FreeMonths = member.FreeMonths,
            FreezeDays = member.FreezeDays,
            SubscriptionStartDate = member.SubscriptionStartDate,
            PaymentMethod = member.PaymentMethod.ToString(),
            FingerprintDeviceId = member.FingerprintDeviceId,
            MemberSignature = member.MemberSignature,
            AdminSignature = member.AdminSignature
        };

        return View(dto);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Member";
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        if (id != dto.Id)
        {
            TempData["Error"] = "Route ID and form ID do not match";
            return View(dto);
        }

        var result = await _memberService.UpdateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = "Member updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Member Details";

        var result = await _memberService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var memberships = await _membershipRepository.Query()
            .Include(m => m.Plan)
            .Where(m => m.MemberId == id)
            .OrderByDescending(m => m.StartDate)
            .ToListAsync(cancellationToken);

        var attendances = await _attendanceRepository.Query()
            .Include(a => a.Device)
            .Where(a => a.MemberId == id)
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.Time)
            .Take(50)
            .ToListAsync(cancellationToken);

        ViewBag.Memberships = memberships;
        ViewBag.Attendances = attendances;

        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Delete Member";

        var result = await _memberService.GetByIdAsync(id, cancellationToken);

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
        var result = await _memberService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Member deleted successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(MemberSearchViewModel model, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Search Members";

        model.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var hasFilters = !string.IsNullOrWhiteSpace(model.Name)
            || !string.IsNullOrWhiteSpace(model.NationalId)
            || !string.IsNullOrWhiteSpace(model.PhoneNumber)
            || !string.IsNullOrWhiteSpace(model.ReceiptNumber)
            || model.PackageId.HasValue
            || !string.IsNullOrWhiteSpace(model.SubscriptionStatus)
            || !string.IsNullOrWhiteSpace(model.PaymentStatus)
            || model.ExpiringSoon
            || model.OutstandingBalance;

        if (!hasFilters)
            return View(model);

        var result = await _memberService.AdvancedSearchAsync(
            model.Name, model.NationalId, model.PhoneNumber, model.ReceiptNumber,
            model.PackageId, model.SubscriptionStatus, model.PaymentStatus,
            model.ExpiringSoon, model.ExpiringWithinDays, model.OutstandingBalance,
            model.Page, model.PageSize, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(model);
        }

        model.Results = result.Data;
        return View(model);
    }

    [HttpGet("import")]
    public IActionResult Import()
    {
        ViewData["Title"] = "Import Members";
        return View();
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Import Members";

        if (file is null || file.Length == 0)
        {
            TempData["Error"] = "Please select a file to upload";
            return View();
        }

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "File must be an .xlsx file";
            return View();
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var result = await _excelImportService.ImportMembersAsync(stream, file.FileName, cancellationToken);

        return View("ImportResult", result);
    }
}
