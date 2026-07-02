using Gym.API.Filters;
using Gym.API.Resources;
using Gym.Application.Common.DTOs;
using Gym.Application.Members.Commands.CreateMember;
using Gym.Application.Members.Commands.DeleteMember;
using Gym.Application.Members.Commands.RestoreMember;
using Gym.Application.Members.Commands.UpdateMember;
using Gym.Application.Members.DTOs;
using Gym.Application.Members.Queries.GetAllMembers;
using Gym.Application.Members.Queries.GetExpiringMembers;
using Gym.Application.Members.Queries.GetMemberById;
using Gym.Application.Members.Queries.GetMembersWithOutstandingBalance;
using Gym.Application.Members.Queries.SearchMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Gym.API.Controllers;

[Authorize]
public class MembersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public MembersController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Members.View")]
    public async Task<IActionResult> GetAllMembers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllMembersQuery(page, pageSize, searchTerm, sortBy, sortDescending);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve members"]));

        return Ok(ApiResponse<PaginatedResult<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Members.View")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMemberByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? _localizer["Member not found"]));

        return Ok(ApiResponse<MemberDto>.Ok(result.Data!));
    }

    [HttpGet("search")]
    [RequirePermission("Members.View")]
    public async Task<IActionResult> SearchMembers([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        var query = new SearchMembersQuery(searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Search failed"]));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("expiring")]
    [RequirePermission("Members.View")]
    public async Task<IActionResult> GetExpiringMembers(
        [FromQuery] int withinDays = 7,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpiringMembersQuery(withinDays);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve expiring members"]));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("outstanding-balance")]
    [RequirePermission("Subscriptions.PaymentHistory")]
    public async Task<IActionResult> GetMembersWithOutstandingBalance(CancellationToken cancellationToken)
    {
        var query = new GetMembersWithOutstandingBalanceQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve members with outstanding balance"]));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Members.Create")]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to create member"]));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    [RequirePermission("Members.Edit")]
    public async Task<IActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.Fail(_localizer["Route ID and body ID do not match"]));

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? _localizer["Failed to update member"]));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    [RequirePermission("Members.Delete")]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMemberCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? _localizer["Failed to delete member"]));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPatch("{id}/restore")]
    [RequirePermission("Members.Delete")]
    public async Task<IActionResult> RestoreMember(Guid id, CancellationToken cancellationToken)
    {
        var command = new RestoreMemberCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? _localizer["Failed to restore member"]));

        return Ok(ApiResponse.Ok(result.Message));
    }
}
