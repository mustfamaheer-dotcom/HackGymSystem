using System.ComponentModel.DataAnnotations;
using Gym.Shared.Enums;

namespace Gym.Application.Offers.DTOs;

public class UpdateOfferDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Offer title is required")]
    [StringLength(200, ErrorMessage = "Offer title must not exceed 200 characters")]
    public string OfferTitle { get; set; } = string.Empty;

    public Guid? LinkedPackageId { get; set; }

    public OfferType OfferType { get; set; } = OfferType.FixedPrice;

    public decimal? OfferPrice { get; set; }
    public int? BonusMonths { get; set; }
    public int? BonusDays { get; set; }

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
}
