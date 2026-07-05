using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Gym.Application.Common.Interfaces;
using Gym.Shared.Common;
using Microsoft.Extensions.Configuration;

namespace Gym.Infrastructure.Services;

public class CaptchaService : ICaptchaService
{
    private readonly string _secretKey;
    private static readonly HttpClient _httpClient = new();

    public CaptchaService(IConfiguration configuration)
    {
        _secretKey = configuration["Captcha:SecretKey"] ?? string.Empty;
    }

    public async Task<Result> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_secretKey))
            return Result.Failure("CAPTCHA verification failed.");

        try
        {
            var response = await _httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _secretKey),
                    new KeyValuePair<string, string>("response", token)
                }),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
                return Result.Failure("CAPTCHA verification failed.");

            var result = await response.Content.ReadFromJsonAsync<RecaptchaResponse>(cancellationToken);

            if (result is null || !result.Success || result.Score < 0.5)
                return Result.Failure("CAPTCHA verification failed.");

            return Result.Success();
        }
        catch
        {
            return Result.Failure("CAPTCHA verification failed.");
        }
    }

    private class RecaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }
    }
}
