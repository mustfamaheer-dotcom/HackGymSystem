using Gym.API.Filters;
using Gym.Application.Common.Interfaces;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Offers")]
public class OffersMvcController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IRepository<MembershipPlan> _planRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public OffersMvcController(IOfferService offerService, IRepository<MembershipPlan> planRepository,
        IStringLocalizer<SharedResources> localizer)
    {
        _offerService = offerService;
        _planRepository = planRepository;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Offers"];
        ViewBag.SearchTerm = searchTerm;

        var result = await _offerService.GetAllAsync(page, pageSize, searchTerm, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        return View(result.Data);
    }

    [HttpGet("create")]
    [RequirePermission("Offers.Create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Offer"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        return View(new CreateOfferDto());
    }

    [HttpPost("create")]
    [RequirePermission("Offers.Create")]
    public async Task<IActionResult> Create(CreateOfferDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Offer"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        if (!ModelState.IsValid)
            return View(dto);

        var result = await _offerService.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = _localizer["Offer created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    [RequirePermission("Offers.Edit")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Offer"];
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var result = await _offerService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

            var dto = result.Data!;
            var model = new UpdateOfferDto
            {
                Id = dto.Id,
                OfferTitle = dto.OfferTitle,
                LinkedPackageId = dto.LinkedPackageId,
                OfferType = OfferType.FixedPrice,
                OfferPrice = dto.OfferPrice,
                BonusMonths = dto.BonusMonths,
                BonusDays = dto.BonusDays,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

        return View(model);
    }

    [HttpPost("edit/{id}")]
    [RequirePermission("Offers.Edit")]
    public async Task<IActionResult> Edit(Guid id, UpdateOfferDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Offer"];
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

        var result = await _offerService.UpdateAsync(dto, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = _localizer["Offer updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Offer Details"];
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpGet("delete/{id}")]
    [RequirePermission("Offers.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Offer"];
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpPost("delete/{id}")]
    [RequirePermission("Offers.Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _offerService.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = _localizer["Offer deleted successfully"].Value;
        return RedirectToAction(nameof(Index));
    }
}
