using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Subscriptions.DTOs;

public class SubscriptionFreezeHistoryDto : IMapFrom<SubscriptionFreezeHistory>
{
    public Guid Id { get; set; }
    public DateTime FreezeStart { get; set; }
    public DateTime FreezeEnd { get; set; }
    public int FreezeDays { get; set; }
    public string? Reason { get; set; }
    public DateTime UnfreezeDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SubscriptionFreezeHistory, SubscriptionFreezeHistoryDto>();
    }
}
