using Gym.Shared.Enums;

namespace Gym.Application.DailySessions.DTOs;

public class DailySessionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public Guid? PlanId { get; set; }
    public string? PlanName { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
