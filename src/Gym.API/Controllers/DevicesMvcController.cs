using Gym.Application.Devices.DTOs;
using Gym.Application.Devices.Commands.CreateDevice;
using Gym.Application.Devices.Commands.UpdateDevice;
using Gym.Application.Devices.Commands.DeleteDevice;
using Gym.Application.Devices.Queries.GetAllDevices;
using Gym.Application.Devices.Queries.GetDeviceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("Devices")]
public class DevicesMvcController : Controller
{
    private readonly IMediator _mediator;

    public DevicesMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Devices";
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

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Device";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDeviceCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Device";
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = "Device created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Device";
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

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateDeviceCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Edit Device";
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
        TempData["Success"] = "Device updated successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Device Details";
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
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
        ViewData["Title"] = "Delete Device";
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
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
        var result = await _mediator.Send(new DeleteDeviceCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = "Device deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
