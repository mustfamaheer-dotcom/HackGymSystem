using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;

namespace Gym.Application.Notifications.Commands.CreateNotification;

public record CreateNotificationCommand(
    Guid? MemberId,
    NotificationType Type,
    NotificationChannel Channel,
    string Subject,
    string Message,
    DateTime? ScheduledDate = null) : IRequest<Result<Guid>>;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<Guid>>
{
    private readonly IRepository<Notification> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNotificationCommandHandler(IRepository<Notification> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification(
            request.MemberId,
            request.Type,
            request.Channel,
            request.Subject,
            request.Message,
            request.ScheduledDate);

        await _repository.AddAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(notification.Id);
    }
}

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(v => v.Type).IsInEnum();
        RuleFor(v => v.Channel).IsInEnum();
        RuleFor(v => v.Subject).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Message).NotEmpty();
    }
}
