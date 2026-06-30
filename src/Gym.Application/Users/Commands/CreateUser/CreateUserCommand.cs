using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string Password,
    string FullName,
    string Email,
    string? Phone,
    Guid RoleId) : IRequest<Result<Guid>>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userRepo = _unitOfWork.Repository<User>();
        var roleRepo = _unitOfWork.Repository<Role>();

        var role = await roleRepo.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
            return Result<Guid>.Failure("Role not found");

        var existingUser = await userRepo.Query()
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        if (existingUser is not null)
            return Result<Guid>.Failure("Username already exists");

        var existingEmail = await userRepo.Query()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingEmail is not null)
            return Result<Guid>.Failure("Email already exists");

        var user = new User(
            request.Username,
            BCrypt.Net.BCrypt.HashPassword(request.Password),
            request.FullName,
            request.Email,
            request.Phone,
            request.RoleId);

        await userRepo.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty().MaximumLength(50);
        RuleFor(v => v.Password).NotEmpty().MinimumLength(6);
        RuleFor(v => v.FullName).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(v => v.RoleId).NotEmpty();
    }
}
