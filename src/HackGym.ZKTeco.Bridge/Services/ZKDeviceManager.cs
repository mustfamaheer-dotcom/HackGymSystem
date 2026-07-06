using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using HackGym.ZKTeco.Bridge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

public class ZKDeviceManager : IDisposable
{
    private readonly ILogger<ZKDeviceManager> _logger;
    private readonly ZKTecoConfig _config;
    private dynamic? _zkem;
    private bool _disposed;
    private readonly object _lock = new();
    private readonly DeviceConnectionInfo _connectionInfo = new();

    public DeviceConnectionInfo ConnectionInfo => _connectionInfo;
    public bool IsConnected => _connectionInfo.IsConnected;

    public ZKDeviceManager(ILogger<ZKDeviceManager> logger, IOptions<ZKTecoConfig> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    public bool Connect()
    {
        lock (_lock)
        {
            try
            {
                if (_connectionInfo.IsConnected)
                    return true;

                _zkem = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00853A19-BD51-419B-9269-2DABE57EB61F"))!);

                var connected = (bool)_zkem.Connect_Net(_config.DeviceIp, _config.DevicePort);
                if (!connected)
                {
                    _logger.LogWarning("Failed to connect to device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                    return false;
                }

                _zkem.RegEvent(1, 65535);
                _connectionInfo.IsConnected = true;
                _connectionInfo.LastConnectedAt = DateTime.UtcNow;
                _connectionInfo.ConsecutiveFailures = 0;
                _connectionInfo.CurrentBackoffDelay = TimeSpan.FromSeconds(10);

                _logger.LogInformation("Connected to ZKTeco device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                return true;
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error connecting to device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                _connectionInfo.IsConnected = false;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error connecting to device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                _connectionInfo.IsConnected = false;
                return false;
            }
        }
    }

    public bool Disconnect()
    {
        lock (_lock)
        {
            try
            {
                if (_zkem is not null && _connectionInfo.IsConnected)
                {
                    _zkem.Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disconnecting from device");
            }
            finally
            {
                _connectionInfo.IsConnected = false;
            }
            return true;
        }
    }

    public List<ZKAttendanceEvent> GetNewLogs()
    {
        var events = new List<ZKAttendanceEvent>();

        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return events;

            try
            {
                var machineNumber = 1;
                _zkem.EnableDevice(machineNumber, false);

                try
                {
                    var dwEnrollNumber = string.Empty;
                    var dwVerifyMode = 0;
                    var dwInOutMode = 0;
                    var dwYear = 0;
                    var dwMonth = 0;
                    var dwDay = 0;
                    var dwHour = 0;
                    var dwMinute = 0;
                    var dwSecond = 0;

                    _zkem.ReadAllGLogData(machineNumber);

                    while (_zkem.SSR_GetGeneralLogData(
                        machineNumber,
                        out dwEnrollNumber,
                        out dwVerifyMode,
                        out dwInOutMode,
                        out dwYear,
                        out dwMonth,
                        out dwDay,
                        out dwHour,
                        out dwMinute,
                        out dwSecond))
                    {
                        events.Add(new ZKAttendanceEvent
                        {
                            EnrollmentId = dwEnrollNumber,
                            Method = (VerifyMethod)dwVerifyMode,
                            Timestamp = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, DateTimeKind.Local).ToUniversalTime(),
                            Direction = dwInOutMode,
                            MachineNumber = machineNumber
                        });
                    }
                }
                finally
                {
                    _zkem.EnableDevice(machineNumber, true);
                }
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error reading attendance logs");
                _connectionInfo.IsConnected = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading attendance logs");
            }
        }

        return events;
    }

    public bool SetUserPrivilege(string enrollmentId, int privilege, DateTime? expiryDate = null)
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return false;

            try
            {
                var machineNumber = 1;
                if (expiryDate.HasValue)
                {
                    return (bool)_zkem.SetUserPrivilege(
                        machineNumber,
                        enrollmentId,
                        privilege,
                        true,
                        expiryDate.Value.Year,
                        expiryDate.Value.Month,
                        expiryDate.Value.Day);
                }

                return (bool)_zkem.SetUserPrivilege(
                    machineNumber,
                    enrollmentId,
                    privilege,
                    false,
                    0, 0, 0);
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error setting user privilege for {EnrollmentId}", enrollmentId);
                return false;
            }
        }
    }

    public bool DeleteUser(string enrollmentId)
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return false;

            try
            {
                return (bool)_zkem.DeleteUserInfo(1, enrollmentId);
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error deleting user {EnrollmentId}", enrollmentId);
                return false;
            }
        }
    }

    public (bool Success, string? Error) EnrollFingerprint(string enrollmentId, string name, int fingerIndex, int timeoutSeconds = 60)
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return (false, "Device not connected");

            try
            {
                var machineNumber = 1;
                _zkem.EnableDevice(machineNumber, false);

                try
                {
                    _zkem.SetUserInfo(machineNumber, enrollmentId, name, "", 0, false);

                    var started = (bool)_zkem.StartEnroll(machineNumber, enrollmentId, fingerIndex);
                    if (!started)
                        return (false, "Failed to start enrollment. Ensure member is at the device.");

                    var tcs = new TaskCompletionSource<bool>();
                    _zkem.OnEnrollComplete += new Action<int, int, int>(OnEnrollCompleteHandler);
                    // Poll for completion
                    var enrolled = WaitForEnrollment(enrollmentId, fingerIndex, timeoutSeconds);
                    _zkem.OnEnrollComplete -= new Action<int, int, int>(OnEnrollCompleteHandler);

                    if (!enrolled)
                        return (false, "Enrollment timed out or failed");

                    return (true, null);
                }
                finally
                {
                    _zkem.EnableDevice(machineNumber, true);
                }
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error during fingerprint enrollment for {EnrollmentId}", enrollmentId);
                return (false, $"Device error: {ex.Message}");
            }
        }
    }

    public (bool Success, string? Error) EnrollFace(string enrollmentId, string name, int timeoutSeconds = 60)
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return (false, "Device not connected");

            try
            {
                var machineNumber = 1;
                _zkem.EnableDevice(machineNumber, false);

                try
                {
                    _zkem.SetUserInfo(machineNumber, enrollmentId, name, "", 0, false);

                    var started = (bool)_zkem.StartFaceEnroll(machineNumber, enrollmentId);
                    if (!started)
                        return (false, "Failed to start face enrollment. Ensure member is at the device.");

                    var enrolled = WaitForFaceEnrollment(enrollmentId, timeoutSeconds);
                    if (!enrolled)
                        return (false, "Face enrollment timed out or failed");

                    return (true, null);
                }
                finally
                {
                    _zkem.EnableDevice(machineNumber, true);
                }
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error during face enrollment for {EnrollmentId}", enrollmentId);
                return (false, $"Device error: {ex.Message}");
            }
        }
    }

    private void OnEnrollCompleteHandler(int machineNumber, int enrollId, int errorCode)
    {
        _logger.LogInformation("Enrollment complete callback: Machine={Machine}, EnrollId={EnrollId}, Error={Error}",
            machineNumber, enrollId, errorCode);
    }

    private bool WaitForEnrollment(string enrollmentId, int fingerIndex, int timeoutSeconds)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var enrolled = (bool)_zkem.CheckEnrolled(1, enrollmentId, fingerIndex);
                if (enrolled)
                    return true;

                Thread.Sleep(500);
            }
        }
        catch { }
        return false;
    }

    private bool WaitForFaceEnrollment(string enrollmentId, int timeoutSeconds)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                _zkem.GetUserFace(1, enrollmentId, out object faceData);
                if (faceData is not null && faceData.ToString()?.Length > 0)
                    return true;

                Thread.Sleep(500);
            }
        }
        catch { }
        return false;
    }

    public (int EnrolledCount, long FreeMemory, string? FirmwareVersion) GetDeviceStatus()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return (0, 0, null);

            try
            {
                var machineNumber = 1;
                var enrolledCount = (int)_zkem.GetDeviceStatus(machineNumber, 1);
                var freeMemory = (long)(int)_zkem.GetDeviceStatus(machineNumber, 2);
                var firmware = (string)_zkem.GetFirmwareVersion(machineNumber);

                _connectionInfo.EnrolledUserCount = enrolledCount;
                _connectionInfo.FreeMemory = freeMemory;
                _connectionInfo.FirmwareVersion = firmware;

                return (enrolledCount, freeMemory, firmware);
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "COM error getting device status");
                _connectionInfo.IsConnected = false;
                return (0, 0, null);
            }
        }
    }

    public (bool Success, long LatencyMs) TestConnection()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        lock (_lock)
        {
            if (_connectionInfo.IsConnected)
            {
                Disconnect();
            }

            try
            {
                _zkem = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00853A19-BD51-419B-9269-2DABE57EB61F"))!);
                var connected = (bool)_zkem.Connect_Net(_config.DeviceIp, _config.DevicePort);
                sw.Stop();

                if (connected)
                {
                    _zkem.RegEvent(1, 65535);
                    _connectionInfo.IsConnected = true;
                    _connectionInfo.LastConnectedAt = DateTime.UtcNow;
                    _connectionInfo.ConsecutiveFailures = 0;
                }

                return (connected, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "Connection test failed");
                return (false, sw.ElapsedMilliseconds);
            }
        }
    }

    public (List<string> EnrolledIds, bool Success) GetAllUserIds()
    {
        var ids = new List<string>();
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected || _zkem is null)
                return (ids, false);

            try
            {
                _zkem.ReadAllUserID(1);
                var total = (int)_zkem.GetDeviceStatus(1, 1);

                string enrollId = string.Empty;
                string name = string.Empty;
                string password = string.Empty;
                int privilege = 0;
                bool enabled = false;

                for (int i = 0; i < total; i++)
                {
                    if ((bool)_zkem.SSR_GetAllUserInfo(1, out enrollId, out name, out password, out privilege, out enabled))
                    {
                        ids.Add(enrollId);
                    }
                }

                return (ids, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading all user IDs");
                return (ids, false);
            }
        }
    }

    public void RecordFailure()
    {
        _connectionInfo.ConsecutiveFailures++;
        _connectionInfo.CurrentBackoffDelay = TimeSpan.FromSeconds(
            Math.Min(300, Math.Pow(2, Math.Min(_connectionInfo.ConsecutiveFailures, 8)) * 10));
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Disconnect();
            if (_zkem is not null)
            {
                try
                {
                    Marshal.ReleaseComObject(_zkem);
                }
                catch { }
                _zkem = null;
            }
            _disposed = true;
        }
    }
}
