using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Notifications.Commands.MarkNotificationFailed;

public record MarkNotificationFailedCommand(Guid Id, string Error) : IRequest<Result>;

public class MarkNotificationFailedCommandHandler : IRequestHandler<MarkNotificationFailedCommand, Result>
{
    private readonly IRepository<Notification> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkNotificationFailedCommandHandler(IRepository<Notification> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(MarkNotificationFailedCommand request, CancellationToken cancellationToken)
    {
        var notification = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (notification == null)
            return Result.Failure("Notification not found");

        notification.MarkFailed(request.Error);
        _repository.Update(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class MarkNotificationFailedCommandValidator : AbstractValidator<MarkNotificationFailedCommand>
{
    public MarkNotificationFailedCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Error).NotEmpty();
    }
}
