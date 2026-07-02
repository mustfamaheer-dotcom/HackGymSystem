using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize]
public class RolesController : BaseController
{
    private readonly IRolePermissionService _roleService;

    public RolesController(IRolePermissionService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAllRolesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse<List<RoleListDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse<RoleDetailDto>.Ok(result.Data!));
    }

    [HttpGet("permissions")]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAllPermissionsGroupedAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse<List<PermissionGroupDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Roles.Create")]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto, CancellationToken cancellationToken)
    {
        var result = await _roleService.CreateRoleAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    [RequirePermission("Roles.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(ApiResponse.Fail("Route ID and body ID do not match."));
        var result = await _roleService.UpdateRoleAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPut("{id}/permissions")]
    [RequirePermission("Roles.Edit")]
    public async Task<IActionResult> UpdatePermissions(Guid id, [FromBody] List<Guid> permissionIds, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRolePermissionsAsync(id, permissionIds, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    [RequirePermission("Roles.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));
        return Ok(ApiResponse.Ok(result.Message));
    }
}
