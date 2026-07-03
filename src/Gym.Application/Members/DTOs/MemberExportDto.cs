using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Members.DTOs;

public class MemberExportDto
{
    public int Code { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public string? ReferralSource { get; set; }
    public DateTime RegistrationDate { get; set; }

    public decimal? Weight { get; set; }
    public bool HasDisease { get; set; }
    public string? DiseaseType { get; set; }
    public string? Notes { get; set; }

    public string? FingerprintDeviceId { get; set; }

    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }

    public string? PlanName { get; set; }
    public string? SubReceiptNumber { get; set; }
    public decimal? TotalSubscriptionValue { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal? RemainingBalance { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? SubStatus { get; set; }
    public DateTime? FreezeStart { get; set; }
    public DateTime? FreezeEnd { get; set; }
    public int? TotalFreezeDays { get; set; }
    public string? SubNotes { get; set; }

    public static MemberExportDto FromMember(Member member)
    {
        var latestSub = member.Subscriptions?
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefault();

        return new MemberExportDto
        {
            Code = member.Code,
            ReceiptNumber = member.ReceiptNumber,
            FullName = member.FullName,
            Nationality = member.Nationality,
            NationalId = member.NationalId,
            PhoneNumber = member.PhoneNumber,
            Email = member.Email,
            DateOfBirth = member.DateOfBirth,
            Gender = member.Gender?.ToString(),
            Company = member.Company,
            Address = member.Address,
            ReferralSource = member.ReferralSource,
            RegistrationDate = member.RegistrationDate,
            Weight = member.Weight,
            HasDisease = member.HasDisease,
            DiseaseType = member.DiseaseType,
            Notes = member.Notes,
            FingerprintDeviceId = member.FingerprintDeviceId?.ToString(),
            MemberSignature = member.MemberSignature,
            AdminSignature = member.AdminSignature,
            PlanName = member.Package?.Name ?? latestSub?.Plan?.Name,
            SubReceiptNumber = latestSub?.ReceiptNumber,
            TotalSubscriptionValue = latestSub?.TotalSubscriptionValue,
            AmountPaid = latestSub?.AmountPaid,
            RemainingBalance = latestSub?.RemainingBalance,
            PaymentMethod = latestSub?.PaymentMethod.ToString(),
            StartDate = latestSub?.StartDate,
            ExpirationDate = latestSub?.ExpirationDate,
            SubStatus = latestSub?.Status.ToString(),
            FreezeStart = latestSub?.FreezeStart,
            FreezeEnd = latestSub?.FreezeEnd,
            TotalFreezeDays = latestSub?.TotalFreezeDays,
            SubNotes = latestSub?.Notes,
        };
    }
}
