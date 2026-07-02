using Gym.Application.Devices.DTOs;
using Gym.Application.Devices.Commands.CreateDevice;
using Gym.Application.Devices.Commands.UpdateDevice;
using Gym.Application.Devices.Commands.DeleteDevice;
using Gym.Application.Devices.Queries.GetAllDevices;
using Gym.Application.Devices.Queries.GetDeviceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Filters;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Devices")]
public class DevicesMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DevicesMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [RequirePermission("Devices.View")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Devices"];
        var query = new GetAllDevicesQuery { Page = page, PageSize = pageSize, SearchTerm = searchTerm };
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }
        ViewBag.SearchTerm = searchTerm;
        return View(result.Data);
    }

    [RequirePermission("Devices.Manage")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = _localizer["New Device"];
        return View();
    }

    [RequirePermission("Devices.Manage")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDeviceCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Device"];
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Device created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Devices.Manage")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Device"];
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        var dto = result.Data!;
        var command = new UpdateDeviceCommand(dto.Id, dto.Name, dto.IPAddress, dto.Port, dto.Model, dto.SerialNumber);
        return View(command);
    }

    [RequirePermission("Devices.Manage")]
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateDeviceCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Device"];
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
        TempData["Success"] = _localizer["Device updated successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Devices.View")]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Device Details"];
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [RequirePermission("Devices.Manage")]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Device"];
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [RequirePermission("Devices.Manage")]
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDeviceCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = _localizer["Device deleted successfully"].Value;
        return RedirectToAction(nameof(Index));
    }
}
