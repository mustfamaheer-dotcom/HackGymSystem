using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class SubscriptionPayment : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Notes { get; set; }
    public decimal RunningBalance { get; set; }

    public Subscription Subscription { get; set; } = null!;
    public User? Employee { get; set; }

    private SubscriptionPayment() { }

    public SubscriptionPayment(Guid subscriptionId, decimal amount, PaymentMethod paymentMethod,
        decimal runningBalance, string? referenceNumber = null, Guid? employeeId = null, string? notes = null)
    {
        SubscriptionId = subscriptionId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        RunningBalance = runningBalance;
        ReferenceNumber = referenceNumber;
        EmployeeId = employeeId;
        Notes = notes;
    }
}
