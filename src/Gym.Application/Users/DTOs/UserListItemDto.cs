using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Users.DTOs;

public class UserListItemDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string RoleName { get; set; }
    public string RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserListItemDto>()
            .ForMember(d => d.RoleName, opt => opt.MapFrom(s => s.Role.Name))
            .ForMember(d => d.RoleId, opt => opt.MapFrom(s => s.RoleId.ToString()));
    }
}
