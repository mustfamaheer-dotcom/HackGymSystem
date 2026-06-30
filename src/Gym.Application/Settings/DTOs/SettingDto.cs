using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Settings.DTOs;

public class SettingDto : IMapFrom<Setting>
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Group { get; set; }
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Setting, SettingDto>();
    }
}
