using Gym.API.Controllers;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Users.Commands.CreateUser;
using Gym.Application.Users.Commands.DeleteUser;
using Gym.Application.Users.Commands.UpdateUser;
using Gym.Application.Users.DTOs;
using Gym.Application.Users.Queries.GetAllUsers;
using Gym.Application.Users.Queries.GetRoles;
using Gym.Application.Users.Queries.GetUserById;
using Gym.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(ApiResponse<PaginatedResult<UserListItemDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse<UserListItemDto>.Ok(result.Data!));
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        return Ok(ApiResponse<List<RoleDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!, "User created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.Fail("Route ID and body ID do not match"));

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }
}
