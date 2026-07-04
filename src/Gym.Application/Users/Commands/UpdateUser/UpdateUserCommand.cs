using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    Guid RoleId,
    bool IsActive) : IRequest<Result>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userRepo = _unitOfWork.Repository<User>();
        var roleRepo = _unitOfWork.Repository<Role>();

        var user = await userRepo.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return Result.Failure(_localizer["User not found"]);

        var role = await roleRepo.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
            return Result.Failure(_localizer["Role not found"]);

        var emailExists = await userRepo.Query()
            .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);
        if (emailExists)
            return Result.Failure(_localizer["Email already in use by another user"]);

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Phone = request.Phone;
        user.RoleId = request.RoleId;
        user.IsActive = request.IsActive;
        user.MarkUpdated();

        userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["User updated successfully"]);
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.FullName).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(v => v.RoleId).NotEmpty();
    }
}
