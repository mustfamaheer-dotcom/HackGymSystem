using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Offer : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    private Offer() { }

    public Offer(string title, string? description, DiscountType discountType,
        decimal discountValue, DateTime startDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        DiscountType = discountType;
        DiscountValue = discountValue;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void Update(string title, string? description, DiscountType discountType,
        decimal discountValue, DateTime startDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        DiscountType = discountType;
        DiscountValue = discountValue;
        StartDate = startDate;
        EndDate = endDate;
        MarkUpdated();
    }

    public void ToggleActive(bool active)
    {
        IsActive = active;
        MarkUpdated();
    }
}
