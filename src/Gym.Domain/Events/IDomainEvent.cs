using MediatR;

namespace Gym.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime Timestamp { get; }
}
