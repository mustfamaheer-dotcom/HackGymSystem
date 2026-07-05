using Gym.API;
using Gym.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IMediator _mediator;
    private readonly ICaptchaService _captchaService;
    private readonly string _captchaSiteKey;

    public AccountController(IAuthService authService, IStringLocalizer<SharedResources> localizer,
        IMediator mediator, ICaptchaService captchaService, IConfiguration configuration)
    {
        _authService = authService;
        _localizer = localizer;
        _mediator = mediator;
        _captchaService = captchaService;
        _captchaSiteKey = configuration["Captcha:SiteKey"] ?? string.Empty;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "HomeMvc");

        ViewData["Title"] = _localizer["Login"];
        ViewBag.CaptchaSiteKey = _captchaSiteKey;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> Login(string username, string password, string captchaToken, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Login"];
        ViewBag.CaptchaSiteKey = _captchaSiteKey;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = _localizer["Username and password are required"];
            return View();
        }

        var captchaResult = await _captchaService.ValidateTokenAsync(captchaToken, cancellationToken);
        if (captchaResult.IsFailure)
        {
            ViewBag.Error = _localizer["CAPTCHA verification failed. Please try again."];
            return View();
        }

        var result = await _authService.LoginAsync(username, password, cancellationToken);

        if (result.IsFailure)
        {
            ViewBag.Error = result.Message ?? _localizer["Login failed"];
            return View();
        }

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

        if (response.User.IsPasswordChangeRequired)
            return RedirectToAction("ChangePassword", "Account");

        return RedirectToAction("Index", "HomeMvc");
    }

    [HttpGet("change-password")]
    [Authorize]
    public IActionResult ChangePassword()
    {
        ViewData["Title"] = _localizer["Change Password"];
        return View();
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer["Change Password"];

        if (string.IsNullOrWhiteSpace(currentPassword))
        {
            ViewBag.Error = _localizer["Current password is required."];
            return View();
        }

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            ViewBag.Error = _localizer["Password must be at least 6 characters."];
            return View();
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.Error = _localizer["Passwords do not match."];
            return View();
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var result = await _mediator.Send(new Gym.Application.Users.Commands.ChangePassword.ChangePasswordCommand(id, currentPassword, newPassword), cancellationToken);

        if (result.IsFailure)
        {
            ViewBag.Error = result.Message;
            return View();
        }

        TempData["Success"] = _localizer["Password changed successfully."].Value;
        return RedirectToAction("Index", "HomeMvc");
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is not null && Guid.TryParse(userId, out var id))
            await _authService.LogoutAsync(id, cancellationToken);

        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");
        return RedirectToAction("Login");
    }

    [HttpPost]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        if (culture != "ar" && culture != "en")
            culture = "ar";

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
        );

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "HomeMvc");
    }
}
