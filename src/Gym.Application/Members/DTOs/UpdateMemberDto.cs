namespace Gym.Application.Members.DTOs;

public class UpdateMemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Notes { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public decimal? Weight { get; set; }
    public bool HasDisease { get; set; }
    public string? DiseaseType { get; set; }
    public string? ReferralSource { get; set; }
    public Guid? PackageId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public decimal PaidAmount { get; set; }
    public int SubscriptionDurationMonths { get; set; }
    public int FreeMonths { get; set; }
    public int FreezeDays { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public string PaymentMethod { get; set; }
    public int? FingerprintDeviceId { get; set; }
    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }
}
