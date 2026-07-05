using Gym.API;
using Gym.Application.Common.Interfaces;
using Gym.API.Controllers;
using Gym.Application.Common.DTOs;
using Gym.Application.Users.Commands.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Gym.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IMediator _mediator;
    private readonly ICaptchaService _captchaService;

    public AuthController(IAuthService authService, IStringLocalizer<SharedResources> localizer,
        IMediator mediator, ICaptchaService captchaService)
    {
        _authService = authService;
        _localizer = localizer;
        _mediator = mediator;
        _captchaService = captchaService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var captchaResult = await _captchaService.ValidateTokenAsync(request.CaptchaToken, cancellationToken);
        if (captchaResult.IsFailure)
            return Unauthorized(ApiResponse.Fail(_localizer["CAPTCHA verification failed. Please try again."]));

        var result = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? _localizer["Login failed"]));

        var response = result.Data!;

        var accessTokenMaxAge = response.ExpiresAt - DateTime.UtcNow;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = accessTokenMaxAge
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

        var accessTokenMaxAge = response.ExpiresAt - DateTime.UtcNow;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            MaxAge = accessTokenMaxAge
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
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

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
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public string CaptchaToken { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;
}
