using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Shared.Enums;

namespace Gym.Application.WhatsAppTemplates.DTOs;

public class WhatsAppTemplateDto : IMapFrom<Domain.Entities.WhatsAppTemplate>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public NotificationType? TriggerType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.WhatsAppTemplate, WhatsAppTemplateDto>();
    }
}
