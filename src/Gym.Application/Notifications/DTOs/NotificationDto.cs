using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Notifications.DTOs;

public class NotificationDto : IMapFrom<Notification>
{
    public Guid Id { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? SentDate { get; set; }
    public bool IsPending { get; set; }
    public int RetryCount { get; set; }
    public string? LastError { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Notification, NotificationDto>()
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member != null ? s.Member.FullName : null));
    }
}
