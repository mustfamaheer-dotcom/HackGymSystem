namespace Gym.Application.Members.DTOs;

public class MemberPaymentDto
{
    public Guid PaymentId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string SubscriptionReceipt { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal RunningBalance { get; set; }
    public string? RecordedBy { get; set; }
}

public class MemberPaymentHistoryViewModel
{
    public Guid MemberId { get; set; }
    public int MemberCode { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberPhone { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public int PaymentCount { get; set; }
    public List<MemberPaymentDto> Payments { get; set; } = new();
}
