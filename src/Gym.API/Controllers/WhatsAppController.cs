using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
[Route("api/whatsapp")]
[ApiController]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public WhatsAppController(IWhatsAppService whatsAppService, IStringLocalizer<SharedResources> localizer)
    {
        _whatsAppService = whatsAppService;
        _localizer = localizer;
    }

    [HttpPost("send")]
    [RequirePermission("WhatsApp.Send")]
    public async Task<IActionResult> Send([FromBody] SendWhatsAppRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return BadRequest(ApiResponse.Fail(_localizer["Phone number is required"]));

        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(ApiResponse.Fail(_localizer["Message is required"]));

        var result = await _whatsAppService.SendAsync(request.PhoneNumber, request.Message, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(_localizer["Message sent successfully"]));
    }

    [HttpPost("send-member")]
    [RequirePermission("WhatsApp.Send")]
    public async Task<IActionResult> SendMemberMessage([FromBody] MemberWhatsAppData data, [FromQuery] string templateBody, [FromQuery] string language = "ar", CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(data.MemberPhone))
            return BadRequest(ApiResponse.Fail(_localizer["Phone number is required"]));

        if (string.IsNullOrWhiteSpace(templateBody))
            return BadRequest(ApiResponse.Fail(_localizer["Template body is required"]));

        var result = await _whatsAppService.SendMemberAsync(data, templateBody, language, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(_localizer["Message sent successfully"]));
    }
}

public class SendWhatsAppRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}