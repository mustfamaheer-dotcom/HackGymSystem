using Gym.Application.Common.Interfaces;
using Gym.API.Controllers;
using Gym.Application.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? "Login failed"));

        var response = result.Data!;

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            MaxAge = TimeSpan.FromDays(7)
        });

        return Ok(ApiResponse<AuthResponse>.Ok(response));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (result.IsFailure)
            return Unauthorized(ApiResponse.Fail(result.Message ?? "Token refresh failed"));

        return Ok(ApiResponse<AuthResponse>.Ok(result.Data!));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (CurrentUserId is null)
            return Unauthorized();

        Response.Cookies.Delete("accessToken");

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
            return Unauthorized(ApiResponse.Fail(result.Message ?? "User not found"));

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
