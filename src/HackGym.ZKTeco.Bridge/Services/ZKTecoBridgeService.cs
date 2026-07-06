using Grpc.Core;
using Microsoft.Extensions.Logging;
using Zkteco;

namespace HackGym.ZKTeco.Bridge.Services;

public class ZKTecoBridgeService : ZKTecoBridge.ZKTecoBridgeBase
{
    private readonly ZKDeviceManager _deviceManager;
    private readonly ILogger<ZKTecoBridgeService> _logger;

    public ZKTecoBridgeService(ZKDeviceManager deviceManager, ILogger<ZKTecoBridgeService> logger)
    {
        _deviceManager = deviceManager;
        _logger = logger;
    }

    public override Task<HealthResponse> CheckHealth(HealthRequest request, ServerCallContext context)
    {
        var (count, memory, firmware) = _deviceManager.IsConnected
            ? _deviceManager.GetDeviceStatus()
            : (0, 0L, null);

        return Task.FromResult(new HealthResponse
        {
            IsConnected = _deviceManager.IsConnected,
            EnrolledUserCount = count,
            FreeMemory = memory,
            FirmwareVersion = firmware ?? string.Empty,
            UptimeMs = _deviceManager.IsConnected
                ? (long)(DateTime.UtcNow - _deviceManager.ConnectionInfo.LastConnectedAt).TotalMilliseconds
                : 0
        });
    }

    public override Task<PrivilegeResponse> SetUserPrivilege(SetUserPrivilegeRequest request, ServerCallContext context)
    {
        if (!_deviceManager.IsConnected)
        {
            return Task.FromResult(new PrivilegeResponse
            {
                Success = false,
                ErrorMessage = "Device not connected"
            });
        }

        DateTime? expiry = null;
        if (request.EnableExpiry)
        {
            try
            {
                expiry = new DateTime(request.ExpiryYear, request.ExpiryMonth, request.ExpiryDay, 23, 59, 59, DateTimeKind.Local);
            }
            catch
            {
                return Task.FromResult(new PrivilegeResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid expiry date"
                });
            }
        }

        var success = _deviceManager.SetUserPrivilege(request.EnrollmentId, request.Privilege, expiry);
        return Task.FromResult(new PrivilegeResponse
        {
            Success = success,
            ErrorMessage = success ? string.Empty : "Failed to set privilege on device"
        });
    }

    public override Task<EnrollResponse> EnrollFingerprint(EnrollFingerprintRequest request, ServerCallContext context)
    {
        var (success, error) = _deviceManager.EnrollFingerprint(
            request.EnrollmentId,
            $"Member_{request.MemberId}",
            request.FingerIndex,
            request.TimeoutSeconds > 0 ? request.TimeoutSeconds : 60);

        return Task.FromResult(new EnrollResponse
        {
            Success = success,
            ErrorMessage = error ?? string.Empty
        });
    }

    public override Task<EnrollResponse> EnrollFace(EnrollFaceRequest request, ServerCallContext context)
    {
        var (success, error) = _deviceManager.EnrollFace(
            request.EnrollmentId,
            $"Member_{request.MemberId}",
            request.TimeoutSeconds > 0 ? request.TimeoutSeconds : 60);

        return Task.FromResult(new EnrollResponse
        {
            Success = success,
            ErrorMessage = error ?? string.Empty
        });
    }

    public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var success = _deviceManager.DeleteUser(request.EnrollmentId);
        return Task.FromResult(new DeleteUserResponse
        {
            Success = success,
            ErrorMessage = success ? string.Empty : "Failed to delete user from device"
        });
    }

    public override Task<DeviceStatusResponse> GetDeviceStatus(DeviceStatusRequest request, ServerCallContext context)
    {
        var (count, memory, firmware) = _deviceManager.IsConnected
            ? _deviceManager.GetDeviceStatus()
            : (0, 0L, null);

        return Task.FromResult(new DeviceStatusResponse
        {
            IsConnected = _deviceManager.IsConnected,
            EnrolledUserCount = count,
            FreeMemory = memory,
            FirmwareVersion = firmware ?? string.Empty,
            MaxFingerprints = 3000,
            MaxFaces = 500
        });
    }

    public override Task<ReconcileResponse> ReconcileUsers(ReconcileRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Starting user reconciliation");
        var response = new ReconcileResponse { Success = true };

        try
        {
            var (deviceIds, success) = _deviceManager.GetAllUserIds();
            if (!success)
            {
                response.Success = false;
                response.Details.Add("Failed to read users from device");
                return Task.FromResult(response);
            }

            response.UsersChecked = deviceIds.Count;
            response.Success = true;
            _logger.LogInformation("Reconciliation complete: {Count} users checked, {Fixed} discrepancies fixed",
                response.UsersChecked, response.DiscrepanciesFixed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reconciliation failed");
            response.Success = false;
            response.Details.Add($"Error: {ex.Message}");
        }

        return Task.FromResult(response);
    }

    public override Task<TestConnectionResponse> TestConnection(TestConnectionRequest request, ServerCallContext context)
    {
        var (success, latencyMs) = _deviceManager.TestConnection();
        return Task.FromResult(new TestConnectionResponse
        {
            Success = success,
            RoundTripLatencyMs = latencyMs,
            ErrorMessage = success ? string.Empty : "Connection test failed"
        });
    }
}
