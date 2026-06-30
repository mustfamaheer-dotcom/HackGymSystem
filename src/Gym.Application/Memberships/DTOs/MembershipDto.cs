using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Memberships.DTOs;

public class MembershipDto : IMapFrom<Membership>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public Guid PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RemainingDays { get; set; }
    public int RemainingVisits { get; set; }
    public MembershipStatus Status { get; set; }
    public DateTime? FreezeStartDate { get; set; }
    public DateTime? FreezeEndDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Membership, MembershipDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.PlanName, opt => opt.MapFrom(s => s.Plan.Name));
    }
}
