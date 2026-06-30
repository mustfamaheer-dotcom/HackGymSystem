using Gym.Application.Common.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<Result<UserDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthService _authService;

    public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IAuthService authService)
    {
        _currentUserService = currentUserService;
        _authService = authService;
    }

    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            return Result<UserDto>.Failure("User not authenticated");

        return await _authService.GetCurrentUserAsync(userId.Value, cancellationToken);
    }
}
