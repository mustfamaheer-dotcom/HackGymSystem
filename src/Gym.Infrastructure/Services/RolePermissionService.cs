using System.Text.Json;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Gym.Application;

namespace Gym.Infrastructure.Services;

public class RolePermissionService : IRolePermissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public RolePermissionService(IUnitOfWork unitOfWork, ICurrentUserService currentUser, IHttpContextAccessor httpContextAccessor, IStringLocalizer<ApplicationResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
    }

    public async Task<Result<List<RoleListDto>>> GetAllRolesAsync(CancellationToken ct = default)
    {
        var roles = await _unitOfWork.Repository<Role>()
            .Query()
            .Include(r => r.Users)
            .Include(r => r.RolePermissions)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

        var dtos = roles.Select(r => new RoleListDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive,
            IsSystem = r.IsSystem,
            UserCount = r.Users.Count,
            PermissionCount = r.RolePermissions.Count,
            CreatedAt = r.CreatedAt
        }).ToList();

        return Result<List<RoleListDto>>.Success(dtos);
    }

    public async Task<Result<RoleDetailDto>> GetRoleByIdAsync(Guid id, CancellationToken ct = default)
    {
        var role = await _unitOfWork.Repository<Role>()
            .Query()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role is null)
            return Result<RoleDetailDto>.Failure(_localizer["Role not found."]);

        return Result<RoleDetailDto>.Success(new RoleDetailDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            IsSystem = role.IsSystem,
            PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList(),
            CreatedAt = role.CreatedAt
        });
    }

    public async Task<Result<Guid>> CreateRoleAsync(CreateRoleDto dto, CancellationToken ct = default)
    {
        var exists = await _unitOfWork.Repository<Role>()
            .Query()
            .AnyAsync(r => r.Name == dto.Name, ct);

        if (exists)
            return Result<Guid>.Failure(_localizer["A role with this name already exists."]);

        var role = new Role(dto.Name, dto.Description) { IsActive = dto.IsActive };
        await _unitOfWork.Repository<Role>().AddAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        if (dto.PermissionIds.Count > 0)
        {
            foreach (var permId in dto.PermissionIds)
            {
                await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission(role.Id, permId), ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);
        }

        await LogPermissionChangeAsync(_localizer["Create"], role.Name, null, dto.PermissionIds, ct);

        return Result<Guid>.Success(role.Id);
    }

    public async Task<Result> UpdateRoleAsync(UpdateRoleDto dto, CancellationToken ct = default)
    {
        var role = await _unitOfWork.Repository<Role>()
            .Query()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == dto.Id, ct);

        if (role is null)
            return Result.Failure(_localizer["Role not found."]);

        var duplicate = await _unitOfWork.Repository<Role>()
            .Query()
            .AnyAsync(r => r.Name == dto.Name && r.Id != dto.Id, ct);

        if (duplicate)
            return Result.Failure(_localizer["A role with this name already exists."]);

        var oldPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList();

        role.Name = dto.Name;
        role.Description = dto.Description;
        role.IsActive = dto.IsActive;
        role.MarkUpdated();

        var existing = role.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();
        var requested = dto.PermissionIds.ToHashSet();

        foreach (var rp in role.RolePermissions.Where(rp => !requested.Contains(rp.PermissionId)).ToList())
            _unitOfWork.Repository<RolePermission>().Delete(rp);

        foreach (var permId in requested.Where(p => !existing.Contains(p)))
            await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission(role.Id, permId), ct);

        _unitOfWork.Repository<Role>().Update(role);
        await _unitOfWork.SaveChangesAsync(ct);

        await LogPermissionChangeAsync(_localizer["Update"], role.Name, oldPermissionIds, dto.PermissionIds, ct);

        return Result.Success(_localizer["Role updated successfully."]);
    }

    public async Task<Result> DeleteRoleAsync(Guid id, CancellationToken ct = default)
    {
        var role = await _unitOfWork.Repository<Role>()
            .Query()
            .Include(r => r.Users)
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role is null)
            return Result.Failure(_localizer["Role not found."]);

        if (role.IsSystem)
            return Result.Failure(_localizer["System roles cannot be deleted."]);

        if (role.Users.Count > 0)
            return Result.Failure(_localizer["Cannot delete a role that is assigned to users."]);

        var adminCount = await _unitOfWork.Repository<User>()
            .Query()
            .CountAsync(u => u.RoleId == id && u.IsActive, ct);

        var totalAdmins = await _unitOfWork.Repository<User>()
            .Query()
            .CountAsync(u => u.IsActive, ct);

        if (adminCount > 0 && totalAdmins <= 1)
            return Result.Failure(_localizer["Cannot remove the last admin role."]);

        foreach (var rp in role.RolePermissions.ToList())
            _unitOfWork.Repository<RolePermission>().Delete(rp);

        _unitOfWork.Repository<Role>().Delete(role);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(_localizer["Role deleted successfully."]);
    }

    public async Task<Result<List<PermissionGroupDto>>> GetAllPermissionsGroupedAsync(CancellationToken ct = default)
    {
        var permissions = await _unitOfWork.Repository<Permission>()
            .Query()
            .OrderBy(p => p.Module).ThenBy(p => p.Name)
            .ToListAsync(ct);

        var groups = permissions
            .GroupBy(p => string.IsNullOrEmpty(p.Module) ? _localizer["General"] : p.Module)
            .Select(g => new PermissionGroupDto
            {
                Module = g.Key,
                Permissions = g.Select(p => new PermissionItemDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Code = p.Name
                }).ToList()
            }).ToList();

        return Result<List<PermissionGroupDto>>.Success(groups);
    }

    public async Task<Result> UpdateRolePermissionsAsync(Guid roleId, List<Guid> permissionIds, CancellationToken ct = default)
    {
        var role = await _unitOfWork.Repository<Role>()
            .Query()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == roleId, ct);

        if (role is null)
            return Result.Failure(_localizer["Role not found."]);

        var oldPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList();
        var existing = oldPermissionIds.ToHashSet();
        var requested = permissionIds.ToHashSet();

        foreach (var rp in role.RolePermissions.Where(rp => !requested.Contains(rp.PermissionId)).ToList())
            _unitOfWork.Repository<RolePermission>().Delete(rp);

        foreach (var permId in requested.Where(p => !existing.Contains(p)))
            await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission(role.Id, permId), ct);

        await _unitOfWork.SaveChangesAsync(ct);

        await LogPermissionChangeAsync(_localizer["UpdatePermissions"], role.Name, oldPermissionIds, permissionIds, ct);

        return Result.Success(_localizer["Role permissions updated successfully."]);
    }

    private string GetClientIp()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded) == true)
                return forwarded.FirstOrDefault() ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? _localizer["unknown"];
            return httpContext?.Connection.RemoteIpAddress?.ToString() ?? _localizer["unknown"];
        }
        catch { return _localizer["unknown"]; }
    }

    private async Task LogPermissionChangeAsync(string action, string roleName, List<Guid>? oldPermIds, List<Guid>? newPermIds, CancellationToken ct)
    {
        try
        {
            var oldPerms = oldPermIds?.Count > 0
                ? JsonSerializer.Serialize(oldPermIds)
                : null;
            var newPerms = newPermIds?.Count > 0
                ? JsonSerializer.Serialize(newPermIds)
                : null;

            var log = new PermissionAuditLog(
                _currentUser.UserId,
                action,
                roleName,
                oldPerms,
                newPerms,
                GetClientIp());

            await _unitOfWork.Repository<PermissionAuditLog>().AddAsync(log, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch
        {
            // Don't throw if audit logging fails
        }
    }
}
