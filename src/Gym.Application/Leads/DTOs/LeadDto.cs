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
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public LeadSource Source { get; set; }
    public Guid? InterestedPackageId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateLeadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public LeadSource Source { get; set; }
    public Guid? InterestedPackageId { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Notes { get; set; }
}

public class AddFollowUpDto
{
    public Guid LeadId { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class ConvertToMemberDto
{
    public Guid LeadId { get; set; }
    public Guid? PackageId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public decimal PaidAmount { get; set; }
}