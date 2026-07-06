using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gym.Shared.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void QueueDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

public interface IDomainEvent
{
    public DateTime OccurredAt { get; }
}

public abstract record DomainEvent : IDomainEvent, INotification
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
