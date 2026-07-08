using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class DailySession : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public Guid? PlanId { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public MembershipPlan? Plan { get; set; }

    private DailySession() { }

    public DailySession(string name, string phone, DateTime visitDate, Guid? planId, decimal amount, decimal paidAmount, PaymentMethod paymentMethod)
    {
        Name = name;
        Phone = phone;
        VisitDate = visitDate;
        PlanId = planId;
        Amount = amount;
        PaidAmount = paidAmount;
        RemainingBalance = amount - paidAmount;
        PaymentMethod = paymentMethod;
    }
}
