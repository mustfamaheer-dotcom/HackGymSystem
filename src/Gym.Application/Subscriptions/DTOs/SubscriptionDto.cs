using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Subscriptions.DTOs;

public class SubscriptionDto : IMapFrom<Domain.Entities.Subscription>
{
    public Guid Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberPhone { get; set; } = string.Empty;
    public Guid PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public Guid? OfferId { get; set; }
    public string? OfferTitle { get; set; }
    public decimal TotalSubscriptionValue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingBalance { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Subscription, SubscriptionDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.MemberPhone, opt => opt.MapFrom(s => s.Member.PhoneNumber))
            .ForMember(d => d.PlanName, opt => opt.MapFrom(s => s.Plan.Name))
            .ForMember(d => d.OfferTitle, opt => opt.MapFrom(s => s.Offer != null ? s.Offer.OfferTitle : null));
    }
}
