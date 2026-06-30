using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Membership : BaseEntity
{
    public Guid MemberId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RemainingDays { get; set; }
    public int RemainingVisits { get; set; }
    public MembershipStatus Status { get; set; } = MembershipStatus.Active;
    public DateTime? FreezeStartDate { get; set; }
    public DateTime? FreezeEndDate { get; set; }
    public string? Notes { get; set; }

    public Member Member { get; set; } = null!;
    public MembershipPlan Plan { get; set; } = null!;

    private Membership() { }

    public Membership(Guid memberId, Guid planId, DateTime startDate, DateTime endDate,
        int remainingDays, int remainingVisits)
    {
        MemberId = memberId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        RemainingDays = remainingDays;
        RemainingVisits = remainingVisits;
    }

    public void Renew(DateTime newEndDate, int additionalDays, int? additionalVisits)
    {
        EndDate = newEndDate;
        RemainingDays += additionalDays;
        if (additionalVisits.HasValue)
            RemainingVisits += additionalVisits.Value;
        Status = MembershipStatus.Active;
        MarkUpdated();
    }

    public void Upgrade(int additionalDays, int additionalVisits, DateTime newEndDate)
    {
        RemainingDays += additionalDays;
        RemainingVisits += additionalVisits;
        EndDate = newEndDate;
        MarkUpdated();
    }

    public void Downgrade(int reduceVisits)
    {
        RemainingVisits = Math.Max(0, RemainingVisits - reduceVisits);
        MarkUpdated();
    }

    public void Freeze(int freezeDays)
    {
        FreezeStartDate = DateTime.UtcNow;
        FreezeEndDate = FreezeStartDate.Value.AddDays(freezeDays);
        Status = MembershipStatus.Frozen;
        RemainingDays += freezeDays;
        MarkUpdated();
    }

    public void Unfreeze()
    {
        FreezeStartDate = null;
        FreezeEndDate = null;
        Status = MembershipStatus.Active;
        MarkUpdated();
    }

    public void Cancel()
    {
        Status = MembershipStatus.Cancelled;
        MarkUpdated();
    }
}
