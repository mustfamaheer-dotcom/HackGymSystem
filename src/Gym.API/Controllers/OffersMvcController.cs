using Gym.Application.Offers.DTOs;
using Gym.Application.Offers.Commands.CreateOffer;
using Gym.Application.Offers.Commands.UpdateOffer;
using Gym.Application.Offers.Commands.DeleteOffer;
using Gym.Application.Offers.Queries.GetAllOffers;
using Gym.Application.Offers.Queries.GetOfferById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Offers")]
public class OffersMvcController : Controller
{
    private readonly IMediator _mediator;

    public OffersMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Offers";
        var query = new GetAllOffersQuery { Page = page, PageSize = pageSize, SearchTerm = searchTerm };
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
        ViewData["Title"] = "New Offer";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateOfferCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Offer";
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = "Offer created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Offer";
        var result = await _mediator.Send(new GetOfferByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var command = new UpdateOfferCommand(dto.Id, dto.Title, dto.Description, dto.DiscountType, dto.DiscountValue, dto.StartDate, dto.EndDate);
        return View(command);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateOfferCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Offer";
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
        TempData["Success"] = "Offer updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Offer Details";
        var result = await _mediator.Send(new GetOfferByIdQuery(id), cancellationToken);
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
        var result = await _mediator.Send(new GetOfferByIdQuery(id), cancellationToken);
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
        var result = await _mediator.Send(new DeleteOfferCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = "Offer deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
