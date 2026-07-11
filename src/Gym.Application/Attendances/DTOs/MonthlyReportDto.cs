using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Attendances.DTOs;

public class MonthlyReportDto : IMapFrom<AttendanceSummary>
{
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string? MemberCode { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int PresentDays { get; set; }
    public int LateDays { get; set; }
    public int AbsentDays { get; set; }
    public int EarlyLeaveDays { get; set; }
    public int HalfDays { get; set; }
    public int OnLeaveDays { get; set; }
    public double TotalWorkHours { get; set; }
    public int WorkingDaysInMonth { get; set; }
    public double AttendancePercentage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttendanceSummary, MonthlyReportDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.MemberCode, opt => opt.MapFrom(s => s.Member.Code.ToString()))
            .ForMember(d => d.Year, opt => opt.MapFrom(s => s.Date.Year))
            .ForMember(d => d.Month, opt => opt.MapFrom(s => s.Date.Month));
    }
}

public class AttendanceSummaryDto : IMapFrom<AttendanceSummary>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public double? WorkDurationMinutes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttendanceSummary, AttendanceSummaryDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
    }
}

public class DashboardStatsDto
{
    public int TotalActiveMembers { get; set; }
    public int CheckedInToday { get; set; }
    public int AbsentToday { get; set; }
    public int LateToday { get; set; }
    public int OnLeaveToday { get; set; }
    public int TotalRecordsToday { get; set; }
    public int DevicesOnline { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class DeviceHealthDto
{
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastConnectedAt { get; set; }
    public bool IsActive { get; set; }
}
