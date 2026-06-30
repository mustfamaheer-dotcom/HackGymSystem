using Gym.Application.Common.DTOs;
using Gym.Application.Members.DTOs;
using Gym.Domain.Entities;

namespace Gym.API.ViewModels;

public class MemberSearchViewModel
{
    public string? Name { get; set; }
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ReceiptNumber { get; set; }
    public Guid? PackageId { get; set; }
    public string? SubscriptionStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public bool ExpiringSoon { get; set; }
    public int ExpiringWithinDays { get; set; } = 7;
    public bool OutstandingBalance { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public PaginatedResult<MemberDto>? Results { get; set; }
    public List<MembershipPlan> Plans { get; set; } = new();
    public bool HasSearched => Results is not null;
}
