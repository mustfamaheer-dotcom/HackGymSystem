using Gym.Application.Settings.DTOs;
using Gym.Application.Settings.Commands.CreateSetting;
using Gym.Application.Settings.Commands.UpdateSetting;
using Gym.Application.Settings.Queries.GetAllSettings;
using Gym.Application.Settings.Queries.GetSettingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Settings")]
public class SettingsMvcController : Controller
{
    private readonly IMediator _mediator;

    public SettingsMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Settings";
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
    public IActionResult Create()
    {
        ViewData["Title"] = "New Setting";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateSettingCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Setting";
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = "Setting created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Setting";
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
    public async Task<IActionResult> Edit(Guid id, UpdateSettingCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Setting";
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
        TempData["Success"] = "Setting updated successfully";
        return RedirectToAction(nameof(Index));
    }
}
