using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Offers.DTOs;

public class OfferDto : IMapFrom<Offer>
{
    public Guid Id { get; set; }
    public string OfferTitle { get; set; } = string.Empty;
    public Guid? LinkedPackageId { get; set; }
    public string? LinkedPackageName { get; set; }
    public OfferType OfferType { get; set; }
    public int? BonusMonths { get; set; }
    public int? BonusDays { get; set; }
    public decimal? OfferPrice { get; set; }
    public int? ExtraFreezeDays { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string OfferTypeDisplay
    {
        get
        {
            var duration = "";
            if (BonusMonths > 0) duration += $"+{BonusMonths}m ";
            if (BonusDays > 0) duration += $"+{BonusDays}d ";
            duration = duration.TrimEnd();

            var price = OfferPrice.HasValue ? $"{OfferPrice:N2} EGP" : "";
            var parts = new[] { price, duration };
            var display = string.Join(" ", parts.Where(p => !string.IsNullOrEmpty(p)));
            if (LinkedPackageName != null) display += $" - {LinkedPackageName}";
            return display;
        }
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Offer, OfferDto>()
            .ForMember(d => d.LinkedPackageName, opt => opt.MapFrom(s => s.LinkedPackage != null ? s.LinkedPackage.Name : null));
    }
}
