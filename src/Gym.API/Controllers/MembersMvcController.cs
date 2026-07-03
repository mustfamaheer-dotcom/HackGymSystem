using Gym.API.ViewModels;
using Gym.Application.Common.Interfaces;
using Gym.Application.Members.DTOs;
using Gym.Application.Members.Import;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.API.Filters;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Members")]
public class MembersMvcController : Controller
{
    private readonly IMemberService _memberService;
    private readonly IExcelImportService _excelImportService;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IWebHostEnvironment _env;

    public MembersMvcController(IMemberService memberService, IExcelImportService excelImportService,
        IRepository<MembershipPlan> planRepository,
        IRepository<Attendance> attendanceRepository,
        IStringLocalizer<SharedResources> localizer,
        IWebHostEnvironment env)
    {
        _memberService = memberService;
        _excelImportService = excelImportService;
        _planRepository = planRepository;
        _attendanceRepository = attendanceRepository;
        _localizer = localizer;
        _env = env;
    }

    [RequirePermission("Members.View")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Members"];

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

    [RequirePermission("Members.Create")]
    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Member"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        return View(new CreateMemberDto());
    }

    [RequirePermission("Members.Create")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateMemberDto dto, IFormFile? imageFile, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Member"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        if (!ModelState.IsValid)
            return View(dto);

        var result = await _memberService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        if (imageFile is not null && imageFile.Length > 0)
        {
            var path = await SaveMemberImageAsync(result.Data, imageFile);
            await _memberService.UpdateImagePathAsync(result.Data, path, cancellationToken);
        }

        TempData["Success"] = _localizer["Member created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Members.Edit")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Member"];
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
            FingerprintDeviceId = member.FingerprintDeviceId,
            MemberSignature = member.MemberSignature,
            AdminSignature = member.AdminSignature,
            ImagePath = member.ImagePath
        };

        return View(dto);
    }

    [RequirePermission("Members.Edit")]
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateMemberDto dto, IFormFile? imageFile, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Member"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        if (id != dto.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"].Value;
            return View(dto);
        }

        if (!ModelState.IsValid)
            return View(dto);

        if (imageFile is not null && imageFile.Length > 0)
        {
            DeleteExistingMemberImage(dto.Id);
            dto.ImagePath = await SaveMemberImageAsync(dto.Id, imageFile);
        }

        var result = await _memberService.UpdateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = _localizer["Member updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Members.View")]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Member Details"];

        var result = await _memberService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var attendances = await _attendanceRepository.Query()
            .Include(a => a.Device)
            .Where(a => a.MemberId == id)
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.Time)
            .Take(50)
            .ToListAsync(cancellationToken);

        ViewBag.Attendances = attendances;

        return View(result.Data);
    }

    [RequirePermission("Members.Delete")]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Member"];

        var result = await _memberService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [RequirePermission("Members.Delete")]
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _memberService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = _localizer["Member deleted successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Members.View")]
    [HttpGet("search")]
    public async Task<IActionResult> Search(MemberSearchViewModel model, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Search Members"];

        model.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var hasFilters = !string.IsNullOrWhiteSpace(model.Name)
            || !string.IsNullOrWhiteSpace(model.NationalId)
            || !string.IsNullOrWhiteSpace(model.PhoneNumber)
            || model.Code.HasValue
            || !string.IsNullOrWhiteSpace(model.ReceiptNumber)
            || model.PackageId.HasValue;

        if (!hasFilters)
            return View(model);

        var result = await _memberService.AdvancedSearchAsync(
            model.Name, model.NationalId, model.PhoneNumber, model.Code, model.ReceiptNumber,
            model.PackageId, null, null,
            false, 7, false,
            model.Page, model.PageSize, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(model);
        }

        model.Results = result.Data;
        return View(model);
    }

    [RequirePermission("Members.View")]
    [HttpGet("import")]
    public IActionResult Import()
    {
        ViewData["Title"] = _localizer["Import Members"];
        return View();
    }

    [RequirePermission("Members.View")]
    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Import Members"];

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

        var result = await _excelImportService.ImportMembersAsync(stream, file.FileName, cancellationToken);

        return View("ImportResult", result);
    }

    private async Task<string> SaveMemberImageAsync(Guid memberId, IFormFile imageFile)
    {
        var uploadsDir = Path.Combine(_env.WebRootPath, "Media-images", memberId.ToString());
        Directory.CreateDirectory(uploadsDir);

        var ext = Path.GetExtension(imageFile.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return $"Media-images/{memberId}/{fileName}";
    }

    private void DeleteExistingMemberImage(Guid memberId)
    {
        var dir = Path.Combine(_env.WebRootPath, "Media-images", memberId.ToString());
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, recursive: true);
        }
    }
}
