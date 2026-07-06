namespace Gym.Shared.Common;

public abstract record SubscriptionDomainEvent : DomainEvent
{
    public Guid MemberId { get; init; }
    public Guid SubscriptionId { get; init; }
    public DateTime? ExpiryDate { get; init; }
}

public sealed record SubscriptionActivatedEvent : SubscriptionDomainEvent;
public sealed record SubscriptionExpiredEvent : SubscriptionDomainEvent;
public sealed record SubscriptionUpgradedEvent : SubscriptionDomainEvent;
public sealed record SubscriptionSuspendedEvent : SubscriptionDomainEvent;
public sealed record SubscriptionRenewedEvent : SubscriptionDomainEvent;
