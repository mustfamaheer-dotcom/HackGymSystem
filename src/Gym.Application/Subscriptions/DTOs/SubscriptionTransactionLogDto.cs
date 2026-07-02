using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Subscriptions.DTOs;

public class SubscriptionTransactionLogDto : IMapFrom<SubscriptionTransactionLog>
{
    public Guid Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? PerformedByName { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SubscriptionTransactionLog, SubscriptionTransactionLogDto>()
            .ForMember(d => d.PerformedByName, opt => opt.MapFrom(s => s.Performer != null ? s.Performer.FullName : null));
    }
}
