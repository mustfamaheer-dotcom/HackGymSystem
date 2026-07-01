using Gym.Application.Common.Interfaces;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Offers")]
public class OffersMvcController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IRepository<MembershipPlan> _planRepository;

    public OffersMvcController(IOfferService offerService, IRepository<MembershipPlan> planRepository)
    {
        _offerService = offerService;
        _planRepository = planRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Offers";
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
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Offer";
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        return View(new CreateOfferDto());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateOfferDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Offer";
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

        TempData["Success"] = "Offer created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Offer";
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
            OfferType = dto.OfferType,
            BonusMonths = dto.BonusMonths,
            BonusDays = dto.BonusDays,
            OfferPrice = dto.OfferPrice,
            ExtraFreezeDays = dto.ExtraFreezeDays,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        return View(model);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateOfferDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Offer";
        ViewBag.Plans = await _planRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        if (id != dto.Id)
        {
            TempData["Error"] = "Route ID and form ID do not match";
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

        TempData["Success"] = "Offer updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Offer Details";
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
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
        ViewData["Title"] = "Delete Offer";
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
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
        var result = await _offerService.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = "Offer deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
