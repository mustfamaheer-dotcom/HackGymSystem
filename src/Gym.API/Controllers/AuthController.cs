using Gym.API;
using Gym.Application.Common.Interfaces;
using Gym.API.Controllers;
using Gym.Application.Common.DTOs;
using Gym.Application.Users.Commands.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IMediator _mediator;

    public AuthController(IAuthService authService, IStringLocalizer<SharedResources> localizer, IMediator mediator)
    {
        _authService = authService;
        _localizer = localizer;
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? _localizer["Login failed"]));

        var response = result.Data!;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = TimeSpan.FromDays(7)
        });

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = TimeSpan.FromDays(30)
        });

        return Ok(ApiResponse<object>.Ok(new
        {
            response.AccessToken,
            response.ExpiresAt,
            response.User
        }));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(ApiResponse.Fail(_localizer["Token refresh failed"]));

        var result = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? _localizer["Token refresh failed"]));

        var response = result.Data!;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = TimeSpan.FromDays(7)
        });

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = TimeSpan.FromDays(30)
        });

        return Ok(ApiResponse<object>.Ok(new
        {
            response.AccessToken,
            response.ExpiresAt,
            response.User
        }));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        if (CurrentUserId is null)
            return Unauthorized();

        var result = await _mediator.Send(new ChangePasswordCommand(CurrentUserId.Value, request.CurrentPassword, request.NewPassword), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Password change failed"]));

        return Ok(ApiResponse.Ok(_localizer["Password changed successfully"]));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null)
            return Unauthorized();

        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        var result = await _authService.LogoutAsync(CurrentUserId.Value, cancellationToken);
        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null)
            return Unauthorized();

        var result = await _authService.GetCurrentUserAsync(CurrentUserId.Value, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? _localizer["User not found"]));

        return Ok(ApiResponse<UserDto>.Ok(result.Data!));
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
