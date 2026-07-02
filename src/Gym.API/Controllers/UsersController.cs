using Gym.API.Filters;
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
using Microsoft.Extensions.Localization;
using Gym.API.Resources;

namespace Gym.API.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UsersController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Users.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(ApiResponse<PaginatedResult<UserListItemDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Users.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse<UserListItemDto>.Ok(result.Data!));
    }

    [HttpGet("roles")]
    [RequirePermission("Users.View")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        return Ok(ApiResponse<List<RoleDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Users.Create")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse<Guid>.Ok(result.Data!, _localizer["User created successfully"]));
    }

    [HttpPut("{id}")]
    [RequirePermission("Users.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.Fail(_localizer["Route ID and body ID do not match"]));

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    [RequirePermission("Users.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message!));

        return Ok(ApiResponse.Ok(result.Message));
    }
}
