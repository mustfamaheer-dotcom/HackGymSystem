using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid MemberId { get; set; }
    public decimal Amount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Notes { get; set; }
    public string ReceiptNumber { get; set; }

    public Member Member { get; set; } = null!;
    public User? Employee { get; set; }

    private Payment() { }

    public Payment(Guid memberId, decimal amount, decimal discountAmount,
        PaymentMethod paymentMethod, string? referenceNumber, Guid? employeeId,
        string? notes, string receiptNumber)
    {
        MemberId = memberId;
        Amount = amount;
        DiscountAmount = discountAmount;
        TotalAmount = amount - discountAmount;
        PaymentDate = DateTime.UtcNow;
        PaymentMethod = paymentMethod;
        ReferenceNumber = referenceNumber;
        EmployeeId = employeeId;
        Notes = notes;
        ReceiptNumber = receiptNumber;
    }
}
