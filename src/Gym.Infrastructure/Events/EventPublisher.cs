using Gym.Application.Common.Events;
using Gym.Domain.Events;

namespace Gym.Infrastructure.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IEventBus _eventBus;

    public EventPublisher(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
    {
        await _eventBus.PublishAsync(@event, cancellationToken);
    }
}
