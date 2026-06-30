using Gym.Application.Attendances.DTOs;
using Gym.Application.Attendances.Queries.GetAllAttendances;
using Gym.Application.Attendances.Queries.GetAttendanceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Attendance")]
public class AttendancesMvcController : Controller
{
    private readonly IMediator _mediator;

    public AttendancesMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Attendance";

        var query = new GetAllAttendancesQuery { Page = page, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        return View(result.Data);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Attendance Details";

        var result = await _mediator.Send(new GetAttendanceByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }
}
