using FluentValidation;
using Gym.Application.Common.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<Result<AuthResponse>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty();
        RuleFor(v => v.Password).NotEmpty();
    }
}
