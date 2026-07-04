using FluentValidation;
using Microsoft.Extensions.Localization;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : IRequest<Result>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private const int BcryptWorkFactor = 11;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ApplicationResources> _localizer;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<ApplicationResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .Query()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(_localizer["User not found."]);

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return Result.Failure(_localizer["Current password is incorrect."]);

        var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: BcryptWorkFactor);
        user.MarkPasswordChanged(newHash);
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_localizer["Password changed successfully."]);
    }
}

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(IStringLocalizer<ApplicationResources> localizer)
    {
        RuleFor(v => v.CurrentPassword)
            .NotEmpty().WithMessage(localizer["Current password is required."]);
        RuleFor(v => v.NewPassword)
            .NotEmpty().WithMessage(localizer["New password is required."])
            .MinimumLength(6).WithMessage(localizer["Password must be at least 6 characters."]);
    }
}
