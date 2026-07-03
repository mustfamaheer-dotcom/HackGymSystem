using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Lead : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public LeadSource Source { get; set; }
    public Guid? InterestedPackageId { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Notes { get; set; }

    public MembershipPlan? InterestedPackage { get; set; }
    public ICollection<LeadFollowUp> FollowUps { get; set; } = new List<LeadFollowUp>();

    private Lead() { }

    public Lead(string name, string phone, LeadSource source, Guid? interestedPackageId, string? notes)
    {
        Name = name;
        Phone = phone;
        Source = source;
        InterestedPackageId = interestedPackageId;
        Status = LeadStatus.New;
        Notes = notes;
    }

    public void Update(string name, string phone, LeadSource source, Guid? interestedPackageId, LeadStatus status, DateTime? nextFollowUpDate, string? notes)
    {
        Name = name;
        Phone = phone;
        Source = source;
        InterestedPackageId = interestedPackageId;
        Status = status;
        NextFollowUpDate = nextFollowUpDate;
        Notes = notes;
        MarkUpdated();
    }

    public void MarkConverted()
    {
        Status = LeadStatus.Converted;
        MarkUpdated();
    }

    public void MarkLost()
    {
        Status = LeadStatus.Lost;
        MarkUpdated();
    }
}