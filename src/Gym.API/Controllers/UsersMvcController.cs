using Gym.Application.Users.DTOs;
using Gym.Application.Users.Commands.CreateUser;
using Gym.Application.Users.Commands.UpdateUser;
using Gym.Application.Users.Commands.DeleteUser;
using Gym.Application.Users.Queries.GetAllUsers;
using Gym.Application.Users.Queries.GetUserById;
using Gym.Application.Users.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.API.Filters;
using Gym.API.Resources;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Users")]
public class UsersMvcController : Controller
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UsersMvcController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [RequirePermission("Users.View")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = _localizer["Users"];

        var query = new GetAllUsersQuery { Page = page, PageSize = pageSize, SearchTerm = searchTerm };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View("Index", null);
        }

        ViewBag.SearchTerm = searchTerm;

        return View(result.Data);
    }

    [RequirePermission("Users.Create")]
    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New User"];

        var rolesResult = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        ViewBag.Roles = rolesResult.Data ?? [];

        return View();
    }

    [RequirePermission("Users.Create")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New User"];

        var rolesResult = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        ViewBag.Roles = rolesResult.Data ?? [];

        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(command);
        }

        TempData["Success"] = _localizer["User created successfully"];
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Users.Edit")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit User"];

        var rolesResult = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        ViewBag.Roles = rolesResult.Data ?? [];

        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var user = result.Data!;
        var command = new UpdateUserCommand(
            user.Id, user.FullName, user.Email, user.Phone,
            Guid.Parse(user.RoleId), user.IsActive);

        return View(command);
    }

    [RequirePermission("Users.Edit")]
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateUserCommand command, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit User"];

        var rolesResult = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        ViewBag.Roles = rolesResult.Data ?? [];

        if (id != command.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"];
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

        TempData["Success"] = _localizer["User updated successfully"];
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Users.View")]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["User Details"];

        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [RequirePermission("Users.Delete")]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete User"];

        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [RequirePermission("Users.Delete")]
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = _localizer["User deleted successfully"];
        return RedirectToAction(nameof(Index));
    }
}
