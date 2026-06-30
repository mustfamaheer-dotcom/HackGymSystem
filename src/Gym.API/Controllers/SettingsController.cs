using Gym.Application.Settings.Commands.CreateSetting;
using Gym.Application.Settings.Commands.DeleteSetting;
using Gym.Application.Settings.Commands.UpdateSetting;
using Gym.Application.Settings.DTOs;
using Gym.Application.Settings.Queries.GetAllSettings;
using Gym.Application.Settings.Queries.GetSettingById;
using Gym.Application.Settings.Queries.GetSettingByKey;
using Gym.Application.Settings.Queries.GetSettingsByGroup;
using Gym.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
public class SettingsController : BaseController
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSettings(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSettingsQuery(), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<List<SettingDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<SettingDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSettingById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettingByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<SettingDto>.Fail(result.Message!));

        return Ok(ApiResponse<SettingDto>.Ok(result.Data!));
    }

    [HttpGet("by-key/{key}")]
    public async Task<IActionResult> GetSettingByKey(string key, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettingByKeyQuery(key), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<SettingDto>.Fail(result.Message!));

        return Ok(ApiResponse<SettingDto>.Ok(result.Data!));
    }

    [HttpGet("by-group/{group}")]
    public async Task<IActionResult> GetSettingsByGroup(string group, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettingsByGroupQuery(group), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<List<SettingDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<SettingDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSetting([FromBody] CreateSettingCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSetting(Guid id, [FromBody] UpdateSettingBody body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateSettingCommand(id, body.Value), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSetting(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSettingCommand(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok());
    }
}

public record UpdateSettingBody(string Value);
