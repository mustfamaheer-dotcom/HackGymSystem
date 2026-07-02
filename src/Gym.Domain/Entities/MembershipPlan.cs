using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class MembershipPlan : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int? MaxVisits { get; set; }
    public int? FreezeDays { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    private MembershipPlan() { }

    public MembershipPlan(string name, decimal price, int durationDays,
        int? maxVisits, int? freezeDays, string? description)
    {
        Name = name;
        Price = price;
        DurationDays = durationDays;
        MaxVisits = maxVisits;
        FreezeDays = freezeDays;
        Description = description;
    }

    public void Update(string name, decimal price, int durationDays,
        int? maxVisits, int? freezeDays, string? description)
    {
        Name = name;
        Price = price;
        DurationDays = durationDays;
        MaxVisits = maxVisits;
        FreezeDays = freezeDays;
        Description = description;
        MarkUpdated();
    }

    public void ToggleActive(bool active)
    {
        IsActive = active;
        MarkUpdated();
    }
}
