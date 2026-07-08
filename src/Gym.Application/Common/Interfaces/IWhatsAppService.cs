using Gym.Shared.Common;

namespace Gym.Application.Common.Interfaces;

public class MemberWhatsAppData
{
    public string MemberPhone { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public string JoinDate { get; set; } = string.Empty;
    public string ReceiptNumber { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string ExpirationDate { get; set; } = string.Empty;
    public string DaysRemaining { get; set; } = string.Empty;
    public string SubscriptionStatus { get; set; } = string.Empty;
    public string TotalPaid { get; set; } = string.Empty;
    public string LastPayment { get; set; } = string.Empty;
    public string RemainingBalance { get; set; } = string.Empty;
    public string LastPaymentDate { get; set; } = string.Empty;
    public string Offers { get; set; } = string.Empty;
}

public interface IWhatsAppService
{
    bool IsConfigured { get; }
    Task<Result> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    Task<Result> SendMemberAsync(MemberWhatsAppData data, string templateBody, string language, CancellationToken cancellationToken = default);
}