using Gym.Shared.Common;
using Gym.Shared.Enums;

namespace Gym.Domain.Entities;

public class Member : BaseEntity
{
    public int Code { get; set; }
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
    public Guid? FingerprintDeviceId { get; set; }
    public string? MemberSignature { get; set; }
    public string? AdminSignature { get; set; }
    public string? ImagePath { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsDeleted { get; set; }

    public MembershipPlan? Package { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    private Member() { }

    public Member(string receiptNumber, string fullName, string phoneNumber, DateTime registrationDate)
    {
        if (string.IsNullOrWhiteSpace(receiptNumber))
            throw new ArgumentException("Receipt number is required", nameof(receiptNumber));
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required", nameof(fullName));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        if (registrationDate == default)
            throw new ArgumentException("Registration date is required", nameof(registrationDate));

        ReceiptNumber = receiptNumber;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        RegistrationDate = registrationDate;
    }

    public void UpdateBasicInfo(string fullName, string nationality, string nationalId,
        string phoneNumber, string? company, string? address, decimal? weight,
        bool hasDisease, string? diseaseType, string? referralSource)
    {
        FullName = fullName;
        Nationality = nationality;
        NationalId = nationalId;
        PhoneNumber = phoneNumber;
        Company = company;
        Address = address;
        Weight = weight;
        HasDisease = hasDisease;
        DiseaseType = hasDisease ? diseaseType : null;
        ReferralSource = referralSource;
        MarkUpdated();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        MarkUpdated();
    }

    public void Restore()
    {
        IsDeleted = false;
        MarkUpdated();
    }
}
