using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Attendances.DTOs;

public class AttendanceDto : IMapFrom<Attendance>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public bool IsManual { get; set; }
    public AttendanceSyncStatus SyncStatus { get; set; }
    public Guid? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Attendance, AttendanceDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.DeviceName, opt => opt.MapFrom(s => s.Device != null ? s.Device.Name : null));
    }
}
