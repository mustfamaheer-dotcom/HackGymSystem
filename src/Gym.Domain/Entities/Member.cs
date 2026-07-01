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

    public MembershipPlan? Package { get; set; }
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    private Member() { }

    public Member(string receiptNumber, string fullName, string phoneNumber,
        decimal subscriptionPrice, decimal paidAmount,
        int subscriptionDurationMonths, DateTime subscriptionStartDate,
        PaymentMethod paymentMethod, DateTime registrationDate)
    {
        ReceiptNumber = receiptNumber;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        SubscriptionPrice = subscriptionPrice;
        PaidAmount = paidAmount;
        RemainingAmount = subscriptionPrice - paidAmount;
        SubscriptionDurationMonths = subscriptionDurationMonths;
        SubscriptionStartDate = subscriptionStartDate;
        SubscriptionEndDate = subscriptionStartDate.AddMonths(subscriptionDurationMonths);
        PaymentMethod = paymentMethod;
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

    public void UpdateSubscription(Guid? packageId, decimal subscriptionPrice,
        decimal paidAmount, int subscriptionDurationMonths, int freeMonths,
        int freezeDays, DateTime subscriptionStartDate)
    {
        PackageId = packageId;
        SubscriptionPrice = subscriptionPrice;
        PaidAmount = paidAmount;
        RemainingAmount = subscriptionPrice - paidAmount;
        SubscriptionDurationMonths = subscriptionDurationMonths;
        FreeMonths = freeMonths;
        FreezeDays = freezeDays;
        SubscriptionStartDate = subscriptionStartDate;
        SubscriptionEndDate = subscriptionStartDate.AddMonths(subscriptionDurationMonths);
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
