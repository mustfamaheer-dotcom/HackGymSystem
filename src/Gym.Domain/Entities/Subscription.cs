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
        if (string.IsNullOrWhiteSpace(receiptNumber))
            throw new ArgumentException("Receipt number is required", nameof(receiptNumber));
        if (totalValue <= 0)
            throw new ArgumentException("Total subscription value must be greater than zero", nameof(totalValue));
        if (amountPaid < 0)
            throw new ArgumentException("Amount paid cannot be negative", nameof(amountPaid));
        if (amountPaid > totalValue)
            throw new ArgumentException("Amount paid cannot exceed total subscription value", nameof(amountPaid));
        if (startDate >= expirationDate)
            throw new ArgumentException("Start date must be before expiration date", nameof(startDate));
        if (memberId == Guid.Empty)
            throw new ArgumentException("Member is required", nameof(memberId));
        if (planId == Guid.Empty)
            throw new ArgumentException("Plan is required", nameof(planId));

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

    public void Freeze(DateTime freezeStart, DateTime freezeEnd, int freezeDays, string? reason = null, string? invalidStatusErrorMessage = null)
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException(invalidStatusErrorMessage ?? "Only active subscriptions can be frozen");

        FreezeStart = freezeStart;
        FreezeEnd = freezeEnd;
        TotalFreezeDays += freezeDays;
        ExpirationDate = ExpirationDate.AddDays(freezeDays);
        Status = SubscriptionStatus.Frozen;
        MarkUpdated();
    }

    public void Unfreeze(string? invalidStatusErrorMessage = null)
    {
        if (Status != SubscriptionStatus.Frozen)
            throw new InvalidOperationException(invalidStatusErrorMessage ?? "Only frozen subscriptions can be unfrozen");

        FreezeStart = null;
        FreezeEnd = null;
        Status = SubscriptionStatus.Active;
        MarkUpdated();
    }

    public void MarkRenewed()
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
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero", nameof(amount));

        AmountPaid += amount;
        RemainingBalance = TotalSubscriptionValue - AmountPaid;
        if (RemainingBalance < 0)
            RemainingBalance = 0;
        MarkUpdated();
    }
}
