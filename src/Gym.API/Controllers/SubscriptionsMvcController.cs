using System.Text.Json;
using Gym.API.Filters;
using Gym.API.Resources;
using Gym.Application.Common.DTOs;
using Gym.Application.Subscriptions.Commands.CreateSubscription;
using Gym.Application.Subscriptions.Commands.FreezeSubscription;
using Gym.Application.Subscriptions.Commands.RecordSubscriptionPayment;
using Gym.Application.Subscriptions.Commands.RenewSubscription;
using Gym.Application.Subscriptions.Commands.UnfreezeSubscription;
using Gym.Application.Subscriptions.DTOs;
using Gym.Application.Subscriptions.Queries.GetAllSubscriptions;
using Gym.Application.Subscriptions.Queries.GetSubscriptionById;
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
[Route("Subscriptions")]
public class SubscriptionsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IRepository<Offer> _offerRepo;
    private readonly IRepository<WhatsAppTemplate> _templateRepo;

    public SubscriptionsMvcController(
        IMediator mediator,
        IStringLocalizer<SharedResources> localizer,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Offer> offerRepo,
        IRepository<WhatsAppTemplate> templateRepo)
    {
        _mediator = mediator;
        _localizer = localizer;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _offerRepo = offerRepo;
        _templateRepo = templateRepo;
    }

    [RequirePermission("Subscriptions.View")]
    [HttpGet]
    public async Task<IActionResult> Index(GetAllSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Subscriptions"];

        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(new PaginatedResult<SubscriptionDto>());
        }

        ViewBag.SearchTerm = query.SearchTerm;
        ViewBag.StatusFilter = query.StatusFilter;
        ViewBag.DateFrom = query.DateFrom?.ToString("yyyy-MM-dd");
        ViewBag.DateTo = query.DateTo?.ToString("yyyy-MM-dd");
        ViewBag.ExpiresWithinDays = query.ExpiresWithinDays;

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateData = templates.Select(t => new { t.Id, t.Name, t.MessageBody }).ToList();
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        return View(result.Data);
    }

    [RequirePermission("Subscriptions.Create")]
    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Subscription"];
        await PopulateDropdowns(cancellationToken);
        return View();
    }

    [RequirePermission("Subscriptions.Create")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(cancellationToken);
            return View(command);
        }

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            await PopulateDropdowns(cancellationToken);
            return View(command);
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id = result.Data });
    }

    [RequirePermission("Subscriptions.View")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Subscription Details"];

        var result = await _mediator.Send(new GetSubscriptionByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        await PopulateDetailsDropdowns(cancellationToken);

        var templates = await _templateRepo.Query().Where(t => t.IsActive).ToListAsync(cancellationToken);
        ViewBag.WhatsAppTemplates = new SelectList(templates, "Id", "Name");
        ViewBag.WhatsAppTemplateData = templates.Select(t => new { t.Id, t.Name, t.MessageBody }).ToList();
        ViewBag.WhatsAppTemplateJson = JsonSerializer.Serialize(templates.Select(t => new { t.Id, t.Name, t.MessageBody }), new JsonSerializerOptions { PropertyNamingPolicy = null });

        return View(result.Data);
    }

    [RequirePermission("Subscriptions.Freeze")]
    [HttpPost("{id}/freeze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Freeze(Guid id, int freezeDays, string? reason, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new FreezeSubscriptionCommand(id, freezeDays, reason), cancellationToken);
        if (result.IsFailure)
            TempData["Error"] = result.Message;
        else
            TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [RequirePermission("Subscriptions.Unfreeze")]
    [HttpPost("{id}/unfreeze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unfreeze(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UnfreezeSubscriptionCommand(id), cancellationToken);
        if (result.IsFailure)
            TempData["Error"] = result.Message;
        else
            TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [RequirePermission("Subscriptions.Create")]
    [HttpPost("{id}/renew")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Renew(Guid id, RenewSubscriptionCommand command, CancellationToken cancellationToken)
    {
        command = command with { PreviousSubscriptionId = id };
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id = result.Data });
    }

    [RequirePermission("Subscriptions.PaymentHistory")]
    [HttpPost("{id}/payment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPayment(Guid id, RecordSubscriptionPaymentCommand command, CancellationToken cancellationToken)
    {
        command = command with { SubscriptionId = id };
        if (!ModelState.IsValid)
        {
            TempData["Error"] = _localizer["Invalid payment data"].Value;
            return RedirectToAction(nameof(Details), new { id });
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            TempData["Error"] = result.Message;
        else
            TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task PopulateDetailsDropdowns(CancellationToken cancellationToken)
    {
        var plans = await _planRepo.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => new { p.Id, p.Name })
            .ToListAsync(cancellationToken);
        ViewBag.Plans = new SelectList(plans, "Id", "Name");

        var offers = await _offerRepo.Query()
            .Where(o => o.IsActive && o.StartDate <= DateTime.UtcNow && o.EndDate >= DateTime.UtcNow)
            .OrderBy(o => o.OfferTitle)
            .Select(o => new { o.Id, o.OfferTitle })
            .ToListAsync(cancellationToken);
        ViewBag.Offers = new SelectList(offers, "Id", "OfferTitle");

        var paymentMethods = Enum.GetValues<PaymentMethod>()
            .Select(pm => new { Value = pm.ToString(), Text = _localizer[pm.ToString()] })
            .ToList();
        ViewBag.PaymentMethods = new SelectList(paymentMethods, "Value", "Text");
    }

    private async Task PopulateDropdowns(CancellationToken cancellationToken)
    {
        var members = await _memberRepo.Query()
            .Where(m => !m.IsDeleted)
            .OrderBy(m => m.FullName)
            .Select(m => new { m.Id, m.FullName })
            .ToListAsync(cancellationToken);
        ViewBag.Members = new SelectList(members, "Id", "FullName");

        var plans = await _planRepo.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Price, p.FreezeDays })
            .ToListAsync(cancellationToken);
        ViewBag.Plans = new SelectList(plans, "Id", "Name");

        var offers = await _offerRepo.Query()
            .Where(o => o.IsActive && o.StartDate <= DateTime.UtcNow && o.EndDate >= DateTime.UtcNow)
            .OrderBy(o => o.OfferTitle)
            .Select(o => new { o.Id, o.OfferTitle })
            .ToListAsync(cancellationToken);
        ViewBag.Offers = new SelectList(offers, "Id", "OfferTitle");

        var paymentMethods = Enum.GetValues<PaymentMethod>()
            .Select(pm => new { Value = pm.ToString(), Text = _localizer[pm.ToString()] })
            .ToList();
        ViewBag.PaymentMethods = new SelectList(paymentMethods, "Value", "Text");
    }
}
