using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Gym.Application.Common.Interfaces;
using Gym.Shared.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiToken;
    private readonly string _defaultLanguage;
    private readonly ILogger<WhatsAppService> _logger;

    public WhatsAppService(HttpClient httpClient, IConfiguration configuration, ILogger<WhatsAppService> logger)
    {
        _httpClient = httpClient;
        _apiUrl = configuration["WhatsApp:ApiUrl"] ?? "https://graph.facebook.com/v18.0";
        _apiToken = configuration["WhatsApp:ApiToken"] ?? string.Empty;
        _defaultLanguage = configuration["WhatsApp:DefaultLanguage"] ?? "ar";
        _logger = logger;
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_apiToken);

    public async Task<Result> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_apiToken))
        {
            _logger.LogWarning("WhatsApp API token not configured. Falling back to wa.me link.");
            return Result.Success();
        }

        try
        {
            var phone = phoneNumber.TrimStart('0', '+', ' ');
            if (!phone.StartsWith("2"))
                phone = "2" + phone;

            var payload = new
            {
                messaging_product = "whatsapp",
                to = phone,
                type = "text",
                text = new { body = message }
            };

            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/messages", payload, cancellationToken);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("WhatsApp message sent to {Phone}", phone);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send WhatsApp message to {Phone}", phoneNumber);
            return Result.Failure("Failed to send WhatsApp message.");
        }
    }

    public async Task<Result> SendMemberAsync(MemberWhatsAppData data, string templateBody, string language, CancellationToken cancellationToken = default)
    {
        var rendered = RenderTemplate(templateBody, data, language);
        return await SendAsync(data.MemberPhone, rendered, cancellationToken);
    }

    private static string RenderTemplate(string template, MemberWhatsAppData data, string language)
    {
        var replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["{MemberName}"] = data.MemberName,
            ["{MemberCode}"] = data.MemberCode,
            ["{JoinDate}"] = data.JoinDate,
            ["{ReceiptNumber}"] = data.ReceiptNumber,
            ["{StartDate}"] = data.StartDate,
            ["{ExpirationDate}"] = data.ExpirationDate,
            ["{DaysRemaining}"] = data.DaysRemaining,
            ["{SubscriptionStatus}"] = data.SubscriptionStatus,
            ["{TotalPaid}"] = data.TotalPaid,
            ["{LastPayment}"] = data.LastPayment,
            ["{RemainingBalance}"] = data.RemainingBalance,
            ["{LastPaymentDate}"] = data.LastPaymentDate,
            ["{Offers}"] = data.Offers,
        };

        var sb = new StringBuilder(template);
        foreach (var (key, value) in replacements)
        {
            sb.Replace(key, value);
        }
        return sb.ToString();
    }
}