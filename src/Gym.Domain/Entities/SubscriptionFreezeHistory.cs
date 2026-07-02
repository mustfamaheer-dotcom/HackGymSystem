using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class SubscriptionFreezeHistory : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public DateTime FreezeStart { get; set; }
    public DateTime FreezeEnd { get; set; }
    public int FreezeDays { get; set; }
    public string? Reason { get; set; }
    public DateTime UnfreezeDate { get; set; }

    public Subscription Subscription { get; set; } = null!;

    private SubscriptionFreezeHistory() { }

    public SubscriptionFreezeHistory(Guid subscriptionId, DateTime freezeStart, DateTime freezeEnd,
        int freezeDays, string? reason = null)
    {
        SubscriptionId = subscriptionId;
        FreezeStart = freezeStart;
        FreezeEnd = freezeEnd;
        FreezeDays = freezeDays;
        Reason = reason;
    }

    public void SetUnfreezeDate(DateTime unfreezeDate)
    {
        UnfreezeDate = unfreezeDate;
    }
}
