using System.ComponentModel.DataAnnotations;
using Gym.Shared.Enums;

namespace Gym.Application.Leads.DTOs;

public class LeadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public LeadSource Source { get; set; }
    public Guid? InterestedPackageId { get; set; }
    public string? InterestedPackageName { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FollowUpCount { get; set; }
}

public class LeadFollowUpDto
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateLeadDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name must not exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required")]
    [StringLength(15, ErrorMessage = "Phone must not exceed 15 characters")]
    [RegularExpression(@"^\d{11,15}$", ErrorMessage = "Phone must be 11 to 15 digits")]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
    public string? Email { get; set; }

    public Gender? Gender { get; set; }

    [Required(ErrorMessage = "Source is required")]
    public LeadSource Source { get; set; }

    public Guid? InterestedPackageId { get; set; }

    [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters")]
    public string? Notes { get; set; }
}

public class UpdateLeadDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name must not exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required")]
    [StringLength(15, ErrorMessage = "Phone must not exceed 15 characters")]
    [RegularExpression(@"^\d{11,15}$", ErrorMessage = "Phone must be 11 to 15 digits")]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
    public string? Email { get; set; }

    public Gender? Gender { get; set; }

    [Required(ErrorMessage = "Source is required")]
    public LeadSource Source { get; set; }

    public Guid? InterestedPackageId { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? NextFollowUpDate { get; set; }

    [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters")]
    public string? Notes { get; set; }
}

public class AddFollowUpDto
{
    [Required(ErrorMessage = "Lead id is required")]
    public Guid LeadId { get; set; }

    [Required(ErrorMessage = "Notes is required")]
    [StringLength(1000, ErrorMessage = "Notes must not exceed 1000 characters")]
    public string Notes { get; set; } = string.Empty;
}

public class ConvertToMemberDto
{
    [Required(ErrorMessage = "Lead id is required")]
    public Guid LeadId { get; set; }

    public Guid? PackageId { get; set; }

    [Required(ErrorMessage = "Subscription price is required")]
    public decimal SubscriptionPrice { get; set; }

    [Required(ErrorMessage = "Paid amount is required")]
    public decimal PaidAmount { get; set; }
}