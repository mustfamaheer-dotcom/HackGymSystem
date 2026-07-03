namespace Gym.Application.Members.DTOs;

public class UpdateMemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string? Nationality { get; set; }
    public string? NationalId { get; set; }
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
    public string? ImagePath { get; set; }
    public Guid? PackageId { get; set; }
    public Guid? OfferId { get; set; }
    public Guid? FingerprintDeviceId { get; set; }
    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }
    public decimal? SubscriptionPrice { get; set; }
    public decimal? PaidAmount { get; set; }
    public int? DurationMonths { get; set; }
    public int? FreeMonths { get; set; }
    public int? FreezeDays { get; set; }
    public DateTime? StartDate { get; set; }
    public string? PaymentMethod { get; set; }
}
