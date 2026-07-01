using Gym.Shared.Enums;

namespace Gym.Application.Offers.DTOs;

public class AppliedOfferDto
{
    public Guid OfferId { get; set; }
    public string OfferTitle { get; set; } = string.Empty;
    public OfferType OfferType { get; set; }
    public int OriginalDurationMonths { get; set; }
    public decimal OriginalPrice { get; set; }
    public int FinalDurationMonths { get; set; }
    public decimal FinalPrice { get; set; }
    public int? BonusDays { get; set; }
    public int? ExtraFreezeDays { get; set; }
    public string? Description { get; set; }
}
