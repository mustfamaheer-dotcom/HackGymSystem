using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Subscription : BaseEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public Guid PlanId { get; set; }
    public Guid? OfferId { get; set; }
    public decimal TotalSubscriptionValue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingBalance { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public DateTime? FreezeStart { get; set; }
    public DateTime? FreezeEnd { get; set; }
    public int TotalFreezeDays { get; set; }
    public string? AdminSignature { get; set; }
    public string? Notes { get; set; }

    public Member Member { get; set; } = null!;
    public MembershipPlan Plan { get; set; } = null!;
    public Offer? Offer { get; set; }
    public ICollection<SubscriptionPayment> Payments { get; set; } = new List<SubscriptionPayment>();
    public ICollection<SubscriptionFreezeHistory> FreezeHistories { get; set; } = new List<SubscriptionFreezeHistory>();
    public ICollection<SubscriptionTransactionLog> TransactionLogs { get; set; } = new List<SubscriptionTransactionLog>();

    private Subscription() { }

    public Subscription(string receiptNumber, Guid memberId, Guid planId, decimal totalValue,
        decimal amountPaid, PaymentMethod paymentMethod, DateTime startDate, DateTime expirationDate,
        Guid? offerId = null)
    {
        ReceiptNumber = receiptNumber;
        MemberId = memberId;
        PlanId = planId;
        OfferId = offerId;
        TotalSubscriptionValue = totalValue;
        AmountPaid = amountPaid;
        RemainingBalance = totalValue - amountPaid;
        PaymentMethod = paymentMethod;
        StartDate = startDate;
        ExpirationDate = expirationDate;
    }

    public void Freeze(DateTime freezeStart, DateTime freezeEnd, int freezeDays, string? reason = null)
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException("Only active subscriptions can be frozen");

        FreezeStart = freezeStart;
        FreezeEnd = freezeEnd;
        TotalFreezeDays += freezeDays;
        ExpirationDate = ExpirationDate.AddDays(freezeDays);
        Status = SubscriptionStatus.Frozen;
        MarkUpdated();
    }

    public void Unfreeze()
    {
        if (Status != SubscriptionStatus.Frozen)
            throw new InvalidOperationException("Only frozen subscriptions can be unfrozen");

        FreezeStart = null;
        FreezeEnd = null;
        Status = SubscriptionStatus.Active;
        MarkUpdated();
    }

    public void Renew(Guid newPlanId, Guid? newOfferId, decimal newTotalValue, decimal newAmountPaid,
        PaymentMethod newPaymentMethod, DateTime newStartDate, DateTime newExpirationDate, string newReceiptNumber)
    {
        Status = SubscriptionStatus.Renewed;
        MarkUpdated();
    }

    public void MarkExpired()
    {
        if (Status == SubscriptionStatus.Active || Status == SubscriptionStatus.Frozen)
        {
            Status = SubscriptionStatus.Expired;
            MarkUpdated();
        }
    }

    public void RecordPayment(decimal amount)
    {
        AmountPaid += amount;
        RemainingBalance = TotalSubscriptionValue - AmountPaid;
        if (RemainingBalance < 0)
            RemainingBalance = 0;
        MarkUpdated();
    }
}
