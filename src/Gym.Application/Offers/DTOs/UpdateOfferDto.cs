using Gym.Shared.Enums;

namespace Gym.Application.Offers.DTOs;

public class UpdateOfferDto
{
    public Guid Id { get; set; }
    public string OfferTitle { get; set; } = string.Empty;
    public Guid? LinkedPackageId { get; set; }
    public OfferType OfferType { get; set; }
    public int? BonusMonths { get; set; }
    public int? BonusDays { get; set; }
    public decimal? OfferPrice { get; set; }
    public int? ExtraFreezeDays { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
