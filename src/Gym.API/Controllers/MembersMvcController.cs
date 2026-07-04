using System.Text.Json;
using Gym.API.Services;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gym.API.Filters;
using Gym.API;
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
    private readonly ReceiptPdfService _pdfService;
    private readonly IRepository<WhatsAppTemplate> _templateRepo;
    private readonly IRepository<Offer> _offerRepo;
    private readonly IRepository<Subscription> _subscriptionRepo;

    public MembersMvcController(IMemberService memberService, IExcelImportService excelImportService,
        IRepository<MembershipPlan> planRepository,
        IRepository<Attendance> attendanceRepository,
        IStringLocalizer<SharedResources> localizer,
        IWebHostEnvironment env,
        ReceiptPdfService pdfService,
        IRepository<WhatsAppTemplate> templateRepo,
        IRepository<Offer> offerRepo,
        IRepository<Subscription> subscriptionRepo)
    {
        _memberService = memberService;
        _excelImportService = excelImportService;
        _planRepository = planRepository;
        _attendanceRepository = attendanceRepository;
        _localizer = localizer;
        _env = env;
        _pdfService = pdfService;
        _templateRepo = templateRepo;
        _offerRepo = offerRepo;
        _subscriptionRepo = subscriptionRepo;
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

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        var activeOffers = await _offerRepo.Query().Where(o => o.IsActive).OrderBy(o => o.OfferTitle).ToListAsync(cancellationToken);
        ViewBag.ActiveOffersJson = JsonSerializer.Serialize(activeOffers.Select(o => new { o.OfferTitle, o.OfferType, o.OfferPrice, o.BonusMonths, o.BonusDays, o.ExtraFreezeDays }), new JsonSerializerOptions { PropertyNamingPolicy = null });

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
        await PopulateOffers(cancellationToken);
        return View(new CreateMemberDto
        {
            AdminSignature = $"{User.Identity?.Name} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });
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
        await PopulateOffers(cancellationToken);

        if (!ModelState.IsValid)
            return View(dto);

        var result = await _memberService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Message?.Contains("National ID") == true)
                ModelState.AddModelError("NationalId", result.Message);
            else if (result.Message?.Contains("Phone number") == true)
                ModelState.AddModelError("PhoneNumber", result.Message);
            else
                ModelState.AddModelError("", result.Message ?? _localizer["An error occurred"].Value);
            return View(dto);
        }

        if (imageFile is not null && imageFile.Length > 0)
        {
            var path = await SaveMemberImageAsync(result.Data, imageFile);
            await _memberService.UpdateImagePathAsync(result.Data, path, cancellationToken);
        }

        TempData["Success"] = string.Format(_localizer["Member '{0}' has been registered successfully"].Value, dto.FullName);
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
        await PopulateOffers(cancellationToken);

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
            AdminSignature = member.AdminSignature ?? $"{User.Identity?.Name} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            ImagePath = member.ImagePath
        };

        var activeSub = await _subscriptionRepo.Query()
            .Where(s => s.MemberId == id && s.Status == SubscriptionStatus.Active)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeSub is not null)
        {
            dto.OfferId = activeSub.OfferId;
            dto.SubscriptionPrice = activeSub.TotalSubscriptionValue;
            dto.PaidAmount = activeSub.AmountPaid;
            dto.DurationMonths = (int)Math.Ceiling((activeSub.ExpirationDate - activeSub.StartDate).TotalDays / 30);
            dto.StartDate = activeSub.StartDate;
            dto.PaymentMethod = activeSub.PaymentMethod.ToString();
        }

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
        await PopulateOffers(cancellationToken);

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

        TempData["Success"] = string.Format(_localizer["Member '{0}' has been updated successfully"].Value, dto.FullName);
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

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        var activeOffers = await _offerRepo.Query().Where(o => o.IsActive).OrderBy(o => o.OfferTitle).ToListAsync(cancellationToken);
        ViewBag.ActiveOffersJson = JsonSerializer.Serialize(activeOffers.Select(o => new { o.OfferTitle, o.OfferType, o.OfferPrice, o.BonusMonths, o.BonusDays, o.ExtraFreezeDays }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        return View(result.Data);
    }

    [RequirePermission("Members.View")]
    [HttpGet("payment-history/{id}")]
    public async Task<IActionResult> PaymentHistory(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Payment History"];

        var memberResult = await _memberService.GetByIdAsync(id, cancellationToken);
        if (memberResult.IsFailure)
        {
            TempData["Error"] = memberResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var member = memberResult.Data!;
        var subscriptions = await _subscriptionRepo.Query()
            .Include(s => s.Payments).ThenInclude(p => p.Employee)
            .Include(s => s.Plan)
            .Where(s => s.MemberId == id)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        var payments = subscriptions
            .SelectMany(s => s.Payments.Select(p => new MemberPaymentDto
            {
                PaymentId = p.Id,
                PaymentDate = p.CreatedAt,
                SubscriptionReceipt = s.ReceiptNumber,
                PlanName = s.Plan.Name,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                RunningBalance = p.RunningBalance,
                RecordedBy = p.Employee?.FullName
            }))
            .OrderByDescending(p => p.PaymentDate)
            .ToList();

        var viewModel = new MemberPaymentHistoryViewModel
        {
            MemberId = member.Id,
            MemberCode = member.Code,
            MemberName = member.FullName,
            MemberPhone = member.PhoneNumber,
            TotalPaid = payments.Sum(p => p.Amount),
            PaymentCount = payments.Count,
            Payments = payments
        };

        return View(viewModel);
    }

    [RequirePermission("Members.View")]
    [HttpGet("payment-history-pdf/{id}")]
    public async Task<IActionResult> PaymentHistoryPdf(Guid id, CancellationToken cancellationToken)
    {
        var memberResult = await _memberService.GetByIdAsync(id, cancellationToken);
        if (memberResult.IsFailure)
            return NotFound();

        var member = memberResult.Data!;
        var subscriptions = await _subscriptionRepo.Query()
            .Include(s => s.Payments).ThenInclude(p => p.Employee)
            .Include(s => s.Plan)
            .Where(s => s.MemberId == id)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        var payments = subscriptions
            .SelectMany(s => s.Payments.Select(p => new MemberPaymentDto
            {
                PaymentId = p.Id,
                PaymentDate = p.CreatedAt,
                SubscriptionReceipt = s.ReceiptNumber,
                PlanName = s.Plan.Name,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                RunningBalance = p.RunningBalance,
                RecordedBy = p.Employee?.FullName
            }))
            .OrderByDescending(p => p.PaymentDate)
            .ToList();

        var pdfDir = @"E:\WORK\FreeLance\ORBiT\SYSTEMS\GYMS\C. Amir - Hack Gym\Phase 1\System\Payments History";
        Directory.CreateDirectory(pdfDir);
        var pdfFileName = $"payment-history-{member.Code}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.pdf";
        var pdfFilePath = Path.Combine(pdfDir, pdfFileName);
        var pdfBytes = _pdfService.GeneratePaymentHistory(member.FullName, member.Code, member.PhoneNumber, payments);
        System.IO.File.WriteAllBytes(pdfFilePath, pdfBytes);
        TempData["Success"] = string.Format(_localizer["Payment history saved to {0}"].Value, pdfFilePath);
        return File(pdfBytes, "application/pdf", pdfFileName);
    }

    [RequirePermission("Members.View")]
    [HttpGet("DownloadReceiptPdf/{id}")]
    public async Task<IActionResult> DownloadReceiptPdf(Guid id, CancellationToken cancellationToken)
    {
        var result = await _memberService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var pdfBytes = _pdfService.GenerateReceipt(result.Data!);
        return File(pdfBytes, "application/pdf", $"receipt-{result.Data!.Code}.pdf");
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

    [RequirePermission("Members.View")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken)
    {
        var result = await _memberService.GetAllMembersForExportAsync(cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var members = result.Data;

        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var sheet = workbook.Worksheets.Add("Members");

        var headers = new (int Col, string Label)[]
        {
            (1,  "Code"),
            (2,  "Receipt Number"),
            (3,  "Full Name"),
            (4,  "Nationality"),
            (5,  "National ID"),
            (6,  "Phone Number"),
            (7,  "Email"),
            (8,  "Date of Birth"),
            (9,  "Gender"),
            (10, "Company"),
            (11, "Address"),
            (12, "Referral Source"),
            (13, "Registration Date"),
            (14, "Weight (kg)"),
            (15, "Has Disease"),
            (16, "Disease Type"),
            (17, "Notes"),
            (18, "Fingerprint Device ID"),
            (19, "Member Signature"),
            (20, "Admin Signature"),
            (21, "Plan Name"),
            (22, "Subscription Receipt"),
            (23, "Total Value"),
            (24, "Amount Paid"),
            (25, "Remaining Balance"),
            (26, "Payment Method"),
            (27, "Start Date"),
            (28, "Expiration Date"),
            (29, "Subscription Status"),
            (30, "Freeze Start"),
            (31, "Freeze End"),
            (32, "Total Freeze Days"),
            (33, "Subscription Notes"),
        };

        foreach (var (col, label) in headers)
        {
            sheet.Cell(1, col).Value = label;
        }

        var headerRow = sheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#F0AD4E");
        headerRow.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

        var row = 2;
        foreach (var m in members)
        {
            sheet.Cell(row, 1).Value = m.Code;
            sheet.Cell(row, 2).Value = m.ReceiptNumber;
            sheet.Cell(row, 3).Value = m.FullName;
            sheet.Cell(row, 4).Value = m.Nationality;
            sheet.Cell(row, 5).Value = m.NationalId;
            sheet.Cell(row, 6).Value = m.PhoneNumber;
            sheet.Cell(row, 7).Value = m.Email;
            sheet.Cell(row, 8).Value = m.DateOfBirth?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 9).Value = m.Gender;
            sheet.Cell(row, 10).Value = m.Company;
            sheet.Cell(row, 11).Value = m.Address;
            sheet.Cell(row, 12).Value = m.ReferralSource;
            sheet.Cell(row, 13).Value = m.RegistrationDate.ToString("yyyy-MM-dd");
            sheet.Cell(row, 14).Value = m.Weight?.ToString("F1");
            sheet.Cell(row, 15).Value = m.HasDisease ? "Yes" : "No";
            sheet.Cell(row, 16).Value = m.DiseaseType;
            sheet.Cell(row, 17).Value = m.Notes;
            sheet.Cell(row, 18).Value = m.FingerprintDeviceId;
            sheet.Cell(row, 19).Value = m.MemberSignature;
            sheet.Cell(row, 20).Value = m.AdminSignature;
            sheet.Cell(row, 21).Value = m.PlanName;
            sheet.Cell(row, 22).Value = m.SubReceiptNumber;
            sheet.Cell(row, 23).Value = m.TotalSubscriptionValue?.ToString("F2");
            sheet.Cell(row, 24).Value = m.AmountPaid?.ToString("F2");
            sheet.Cell(row, 25).Value = m.RemainingBalance?.ToString("F2");
            sheet.Cell(row, 26).Value = m.PaymentMethod;
            sheet.Cell(row, 27).Value = m.StartDate?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 28).Value = m.ExpirationDate?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 29).Value = m.SubStatus;
            sheet.Cell(row, 30).Value = m.FreezeStart?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 31).Value = m.FreezeEnd?.ToString("yyyy-MM-dd");
            sheet.Cell(row, 32).Value = m.TotalFreezeDays;
            sheet.Cell(row, 33).Value = m.SubNotes;
            row++;
        }

        sheet.Columns().AdjustToContents();

        var fileName = $"Members_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
        var exportDir = @"E:\WORK\FreeLance\ORBiT\SYSTEMS\GYMS\C. Amir - Hack Gym\Phase 1\System\Exported Excel Sheets";
        Directory.CreateDirectory(exportDir);
        var filePath = Path.Combine(exportDir, fileName);
        workbook.SaveAs(filePath);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        TempData["Success"] = string.Format(_localizer["Members exported to {0}"].Value, filePath);

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    private async Task PopulateOffers(CancellationToken cancellationToken)
    {
        var offers = await _offerRepo.Query()
            .Where(o => o.IsActive)
            .OrderBy(o => o.OfferTitle)
            .Select(o => new { o.Id, o.OfferTitle, o.OfferPrice, o.BonusMonths, o.BonusDays, o.LinkedPackageId, o.OfferType })
            .ToListAsync(cancellationToken);
        ViewBag.Offers = new SelectList(offers, "Id", "OfferTitle");
        ViewBag.OfferList = offers.Select(o =>
        {
            var parts = new List<string>();
            if (o.OfferPrice.HasValue) parts.Add($"{o.OfferPrice:N2} EGP");
            if (o.BonusMonths > 0) parts.Add($"+{o.BonusMonths}m");
            if (o.BonusDays > 0) parts.Add($"+{o.BonusDays}d");
            return new { o.Id, Display = $"{o.OfferTitle} ({string.Join(" ", parts)})", o.LinkedPackageId, o.OfferType, o.OfferPrice };
        }).ToList();
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
