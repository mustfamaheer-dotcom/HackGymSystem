using Gym.Application.Memberships.DTOs;
using Gym.Application.Memberships.Queries.GetAllMemberships;
using Gym.Application.Memberships.Queries.GetMembershipById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
[Route("Memberships")]
public class MembershipsMvcController : Controller
{
    private readonly IMediator _mediator;

    public MembershipsMvcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "Memberships";
        var query = new GetAllMembershipsQuery { Page = page, PageSize = pageSize };
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
        ViewData["Title"] = "Membership Details";
        var result = await _mediator.Send(new GetMembershipByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }
}
