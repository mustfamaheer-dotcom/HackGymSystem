using System.ComponentModel.DataAnnotations;

namespace Gym.Application.Members.DTOs;

public class UpdateMemberDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(200, ErrorMessage = "Full name must not exceed 200 characters")]
    public string FullName { get; set; }

    [StringLength(100, ErrorMessage = "Nationality must not exceed 100 characters")]
    public string? Nationality { get; set; }

    [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 digits")]
    public string? NationalId { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(15, ErrorMessage = "Phone number must not exceed 15 characters")]
    public string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters")]
    public string? Notes { get; set; }

    [StringLength(200, ErrorMessage = "Company must not exceed 200 characters")]
    public string? Company { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters")]
    public string? Address { get; set; }

    public decimal? Weight { get; set; }
    public bool HasDisease { get; set; }

    [StringLength(500, ErrorMessage = "Disease type must not exceed 500 characters")]
    public string? DiseaseType { get; set; }

    [StringLength(200, ErrorMessage = "Referral source must not exceed 200 characters")]
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
