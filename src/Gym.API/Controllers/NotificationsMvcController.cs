using Gym.Application.Notifications.DTOs;
using Gym.Application.Notifications.Commands.CreateNotification;
using Gym.Application.Notifications.Queries.GetAllNotifications;
using Gym.Application.Notifications.Queries.GetNotificationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Notifications")]
public class NotificationsMvcController : Controller
{
    private readonly IMediator _mediator;

    public NotificationsMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Notifications";
        var query = new GetAllNotificationsQuery { Page = page, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }
        return View(result.Data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "New Notification";
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "New Notification";
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = "Notification created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Notification Details";
        var result = await _mediator.Send(new GetNotificationByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }
}
