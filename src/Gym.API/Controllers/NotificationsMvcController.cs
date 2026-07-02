using Gym.Application.Notifications.DTOs;
using Gym.Application.Notifications.Commands.CreateNotification;
using Gym.Application.Notifications.Queries.GetAllNotifications;
using Gym.Application.Notifications.Queries.GetNotificationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Filters;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Notifications")]
public class NotificationsMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public NotificationsMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [RequirePermission("Notifications.View")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Notifications"];
        var query = new GetAllNotificationsQuery { Page = page, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }
        return View(result.Data);
    }

    [RequirePermission("Notifications.Send")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewData["Title"] = _localizer["New Notification"];
        return View();
    }

    [RequirePermission("Notifications.Send")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Notification"];
        if (!ModelState.IsValid)
            return View(command);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }
        TempData["Success"] = _localizer["Notification created successfully"].Value;
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Notifications.View")]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Notification Details"];
        var result = await _mediator.Send(new GetNotificationByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }
}
