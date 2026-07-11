using System.Buffers.Binary;
using HackGym.ZKTeco.Bridge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackGym.ZKTeco.Bridge.Services;

/// <summary>
/// Manages connection to a ZKTeco device using pure TCP sockets.
/// No COM DLL required — compatible with Python pyzk / Node.js node-zklib.
/// </summary>
public class ZKDeviceManager : IDisposable
{
    private readonly ILogger<ZKDeviceManager> _logger;
    private readonly ZKTecoConfig _config;
    private readonly ZKTecoTcpClient _client;
    private bool _disposed;
    private readonly object _lock = new();
    private readonly DeviceConnectionInfo _connectionInfo = new();

    public DeviceConnectionInfo ConnectionInfo => _connectionInfo;
    public bool IsConnected => _connectionInfo.IsConnected;

    public ZKDeviceManager(ILogger<ZKDeviceManager> logger, ILoggerFactory loggerFactory, IOptions<ZKTecoConfig> config)
    {
        _logger = logger;
        _config = config.Value;
        _client = new ZKTecoTcpClient(loggerFactory.CreateLogger<ZKTecoTcpClient>());
    }

    public bool Connect()
    {
        lock (_lock)
        {
            try
            {
                if (_connectionInfo.IsConnected)
                    return true;

                var connected = _client.Connect(_config.DeviceIp, _config.DevicePort, _config.ConnectionTimeoutMs, _config.Password);
                if (!connected)
                {
                    _logger.LogWarning("Failed to connect to device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                    return false;
                }

                _client.RegisterEvent(0xFFFF);

                _connectionInfo.IsConnected = true;
                _connectionInfo.LastConnectedAt = DateTime.UtcNow;
                _connectionInfo.ConsecutiveFailures = 0;
                _connectionInfo.CurrentBackoffDelay = TimeSpan.FromSeconds(10);

                _logger.LogInformation("Connected to ZKTeco device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to device at {Ip}:{Port}", _config.DeviceIp, _config.DevicePort);
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
                if (_connectionInfo.IsConnected)
                {
                    _client.Disconnect();
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
            if (!_connectionInfo.IsConnected)
                return events;

            try
            {
                _client.EnableDevice(false);
                try
                {
                    events = _client.GetAttendanceLogs();
                }
                finally
                {
                    _client.EnableDevice(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading attendance logs");
                _connectionInfo.IsConnected = false;
            }
        }

        return events;
    }

    public bool SetUserPrivilege(string enrollmentId, int privilege, DateTime? expiryDate = null)
    {
        // TCP protocol doesn't directly support privilege setting via simple command
        // This would require CMD_USER_WRQ with full user record
        _logger.LogDebug("SetUserPrivilege not yet implemented for TCP protocol");
        return false;
    }

    public bool DeleteUser(string enrollmentId)
    {
        // Would require CMD_DELETE_USER with the user's uid
        _logger.LogDebug("DeleteUser not yet implemented for TCP protocol");
        return false;
    }

    public (bool Success, string? Error) EnrollFingerprint(string enrollmentId, string name, int fingerIndex, int timeoutSeconds = 60)
    {
        return (false, "Fingerprint enrollment requires the ZKTeco COM SDK");
    }

    public (bool Success, string? Error) EnrollFace(string enrollmentId, string name, int timeoutSeconds = 60)
    {
        return (false, "Face enrollment requires the ZKTeco COM SDK");
    }

    public (int EnrolledCount, long FreeMemory, string? FirmwareVersion) GetDeviceStatus()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return (0, 0, null);

            try
            {
                var sizes = _client.GetFreeSizes();
                var firmware = _client.GetFirmwareVersion();

                _connectionInfo.EnrolledUserCount = sizes.Users;
                _connectionInfo.FreeMemory = sizes.RecCap - sizes.Records;
                _connectionInfo.FirmwareVersion = firmware;

                return (sizes.Users, sizes.RecCap - sizes.Records, firmware);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting device status");
                _connectionInfo.IsConnected = false;
                return (0, 0, null);
            }
        }
    }

    public List<ZKUserInfo> GetAllUsersWithDetails()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return new List<ZKUserInfo>();

            try
            {
                var users = _client.GetUsers();
                _logger.LogInformation("Read {Count} users from device", users.Count);

                // If device has users but we got 0, connection is stale — force reconnect on next cycle
                if (users.Count == 0 && _connectionInfo.EnrolledUserCount > 0)
                {
                    _logger.LogWarning("Device read returned 0 users but expected {Expected}, reconnecting",
                        _connectionInfo.EnrolledUserCount);
                    _connectionInfo.IsConnected = false;
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading users from device");
                _connectionInfo.IsConnected = false;
                return new List<ZKUserInfo>();
            }
        }
    }

    public bool ClearAttendanceLogs()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return false;

            try
            {
                var result = _client.ClearAttendanceLogs();
                if (result)
                    _logger.LogInformation("Cleared attendance logs from device");
                else
                    _logger.LogWarning("Failed to clear attendance logs from device");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing attendance logs");
                _connectionInfo.IsConnected = false;
                return false;
            }
        }
    }

    public ZKDeviceInfo? GetDeviceInfo()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return null;

            try
            {
                var model = _client.GetDeviceModel();
                var serial = _client.GetSerialNumber();
                var firmware = _client.GetFirmwareVersion();
                var sizes = _client.GetFreeSizes();

                _connectionInfo.EnrolledUserCount = sizes.Users;
                _connectionInfo.FreeMemory = sizes.RecCap - sizes.Records;
                _connectionInfo.FirmwareVersion = firmware;

                return new ZKDeviceInfo
                {
                    Model = model,
                    SerialNumber = serial,
                    FirmwareVersion = firmware,
                    EnrolledUserCount = sizes.Users,
                    FreeMemory = sizes.RecCap - sizes.Records
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting device info");
                _connectionInfo.IsConnected = false;
                return null;
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
                var connected = _client.Connect(_config.DeviceIp, _config.DevicePort, _config.ConnectionTimeoutMs, _config.Password);
                sw.Stop();

                if (connected)
                {
                    // Register events after connect to enable real-time attendance push
                    try { _client.RegisterEvent(0xFFFF); } catch { }
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
            if (!_connectionInfo.IsConnected)
                return (ids, false);

            try
            {
                var users = _client.GetUsers();
                return (users.Select(u => u.EnrollmentId).ToList(), true);
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
            _client?.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Raw send/recv diagnostic that shows exact bytes exchanged with the device.
    /// </summary>
    public (string sendHex, ushort recvMagic1, ushort recvMagic2, uint payloadSize, string recvPayloadHex, ushort code, int dataLen, ushort sid, ushort rid) TestRawSendRecv(ushort command, byte[] data)
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return ("not connected", 0, 0, 0, "", 0, 0, 0, 0);
            return _client.RawSendRecv(command, data);
        }
    }

    public object DiagnoseProtocols()
    {
        lock (_lock)
        {
            if (!_connectionInfo.IsConnected)
                return new { connected = false };

            var results = new Dictionary<string, object>();

            // Test 1: CMD_GET_FREE_SIZES (50) with empty data
            try
            {
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(50, Array.Empty<byte>());
                results["get_free_sizes_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["get_free_sizes_error"] = ex.Message; }

            // Test 2: CMD_DB_RRQ (7) with FCT_USER (5) - old protocol
            try
            {
                byte[] data = new byte[4];
                BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(0, 2), 5); // FCT_USER
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(7, data);
                results["db_rrq_user_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["db_rrq_user_error"] = ex.Message; }

            // Test 3: CMD_USERTEMP_RRQ (9) with FCT_USER (5) - like pyzk
            try
            {
                byte[] data = new byte[4];
                BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(0, 2), 5); // FCT_USER
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(9, data);
                results["usertemp_rrq_user_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["usertemp_rrq_user_error"] = ex.Message; }

            // Test 4: CMD_PREPARE_BUFFER (1503) with user read command
            try
            {
                byte[] data = new byte[11];
                data[0] = 1;
                BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(1, 2), 9); // CMD_USERTEMP_RRQ
                BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(3, 4), 5); // FCT_USER
                BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(7, 4), 0); // ext
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(1503, data);
                results["prepare_buffer_user_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["prepare_buffer_user_error"] = ex.Message; }

            // Test 5: CMD_ATTLOG_RRQ (13) with empty data - old attendance protocol
            try
            {
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(13, Array.Empty<byte>());
                results["attlog_rrq_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["attlog_rrq_error"] = ex.Message; }

            // Test 6: CMD_OPTIONS_RRQ (11) with ~DeviceName
            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes("~DeviceName\0");
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(11, data);
                results["device_name_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["device_name_error"] = ex.Message; }

            // Test 7: CMD_OPTIONS_RRQ (11) with ~SerialNumber
            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes("~SerialNumber\0");
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(11, data);
                results["serial_number_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["serial_number_error"] = ex.Message; }

            // Test 8: CMD_GET_VERSION (1100)
            try
            {
                var (code, dataLen, hex, sid, rid) = _client.TestCommandRaw(1100, Array.Empty<byte>());
                results["get_version_raw"] = new { code, dataLen, hex, sessionId = sid, replyId = rid };
            }
            catch (Exception ex) { results["get_version_error"] = ex.Message; }

            return results;
        }
    }
}
