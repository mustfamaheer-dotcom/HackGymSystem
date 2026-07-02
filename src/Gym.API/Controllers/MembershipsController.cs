using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Memberships.Commands.CancelMembership;
using Gym.Application.Memberships.Commands.CreateMembership;
using Gym.Application.Memberships.Commands.FreezeMembership;
using Gym.Application.Memberships.Commands.RenewMembership;
using Gym.Application.Memberships.Commands.UnfreezeMembership;
using Gym.Application.Memberships.DTOs;
using Gym.Application.Memberships.Queries.GetAllMemberships;
using Gym.Application.Memberships.Queries.GetMemberMemberships;
using Gym.Application.Memberships.Queries.GetMembershipById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize]
public class MembershipsController : BaseController
{
    private readonly IMediator _mediator;

    public MembershipsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [RequirePermission("Memberships.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMembershipsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PaginatedResult<MembershipDto>>.Fail(result.Message!));

        return Ok(ApiResponse<PaginatedResult<MembershipDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Memberships.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMembershipByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<MembershipDetailDto>.Fail(result.Message!));

        return Ok(ApiResponse<MembershipDetailDto>.Ok(result.Data!));
    }

    [HttpGet("by-member/{memberId}")]
    [RequirePermission("Memberships.View")]
    public async Task<IActionResult> GetByMember(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberMembershipsQuery(memberId), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<List<MembershipDto>>.Fail(result.Message!));

        return Ok(ApiResponse<List<MembershipDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Memberships.Manage")]
    public async Task<IActionResult> Create([FromBody] CreateMembershipCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPost("{id}/renew")]
    [RequirePermission("Memberships.Manage")]
    public async Task<IActionResult> Renew(Guid id, [FromBody] RenewMembershipRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RenewMembershipCommand(id, request.NewEndDate, request.AdditionalDays, request.AdditionalVisits), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("{id}/freeze")]
    [RequirePermission("Memberships.Manage")]
    public async Task<IActionResult> Freeze(Guid id, [FromBody] FreezeMembershipRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new FreezeMembershipCommand(id, request.FreezeDays), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("{id}/unfreeze")]
    [RequirePermission("Memberships.Manage")]
    public async Task<IActionResult> Unfreeze(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UnfreezeMembershipCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPost("{id}/cancel")]
    [RequirePermission("Memberships.Manage")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelMembershipCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }
}

public class RenewMembershipRequest
{
    public DateTime NewEndDate { get; set; }
    public int AdditionalDays { get; set; }
    public int? AdditionalVisits { get; set; }
}

public class FreezeMembershipRequest
{
    public int FreezeDays { get; set; }
}
