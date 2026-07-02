using Gym.API.Extensions;
using Gym.API.Filters;
using Gym.API.Resources;
using Gym.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("Roles")]
public class RolesMvcController : Controller
{
    private readonly IRolePermissionService _roleService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public RolesMvcController(IRolePermissionService roleService, IStringLocalizer<SharedResources> localizer)
    {
        _roleService = roleService;
        _localizer = localizer;
    }

    [RequirePermission("Roles.View")]
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Roles & Permissions"];
        var result = await _roleService.GetAllRolesAsync(cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(new List<RoleListDto>());
        }
        return View(result.Data);
    }

    [RequirePermission("Roles.Create")]
    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Role"];
        var permsResult = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        ViewBag.PermissionGroups = permsResult.Data ?? new List<PermissionGroupDto>();
        return View();
    }

    [RequirePermission("Roles.Create")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateRoleDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["New Role"];
        var permsResult = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        ViewBag.PermissionGroups = permsResult.Data ?? new List<PermissionGroupDto>();

        if (!ModelState.IsValid)
            return View(dto);

        var result = await _roleService.CreateRoleAsync(dto, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = _localizer["Role created successfully"];
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Roles.Edit")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Role"];
        var roleResult = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        if (roleResult.IsFailure)
        {
            TempData["Error"] = roleResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var permsResult = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        ViewBag.PermissionGroups = permsResult.Data ?? new List<PermissionGroupDto>();

        var role = roleResult.Data!;
        var dto = new UpdateRoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            PermissionIds = role.PermissionIds
        };

        return View(dto);
    }

    [RequirePermission("Roles.Edit")]
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Edit Role"];
        if (id != dto.Id)
        {
            TempData["Error"] = _localizer["Route ID and form ID do not match"];
            return View(dto);
        }

        var permsResult = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        ViewBag.PermissionGroups = permsResult.Data ?? new List<PermissionGroupDto>();

        if (!ModelState.IsValid)
            return View(dto);

        var result = await _roleService.UpdateRoleAsync(dto, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = _localizer["Role updated successfully"];
        return RedirectToAction(nameof(Index));
    }

    [RequirePermission("Roles.View")]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Role Details"];
        var result = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var permsResult = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        ViewBag.PermissionGroups = permsResult.Data ?? new List<PermissionGroupDto>();

        return View(result.Data);
    }

    [RequirePermission("Roles.Delete")]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Delete Role"];
        var result = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [RequirePermission("Roles.Delete")]
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = _localizer["Role deleted successfully"];
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        ViewData["Title"] = _localizer["Access Denied"];
        return View();
    }
}
