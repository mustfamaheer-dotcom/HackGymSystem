using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Application.Resources;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userRepo = _unitOfWork.Repository<User>();

        var user = await userRepo.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return Result.Failure(_localizer["User not found"]);

        userRepo.Delete(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["User deleted successfully"]);
    }
}

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}
