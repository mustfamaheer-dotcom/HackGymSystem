using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class LeadFollowUp : BaseEntity
{
    public Guid LeadId { get; set; }
    public string Notes { get; set; } = string.Empty;

    public Lead? Lead { get; set; }

    private LeadFollowUp() { }

    public LeadFollowUp(Guid leadId, string notes)
    {
        LeadId = leadId;
        Notes = notes;
    }
}