using Gym.API.Resources;
using Gym.Application.WhatsAppTemplates.Commands.CreateTemplate;
using Gym.Application.WhatsAppTemplates.Commands.UpdateTemplate;
using Gym.Application.WhatsAppTemplates.Queries.GetAllTemplates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Filters;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("WhatsAppTemplates")]
public class WhatsAppTemplatesMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public WhatsAppTemplatesMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [RequirePermission("WhatsApp.Send")]
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "WhatsApp Templates";
        var result = await _mediator.Send(new GetAllTemplatesQuery(), cancellationToken);
        return View(result.IsSuccess ? result.Data : new());
    }

    [RequirePermission("WhatsApp.Broadcast")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Template";
        return View();
    }

    [RequirePermission("WhatsApp.Broadcast")]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTemplateCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("WhatsApp.Broadcast")]
    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Template";
        var result = await _mediator.Send(new GetAllTemplatesQuery(), cancellationToken);
        var template = result.Data?.FirstOrDefault(t => t.Id == id);
        if (template == null)
        {
            TempData["Error"] = _localizer["Template not found"];
            return RedirectToAction(nameof(Index));
        }
        return View(template);
    }

    [RequirePermission("WhatsApp.Broadcast")]
    [HttpPost("{id}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateTemplateCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
