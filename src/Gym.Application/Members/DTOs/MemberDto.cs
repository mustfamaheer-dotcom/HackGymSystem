using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Members.DTOs;

public class MemberDto : IMapFrom<Member>
{
    public Guid Id { get; set; }
    public string ReceiptNumber { get; set; }
    public string FullName { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Notes { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public decimal? Weight { get; set; }
    public bool HasDisease { get; set; }
    public string? DiseaseType { get; set; }
    public string? ReferralSource { get; set; }
    public Guid? PackageId { get; set; }
    public string? PackageName { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public int SubscriptionDurationMonths { get; set; }
    public int FreeMonths { get; set; }
    public int FreezeDays { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime SubscriptionEndDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? FingerprintDeviceId { get; set; }
    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Member, MemberDto>()
            .ForMember(d => d.PackageName, opt => opt.MapFrom(s => s.Package != null ? s.Package.Name : null));
    }
}
