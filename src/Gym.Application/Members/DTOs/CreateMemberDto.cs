using System.ComponentModel.DataAnnotations;

namespace Gym.Application.Members.DTOs;

public class CreateMemberDto
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(200, ErrorMessage = "Full name must not exceed 200 characters")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Nationality is required")]
    [StringLength(100, ErrorMessage = "Nationality must not exceed 100 characters")]
    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "National ID is required")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 digits")]
    [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits")]
    public string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
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
    public Guid? FingerprintDeviceId { get; set; }
    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }
}
