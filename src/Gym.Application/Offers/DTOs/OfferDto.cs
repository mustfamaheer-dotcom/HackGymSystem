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

    public string OfferTypeDisplay => OfferType switch
    {
        OfferType.BonusDuration => $"Bonus Duration (+{BonusMonths}m +{BonusDays}d)" + (LinkedPackageName != null ? $" - {LinkedPackageName}" : ""),
        OfferType.FixedPrice => $"{OfferPrice:N2} EGP" + (LinkedPackageName != null ? $" - {LinkedPackageName}" : ""),
        OfferType.ExtraFreeze => $"+{ExtraFreezeDays} Freeze Days" + (LinkedPackageName != null ? $" - {LinkedPackageName}" : ""),
        OfferType.FreeRegistration => "Free Registration" + (LinkedPackageName != null ? $" - {LinkedPackageName}" : ""),
        OfferType.Custom => "Custom" + (LinkedPackageName != null ? $" - {LinkedPackageName}" : ""),
        _ => ""
    };

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Offer, OfferDto>()
            .ForMember(d => d.LinkedPackageName, opt => opt.MapFrom(s => s.LinkedPackage != null ? s.LinkedPackage.Name : null));
    }
}
