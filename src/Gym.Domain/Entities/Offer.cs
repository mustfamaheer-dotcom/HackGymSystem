using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Offer : BaseEntity
{
    public string OfferTitle { get; set; }
    public Guid? LinkedPackageId { get; set; }
    public OfferType OfferType { get; set; }
    public int? BonusMonths { get; set; }
    public int? BonusDays { get; set; }
    public decimal? OfferPrice { get; set; }
    public int? ExtraFreezeDays { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public MembershipPlan? LinkedPackage { get; set; }

    private Offer() { }

    public Offer(string offerTitle, OfferType offerType, DateTime startDate, DateTime endDate,
        Guid? linkedPackageId = null, int? bonusMonths = null, int? bonusDays = null,
        decimal? offerPrice = null, int? extraFreezeDays = null, string? description = null)
    {
        OfferTitle = offerTitle;
        OfferType = offerType;
        LinkedPackageId = linkedPackageId;
        BonusMonths = bonusMonths;
        BonusDays = bonusDays;
        OfferPrice = offerPrice;
        ExtraFreezeDays = extraFreezeDays;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void Update(string offerTitle, OfferType offerType, DateTime startDate, DateTime endDate,
        Guid? linkedPackageId, int? bonusMonths, int? bonusDays,
        decimal? offerPrice, int? extraFreezeDays, string? description)
    {
        OfferTitle = offerTitle;
        OfferType = offerType;
        LinkedPackageId = linkedPackageId;
        BonusMonths = bonusMonths;
        BonusDays = bonusDays;
        OfferPrice = offerPrice;
        ExtraFreezeDays = extraFreezeDays;
        Description = description;
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
