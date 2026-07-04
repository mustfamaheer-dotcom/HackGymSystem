using System.Security.Cryptography;
using System.Text;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Gym.Application;

namespace Gym.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IStringLocalizer<ApplicationResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _localizer = localizer;
    }

    public async Task<Result<AuthResponse>> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Repository<User>()
            .Query()
            .Include(u => u.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return Result<AuthResponse>.Failure(_localizer["Invalid username or password."]);

        user.RecordLogin();
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var permissions = user.Role.RolePermissions?
            .Select(rp => rp.Permission?.Name ?? string.Empty)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList() ?? new List<string>();

        var (accessToken, refreshToken, expiresAt) = await _tokenService.GenerateTokensAsync(user, permissions);

        user.UpdateRefreshToken(refreshToken, expiresAt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role?.Name ?? _localizer["User"],
                RoleId = user.RoleId,
                Permissions = permissions
            }
        });
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Repository<User>()
            .Query()
            .Include(u => u.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.IsActive, cancellationToken);

        if (user is null)
        {
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
            var replayUser = await _unitOfWork.Repository<User>()
                .Query()
                .FirstOrDefaultAsync(u => u.PreviousRefreshTokenHash == tokenHash && u.IsActive, cancellationToken);

            if (replayUser != null)
            {
                replayUser.UpdateRefreshToken(null, null);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<AuthResponse>.Failure(_localizer["Refresh token has been compromised. Please login again."]);
            }

            return Result<AuthResponse>.Failure(_localizer["Invalid or expired refresh token."]);
        }

        if (user.RefreshTokenExpiry <= DateTime.UtcNow)
            return Result<AuthResponse>.Failure(_localizer["Invalid or expired refresh token."]);

        var permissions = user.Role.RolePermissions?
            .Select(rp => rp.Permission?.Name ?? string.Empty)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList() ?? new List<string>();

        var (accessToken, newRefreshToken, expiresAt) = await _tokenService.GenerateTokensAsync(user, permissions);
        user.UpdateRefreshToken(newRefreshToken, expiresAt);
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role?.Name ?? _localizer["User"],
                RoleId = user.RoleId,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired,
                Permissions = permissions
            }
        });
    }

    public async Task<Result> LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure(_localizer["User not found."]);

        user.UpdateRefreshToken(null, null);
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["Logged out successfully."]);
    }

    public async Task<Result<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Repository<User>()
            .Query()
            .Include(u => u.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result<UserDto>.Failure(_localizer["User not found."]);

        var permissions = user.Role.RolePermissions?
            .Select(rp => rp.Permission?.Name ?? string.Empty)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList() ?? new List<string>();

        return Result<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role?.Name ?? _localizer["User"],
            RoleId = user.RoleId,
            IsPasswordChangeRequired = user.IsPasswordChangeRequired,
            Permissions = permissions
        });
    }
}
