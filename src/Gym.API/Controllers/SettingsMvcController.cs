using Gym.API.Filters;
using Gym.Application.Settings.DTOs;
using Gym.Application.Settings.Commands.CreateSetting;
using Gym.Application.Settings.Commands.UpdateSetting;
using Gym.Application.Settings.Queries.GetAllSettings;
using Gym.Application.Settings.Queries.GetSettingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Settings")]
public class SettingsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public SettingsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Settings"];
        var result = await _mediator.Send(new GetAllSettingsQuery(), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(new List<SettingDto>());
        }
        var groups = result.Data!.Select(s => s.Group).Distinct().OrderBy(g => g).ToList();
        ViewBag.Groups = groups;
        return View(result.Data);
    }

    [HttpGet("create")]
    [RequirePermission("Settings.Edit")]
    public IActionResult Create()
    {
        ViewData["Title"] = _localizer["New Setting"];
        return View();
    }

    [HttpPost("create")]
    [RequirePermission("Settings.Edit")]
    public async Task<IActionResult> Create(CreateSettingCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Setting"];
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Setting created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    [RequirePermission("Settings.Edit")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Setting"];
        var result = await _mediator.Send(new GetSettingByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var command = new UpdateSettingCommand(dto.Id, dto.Value);
        ViewBag.SettingKey = dto.Key;
        ViewBag.SettingGroup = dto.Group;
        return View(command);
    }

    [HttpPost("edit/{id}")]
    [RequirePermission("Settings.Edit")]
    public async Task<IActionResult> Edit(Guid id, UpdateSettingCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Setting"];
        if (id != command.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"].Value;
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
        TempData["Success"] = _localizer["Setting updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }
}
