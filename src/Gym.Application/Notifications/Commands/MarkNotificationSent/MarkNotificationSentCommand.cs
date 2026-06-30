using FluentValidation;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.Notifications.Commands.MarkNotificationSent;

public record MarkNotificationSentCommand(Guid Id) : IRequest<Result>;

public class MarkNotificationSentCommandHandler : IRequestHandler<MarkNotificationSentCommand, Result>
{
    private readonly IRepository<Notification> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkNotificationSentCommandHandler(IRepository<Notification> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(MarkNotificationSentCommand request, CancellationToken cancellationToken)
    {
        var notification = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (notification == null)
            return Result.Failure("Notification not found");

        notification.MarkSent();
        _repository.Update(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public class MarkNotificationSentCommandValidator : AbstractValidator<MarkNotificationSentCommand>
{
    public MarkNotificationSentCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}
