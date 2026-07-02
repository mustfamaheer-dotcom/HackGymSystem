using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class SubscriptionTransactionLog : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? PerformedBy { get; set; }

    public Subscription Subscription { get; set; } = null!;
    public User? Performer { get; set; }

    private SubscriptionTransactionLog() { }

    public SubscriptionTransactionLog(Guid subscriptionId, string action, string description, Guid? performedBy = null)
    {
        SubscriptionId = subscriptionId;
        Action = action;
        Description = description;
        PerformedBy = performedBy;
    }
}
