using Gym.Shared.Common;

namespace Gym.Domain.Entities;

public class Package : BaseEntity
{
    public string PackageName { get; set; }
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public int? FreePeriodMonths { get; set; }
    public int? MaxFreezeDays { get; set; }
    public bool IsActive { get; set; } = true;

    private Package() { }

    public Package(string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays)
    {
        PackageName = packageName;
        DurationMonths = durationMonths;
        Price = price;
        FreePeriodMonths = freePeriodMonths;
        MaxFreezeDays = maxFreezeDays;
    }

    public void Update(string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays)
    {
        PackageName = packageName;
        DurationMonths = durationMonths;
        Price = price;
        FreePeriodMonths = freePeriodMonths;
        MaxFreezeDays = maxFreezeDays;
        MarkUpdated();
    }

    public void ToggleActive(bool active)
    {
        IsActive = active;
        MarkUpdated();
    }
}
