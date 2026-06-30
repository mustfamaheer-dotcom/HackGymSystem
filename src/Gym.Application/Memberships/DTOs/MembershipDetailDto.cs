using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Memberships.DTOs;

public class MembershipDetailDto : MembershipDto
{
    public MemberDto? Member { get; set; }
    public PlanDto? Plan { get; set; }

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Membership, MembershipDetailDto>()
            .IncludeBase<Membership, MembershipDto>()
            .ForMember(d => d.Member, opt => opt.MapFrom(s => s.Member))
            .ForMember(d => d.Plan, opt => opt.MapFrom(s => s.Plan));
    }
}

public class MemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public Gender Gender { get; set; }
}

public class PlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int? MaxVisits { get; set; }
    public bool IsActive { get; set; }
}
