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

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner,Receptionist")]
public class MembersController : BaseController
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
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
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve members"));

        return Ok(ApiResponse<PaginatedResult<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMemberByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Member not found"));

        return Ok(ApiResponse<MemberDto>.Ok(result.Data!));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMembers([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        var query = new SearchMembersQuery(searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Search failed"));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiringMembers(
        [FromQuery] int withinDays = 7,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExpiringMembersQuery(withinDays);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve expiring members"));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpGet("outstanding-balance")]
    public async Task<IActionResult> GetMembersWithOutstandingBalance(CancellationToken cancellationToken)
    {
        var query = new GetMembersWithOutstandingBalanceQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve members with outstanding balance"));

        return Ok(ApiResponse<List<MemberDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create member"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.Fail("Route ID and body ID do not match"));

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to update member"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMemberCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to delete member"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> RestoreMember(Guid id, CancellationToken cancellationToken)
    {
        var command = new RestoreMemberCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Failed to restore member"));

        return Ok(ApiResponse.Ok(result.Message));
    }
}
