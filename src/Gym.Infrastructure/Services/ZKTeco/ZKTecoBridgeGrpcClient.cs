using System.Text.Json;
using Gym.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gym.Infrastructure.Services.ZKTeco;

public class ZKTecoBridgeOptions
{
    public string GrpcUrl { get; set; } = "http://localhost:50051";
}

public class ZKTecoBridgeGrpcClient : IZKTecoBridgeClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ZKTecoBridgeGrpcClient> _logger;
    private readonly string _baseUrl;

    public ZKTecoBridgeGrpcClient(
        IHttpClientFactory httpClientFactory,
        IOptions<ZKTecoBridgeOptions> options,
        ILogger<ZKTecoBridgeGrpcClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ZKTecoBridge");
        _baseUrl = options.Value.GrpcUrl;
        _logger = logger;
    }

    public async Task<DeviceHealthStatus> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/CheckHealth",
                new StringContent("{}", System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new DeviceHealthStatus { IsConnected = false };

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return new DeviceHealthStatus
            {
                IsConnected = data.GetProperty("isConnected").GetBoolean(),
                EnrolledUserCount = data.GetProperty("enrolledUserCount").GetInt32(),
                FreeMemory = data.GetProperty("freeMemory").GetInt64(),
                FirmwareVersion = data.GetProperty("firmwareVersion").GetString(),
                UptimeMs = data.GetProperty("uptimeMs").GetInt64()
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check device health");
            return new DeviceHealthStatus { IsConnected = false };
        }
    }

    public async Task<bool> SetUserPrivilegeAsync(string enrollmentId, int privilege, DateTime? expiryDate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new
            {
                enrollmentId,
                privilege,
                enableExpiry = expiryDate.HasValue,
                expiryYear = expiryDate?.Year ?? 0,
                expiryMonth = expiryDate?.Month ?? 0,
                expiryDay = expiryDate?.Day ?? 0
            };

            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/SetUserPrivilege",
                new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return data.GetProperty("success").GetBoolean();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to set privilege for {EnrollmentId}", enrollmentId);
            return false;
        }
    }

    public async Task<EnrollmentResult> EnrollFingerprintAsync(string memberId, string enrollmentId, int fingerIndex, int timeoutSeconds = 60, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new { memberId, enrollmentId, fingerIndex, timeoutSeconds };
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/EnrollFingerprint",
                new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new EnrollmentResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" };

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return new EnrollmentResult
            {
                Success = data.GetProperty("success").GetBoolean(),
                ErrorMessage = data.GetProperty("errorMessage").GetString()
            };
        }
        catch (Exception ex)
        {
            return new EnrollmentResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<EnrollmentResult> EnrollFaceAsync(string memberId, string enrollmentId, int timeoutSeconds = 60, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new { memberId, enrollmentId, timeoutSeconds };
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/EnrollFace",
                new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new EnrollmentResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" };

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return new EnrollmentResult
            {
                Success = data.GetProperty("success").GetBoolean(),
                ErrorMessage = data.GetProperty("errorMessage").GetString()
            };
        }
        catch (Exception ex)
        {
            return new EnrollmentResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<bool> DeleteUserAsync(string enrollmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new { enrollmentId };
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/DeleteUser",
                new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode) return false;
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return data.GetProperty("success").GetBoolean();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete user {EnrollmentId}", enrollmentId);
            return false;
        }
    }

    public async Task<DeviceHealthStatus> GetDeviceStatusAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/GetDeviceStatus",
                new StringContent("{}", System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new DeviceHealthStatus { IsConnected = false };

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return new DeviceHealthStatus
            {
                IsConnected = data.GetProperty("isConnected").GetBoolean(),
                EnrolledUserCount = data.GetProperty("enrolledUserCount").GetInt32(),
                FreeMemory = data.GetProperty("freeMemory").GetInt64(),
                FirmwareVersion = data.GetProperty("firmwareVersion").GetString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get device status");
            return new DeviceHealthStatus { IsConnected = false };
        }
    }

    public async Task<TestConnectionResult> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/zkteco.bridge.ZKTecoBridge/TestConnection",
                new StringContent("{}", System.Text.Encoding.UTF8, "application/json"), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new TestConnectionResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" };

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            return new TestConnectionResult
            {
                Success = data.GetProperty("success").GetBoolean(),
                RoundTripLatencyMs = data.GetProperty("roundTripLatencyMs").GetInt64(),
                ErrorMessage = data.GetProperty("errorMessage").GetString()
            };
        }
        catch (Exception ex)
        {
            return new TestConnectionResult { Success = false, ErrorMessage = ex.Message };
        }
    }
}
