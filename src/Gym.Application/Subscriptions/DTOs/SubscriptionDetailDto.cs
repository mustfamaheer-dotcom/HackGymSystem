using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Application.Members.DTOs;
using Gym.Application.MembershipPlans.DTOs;
using Gym.Application.Offers.DTOs;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Subscriptions.DTOs;

public class SubscriptionDetailDto : IMapFrom<Domain.Entities.Subscription>
{
    public Guid Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public Guid PlanId { get; set; }
    public Guid? OfferId { get; set; }
    public decimal TotalSubscriptionValue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingBalance { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime? FreezeStart { get; set; }
    public DateTime? FreezeEnd { get; set; }
    public int TotalFreezeDays { get; set; }
    public string? AdminSignature { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public MemberDto? Member { get; set; }
    public PlanDto? Plan { get; set; }
    public OfferDto? Offer { get; set; }
    public List<SubscriptionPaymentDto> Payments { get; set; } = new();
    public List<SubscriptionFreezeHistoryDto> FreezeHistories { get; set; } = new();
    public List<SubscriptionTransactionLogDto> TransactionLogs { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Subscription, SubscriptionDetailDto>()
            .ForMember(d => d.Payments, opt => opt.Ignore())
            .ForMember(d => d.FreezeHistories, opt => opt.Ignore())
            .ForMember(d => d.TransactionLogs, opt => opt.Ignore());
    }
}
