using Gym.Application.Common.Interfaces;
using Gym.Application.Packages.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Packages")]
public class PackagesMvcController : Controller
{
    private readonly IPackageService _packageService;

    public PackagesMvcController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Packages";

        var result = string.IsNullOrWhiteSpace(searchTerm)
            ? await _packageService.GetAllAsync(cancellationToken)
            : await _packageService.SearchAsync(searchTerm, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(new List<PackageDto>());
        }

        ViewBag.SearchTerm = searchTerm;
        return View(result.Data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Package";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Package";

        if (!ModelState.IsValid)
            return View();

        var result = await _packageService.CreateAsync(
            packageName, durationMonths, price, freePeriodMonths, maxFreezeDays, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View();
        }

        TempData["Success"] = "Package created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Package";

        var result = await _packageService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Package";

        if (!ModelState.IsValid)
        {
            var dto = await _packageService.GetByIdAsync(id, cancellationToken);
            return View(dto.Data);
        }

        var result = await _packageService.UpdateAsync(
            id, packageName, durationMonths, price, freePeriodMonths, maxFreezeDays, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;

            var dto = await _packageService.GetByIdAsync(id, cancellationToken);
            return View(dto.Data);
        }

        TempData["Success"] = "Package updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Package Details";

        var result = await _packageService.GetByIdAsync(id, cancellationToken);

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
        ViewData["Title"] = "Delete Package";

        var result = await _packageService.GetByIdAsync(id, cancellationToken);

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
        var result = await _packageService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Package deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
