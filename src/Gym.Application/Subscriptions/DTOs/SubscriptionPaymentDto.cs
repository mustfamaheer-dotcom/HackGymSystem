using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Subscriptions.DTOs;

public class SubscriptionPaymentDto : IMapFrom<SubscriptionPayment>
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? EmployeeName { get; set; }
    public string? Notes { get; set; }
    public decimal RunningBalance { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SubscriptionPayment, SubscriptionPaymentDto>()
            .ForMember(d => d.EmployeeName, opt => opt.MapFrom(s => s.Employee != null ? s.Employee.FullName : null));
    }
}
