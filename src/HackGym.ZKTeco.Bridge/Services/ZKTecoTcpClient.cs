using System.Buffers.Binary;
using System.Net.Sockets;
using HackGym.ZKTeco.Bridge.Models;
using HackGym.ZKTeco.Bridge.Protocol;
using Microsoft.Extensions.Logging;

namespace HackGym.ZKTeco.Bridge.Services;

/// <summary>
/// Pure TCP socket client for ZKTeco devices.
/// Implements the ZK binary protocol — no COM DLL required.
/// Compatible with Python pyzk / Node.js node-zklib.
/// </summary>
public class ZKTecoTcpClient : IDisposable
{
    private readonly ILogger<ZKTecoTcpClient> _logger;
    private TcpClient? _tcp;
    private NetworkStream? _stream;
    private ushort _sessionId;
    private ushort _replyId;
    private bool _disposed;

    // Stored connection parameters so we can self-recover from a protocol state desync.
    private string? _lastIp;
    private int _lastPort;
    private int _lastTimeoutMs;
    private int _lastPassword;

    public bool IsConnected { get; private set; }

    public ZKTecoTcpClient(ILogger<ZKTecoTcpClient> logger)
    {
        _logger = logger;
        _replyId = ZKProtocol.USHRT_MAX - 1;
    }

    /// <summary>
    /// Connect to the device via TCP. Handles CMD_CONNECT + CMD_AUTH if needed.
    /// </summary>
    public bool Connect(string ip, int port, int timeoutMs = 5000, int password = 0)
    {
        Disconnect();

        try
        {
            // Capture parameters so we can reconnect on a protocol state desync.
            _lastIp = ip;
            _lastPort = port;
            _lastTimeoutMs = timeoutMs;
            _lastPassword = password;

            _tcp = new TcpClient();
            _tcp.ReceiveTimeout = timeoutMs;
            _tcp.SendTimeout = timeoutMs;

            var result = _tcp.BeginConnect(ip, port, null, null);
            bool connected = result.AsyncWaitHandle.WaitOne(timeoutMs);

            if (!connected || !_tcp.Connected)
            {
                _logger.LogWarning("TCP connection to {Ip}:{Port} failed", ip, port);
                _tcp.Close();
                return false;
            }

            _tcp.EndConnect(result);
            _stream = _tcp.GetStream();
            _stream.ReadTimeout = timeoutMs;
            _stream.WriteTimeout = timeoutMs;

            // Drain any initial packet the device might send on connect
            // (some ZK devices send an unsolicited "hello" packet)
            if (_stream.DataAvailable)
            {
                byte[] initial = new byte[8192];
                int drained = _stream.Read(initial, 0, 8192);
                _logger.LogDebug("Drained {Count} bytes of initial device data", drained);
            }

            _sessionId = 0;
            _replyId = ZKProtocol.USHRT_MAX - 1;

            // Send CMD_CONNECT
            var resp = SendCommand(ZKProtocol.CMD_CONNECT, Array.Empty<byte>());

            // pyzk: self.__session_id = self.__header[2] — reads session_id from CMD_CONNECT response
            _sessionId = resp.SessionId;

            if (resp.Code == ZKProtocol.CMD_ACK_UNAUTH)
            {
                // Device requires authentication
                byte[] commKey = ZKProtocol.MakeCommKey(password, _sessionId);
                resp = SendCommand(ZKProtocol.CMD_AUTH, commKey);
            }

            if (resp.Code == ZKProtocol.CMD_ACK_OK)
            {
                IsConnected = true;
                _logger.LogInformation("Connected to ZKTeco device at {Ip}:{Port} (session={Session})", ip, port, _sessionId);
                return true;
            }

            _logger.LogWarning("Device rejected connection: code={Code}", resp.Code);
            Disconnect();
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error connecting to {Ip}:{Port}", ip, port);
            Disconnect();
            return false;
        }
    }

    /// <summary>
    /// Force a disconnect + reconnect using the parameters from the last successful
    /// <see cref="Connect"/>. Used to recover from a protocol state desync (e.g. after
    /// repeated empty attendance reads) where the device stops responding normally.
    /// </summary>
    private bool ForceReconnect()
    {
        _logger.LogWarning("Forcing device reconnect to recover from protocol state desync");
        try { Disconnect(); } catch { }
        if (_lastIp == null) return false;
        return Connect(_lastIp, _lastPort, _lastTimeoutMs, _lastPassword);
    }

    /// <summary>
    /// Disconnect from the device.
    /// </summary>
    public void Disconnect()
    {
        try
        {
            if (IsConnected && _stream != null)
            {
                try { SendCommand(ZKProtocol.CMD_EXIT, Array.Empty<byte>()); } catch { }
            }
        }
        catch { }
        finally
        {
            IsConnected = false;
            _stream?.Dispose();
            _tcp?.Dispose();
            _stream = null;
            _tcp = null;
        }
    }

    /// <summary>
    /// Enable or disable the device (lock/unlock UI).
    /// </summary>
    public bool EnableDevice(bool enable)
    {
        if (!IsConnected) return false;
        ushort cmd = enable ? ZKProtocol.CMD_ENABLEDEVICE : ZKProtocol.CMD_DISABLEDEVICE;
        var resp = SendCommand(cmd, Array.Empty<byte>());
        return resp.Code == ZKProtocol.CMD_ACK_OK;
    }

    /// <summary>
    /// Get device free sizes: users, records, faces, etc.
    /// </summary>
    public (int Users, int Records, int Faces, int UsersCap, int RecCap, int FacesCap) GetFreeSizes()
    {
        if (!IsConnected) return (0, 0, 0, 0, 0, 0);

        var resp = SendCommand(ZKProtocol.CMD_GET_FREE_SIZES, Array.Empty<byte>());
        if (!resp.IsOk || resp.Data.Length < 80)
            return (0, 0, 0, 0, 0, 0);

        // Unpack 20 int32 fields from first 80 bytes
        int[] fields = new int[20];
        for (int i = 0; i < 20; i++)
            fields[i] = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(i * 4, 4));

        int users = fields[4];
        int records = fields[8];
        int usersCap = fields[15];
        int recCap = fields[16];

        int faces = 0, facesCap = 0;
        if (resp.Data.Length >= 92)
        {
            faces = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(80, 4));
            facesCap = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(88, 4));
        }

        return (users, records, faces, usersCap, recCap, facesCap);
    }

    /// <summary>
    /// Get firmware version string.
    /// </summary>
    public string GetFirmwareVersion()
    {
        if (!IsConnected) return string.Empty;
        var resp = SendCommand(ZKProtocol.CMD_GET_VERSION, Array.Empty<byte>());
        if (resp.IsOk && resp.Data.Length > 0)
        {
            int nullIdx = Array.IndexOf(resp.Data, (byte)0);
            return System.Text.Encoding.ASCII.GetString(resp.Data, 0, nullIdx > 0 ? nullIdx : resp.Data.Length).Trim();
        }
        return string.Empty;
    }

    /// <summary>
    /// Get serial number via CMD_OPTIONS_RRQ.
    /// </summary>
    public string GetSerialNumber()
    {
        return GetOption("~SerialNumber");
    }

    /// <summary>
    /// Get device model/platform name.
    /// </summary>
    public string GetDeviceModel()
    {
        return GetOption("~DeviceName");
    }

    private string GetOption(string optionName)
    {
        if (!IsConnected) return string.Empty;
        byte[] data = System.Text.Encoding.ASCII.GetBytes(optionName + "\0");
        var resp = SendCommand(ZKProtocol.CMD_OPTIONS_RRQ, data);
        if (resp.IsOk && resp.Data.Length > 0)
        {
            var str = System.Text.Encoding.ASCII.GetString(resp.Data);
            // Format: "OptionName=Value\0"
            int eqIdx = str.IndexOf('=');
            if (eqIdx >= 0)
            {
                int nullIdx = str.IndexOf('\0', eqIdx);
                return str.Substring(eqIdx + 1, (nullIdx > eqIdx ? nullIdx : str.Length) - eqIdx - 1).Trim();
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Get all users from the device.
    /// Uses CMD_DB_RRQ + FCT_USER with buffer-based transfer.
    /// </summary>
    public List<ZKUserInfo> GetUsers()
    {
        var users = new List<ZKUserInfo>();
        if (!IsConnected) return users;

        var sizes = GetFreeSizes();
        _logger.LogWarning("GetFreeSizes result: Users={Users}, Records={Records}, Faces={Faces}, UsersCap={UsersCap}, RecCap={RecCap}, FacesCap={FacesCap}",
            sizes.Users, sizes.Records, sizes.Faces, sizes.UsersCap, sizes.RecCap, sizes.FacesCap);

        byte[] userdata = ReadUsersDirect();
        _logger.LogWarning("ReadUsersDirect for users returned {Len} bytes, first 100 hex: {Hex}",
            userdata.Length, userdata.Length > 0 ? Convert.ToHexString(userdata.AsSpan(0, Math.Min(100, userdata.Length))) : "(empty)");
        if (userdata.Length <= 8) return users;

        // The data begins with an 8-byte header: [maxChunkSize(4)][totalSize(4)].
        // Detect and strip it so records start at offset 8. (ReadUsersDirect may or may not
        // have already stripped it depending on the response path, so detect explicitly.)
        int totalSize = BinaryPrimitives.ReadInt32LittleEndian(userdata.AsSpan(4, 4));
        if (totalSize == userdata.Length - 8 || Math.Abs(totalSize - (userdata.Length - 8)) < 72)
        {
            userdata = userdata[8..];
        }
        else
        {
            int totalSize4 = BinaryPrimitives.ReadInt32LittleEndian(userdata.AsSpan(0, 4));
            if (totalSize4 == userdata.Length - 4)
                userdata = userdata[4..];
        }

        int packetSize = sizes.Users > 0 ? totalSize / sizes.Users : 0;

        // If packet size is unknown (sizes.Users==0), try to auto-detect
        if (packetSize != 72 && packetSize != 28)
        {
            if (userdata.Length >= 72 && (userdata.Length % 72 == 0 || userdata.Length >= 72 * 10))
                packetSize = 72;
            else if (userdata.Length >= 28 && (userdata.Length % 28 == 0 || userdata.Length >= 28 * 10))
                packetSize = 28;
            else if (userdata.Length >= 72)
                packetSize = 72; // default to TCP 72-byte for face devices
            _logger.LogDebug("Auto-detected packetSize={PacketSize} for dataLen={DataLen}", packetSize, userdata.Length);
        }

        if (packetSize == 72)
        {
            // TCP 72-byte user record
            while (userdata.Length >= 72)
            {
                ushort uid = BinaryPrimitives.ReadUInt16LittleEndian(userdata.AsSpan(0, 2));
                byte privilege = userdata[2];
                string password = ReadNullTerminated(userdata, 3, 8).Trim();
                string name = ReadNullTerminated(userdata, 11, 24).Trim();
                // card at 35 (4 bytes) - skip
                string groupId = ReadNullTerminated(userdata, 40, 7).Trim();
                string userId = ReadNullTerminated(userdata, 48, 24).Trim();

                // Some devices (e.g. MB2000 face terminals) store the enrollment/employee
                // ID in the 8-byte password field rather than the 24-byte userId field.
                // Prefer the standard userId field when it looks like a real ID, otherwise
                // fall back to the password field so attendance can be matched.
                string enrollmentId = userId;
                bool userIdFieldValid = !string.IsNullOrWhiteSpace(userId)
                    && userId.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
                if (!userIdFieldValid && !string.IsNullOrWhiteSpace(password))
                    enrollmentId = password;

                if (string.IsNullOrEmpty(name))
                    name = $"NN-{enrollmentId}";

                users.Add(new ZKUserInfo
                {
                    EnrollmentId = enrollmentId,
                    Name = name,
                    Privilege = privilege,
                    Enabled = true
                });

                userdata = userdata[72..];
            }
        }
        else if (packetSize == 28)
        {
            // UDP 28-byte user record
            while (userdata.Length >= 28)
            {
                ushort uid = BinaryPrimitives.ReadUInt16LittleEndian(userdata.AsSpan(0, 2));
                byte privilege = userdata[2];
                string password = ReadNullTerminated(userdata, 3, 5);
                string name = ReadNullTerminated(userdata, 8, 8).Trim();
                uint card = BitConverter.ToUInt32(userdata, 16);
                byte groupId = userdata[21];
                ushort userIdInt = BinaryPrimitives.ReadUInt16LittleEndian(userdata.AsSpan(24, 2));
                string userId = userIdInt.ToString();

                if (string.IsNullOrEmpty(name))
                    name = $"NN-{userId}";

                users.Add(new ZKUserInfo
                {
                    EnrollmentId = userId,
                    Name = name,
                    Privilege = privilege,
                    Enabled = true
                });

                userdata = userdata[28..];
            }
        }

        _logger.LogInformation("Read {Count} users from device", users.Count);
        for (int i = 0; i < Math.Min(5, users.Count); i++)
            _logger.LogWarning("PARSE user[{I}]: EnrollmentId='{Eid}' Name='{Name}' Priv={P}", i, users[i].EnrollmentId, users[i].Name, users[i].Privilege);
        return users;
    }

    /// <summary>
    /// Get attendance logs from the device.
    /// Tries CMD_PREPARE_BUFFER protocol first, then falls back to classic CMD_ATTLOG_RRQ direct protocol.
    /// </summary>
    public List<ZKAttendanceEvent> GetAttendanceLogs()
    {
        var logs = new List<ZKAttendanceEvent>();
        if (!IsConnected) return logs;

        byte[] data = ReadAttendanceDirect();

        // The first 4 bytes are a count header; records follow (40 bytes each).
        if (data.Length < 4 + ZKProtocol.ATT_LOG_TCP_RECORD) return logs;

        int offset = 4;
        while (offset + ZKProtocol.ATT_LOG_TCP_RECORD <= data.Length)
        {
            try
            {
                // 40-byte attendance record (MB2000 / FW 6.60 TCP):
                //   uid(2)@0, deviceUserId string(9)@2, recordTime uint32@27
                int uid = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));
                string userId = System.Text.Encoding.ASCII
                    .GetString(data, offset + 2, 9)
                    .TrimEnd('\0').Trim();

                byte[] timeBytes = data[(offset + 27)..(offset + 31)];
                DateTime timestamp = ZKProtocol.DecodeTime(timeBytes);

                logs.Add(new ZKAttendanceEvent
                {
                    EnrollmentId = string.IsNullOrWhiteSpace(userId) ? uid.ToString() : userId,
                    Method = MapVerifyMethod(0),
                    Timestamp = timestamp.ToUniversalTime(),
                    Direction = 0,
                    MachineNumber = 1
                });

                offset += ZKProtocol.ATT_LOG_TCP_RECORD;
            }
            catch
            {
                offset++;
            }
        }

        _logger.LogInformation("Read {Count} attendance logs from device", logs.Count);
        for (int i = 0; i < Math.Min(5, logs.Count); i++)
            _logger.LogWarning("PARSE att[{I}]: EnrollmentId='{Eid}' Time={T} uid={U}", i, logs[i].EnrollmentId, logs[i].Timestamp, logs[i].MachineNumber);
        return logs;
    }

    /// <summary>
    /// Attendance read using the buffered-transfer protocol that works on this device
    /// (MB2000 / FW 6.60). Request data with CMD_DATA_WRRQ, which returns CMD_PREPARE_DATA
    /// with the total size; then pull each chunk via CMD_DATA_RDY carrying [start(4)][size(4)].
    /// Returns the raw record payload (with the 4-byte count header still in front — the
    /// caller strips it before parsing 40-byte records).
    /// </summary>
    private byte[] ReadAttendanceDirect()
    {
        // Reset the device state before a bulk read. Disabling the device stops it from
        // accepting new check-ins and clears any half-open buffer transfer — this is the
        // standard pyzk pattern and prevents the "first read works, subsequent reads return
        // 0 bytes" protocol state desync. The device is re-enabled in the finally block.
        try { EnableDevice(false); } catch { }
        try { FreeData(); } catch { }

        try
                {
                    // STRATEGY 1: Try direct CMD_ATTLOG_RRQ (command 13) — simpler protocol,
                    // works better on newer firmware (6.x). Returns logs directly without
                    // the buffered transfer handshake.
                    _logger.LogInformation("ReadAttendanceDirect: trying CMD_ATTLOG_RRQ (direct)");
                    var resp = SendCommand(ZKProtocol.CMD_ATTLOG_RRQ, Array.Empty<byte>());
                    _logger.LogWarning("ReadAttendanceDirect: CMD_ATTLOG_RRQ resp.Code={Code} dataLen={Len}", resp.Code, resp.Data.Length);

                    // Success with data — parse it (skip 8-byte header)
                    if (resp.Code == ZKProtocol.CMD_ACK_OK && resp.Data.Length > 8)
                    {
                        var records = resp.Data[8..];
                        _logger.LogInformation("ReadAttendanceDirect: CMD_ATTLOG_RRQ returned {Count} bytes", records.Length);
                        FreeData();
                        return records;
                    }

                    // Empty or error response — fall through to STRATEGY 2 (buffered protocol)
                    _logger.LogWarning("ReadAttendanceDirect: CMD_ATTLOG_RRQ returned empty (Code={Code}), trying CMD_DB_RRQ+FCT_ATTLOG", resp.Code);
            
                    // STRATEGY 2: Buffered transfer protocol (fallback)
                    byte[] fctData = new byte[4];
                    BinaryPrimitives.WriteUInt16LittleEndian(fctData.AsSpan(0, 2), ZKProtocol.FCT_ATTLOG);
                    resp = SendCommand(ZKProtocol.CMD_DB_RRQ, fctData);
                    _logger.LogWarning("ReadAttendanceDirect: CMD_DB_RRQ+FCT_ATTLOG resp.Code={Code} dataLen={Len}", resp.Code, resp.Data.Length);

                    // No new logs: device read pointer is at the end of available data.
                    // REMOVED auto-clear: Never clear attendance logs from here — that decision
                    // belongs to the AttendancePollingWorker which tracks whether events were
                    // successfully pushed to the API. Clearing here destroys data permanently.
                        if (resp.Code == ZKProtocol.CMD_ACK_OK && resp.Data.Length == 0)
                        {
                            _logger.LogDebug(
                                "ReadAttendanceDirect: no new attendance logs (ACK_OK, dataLen=0)");
                            FreeData();
                            return Array.Empty<byte>();
                        }

            // We actually got data.
            if (resp.Code == ZKProtocol.CMD_PREPARE_DATA && resp.Data.Length >= 4)
            {
                int totalSize = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(0, 4));
                int maxChunk = (resp.Data.Length >= 8)
                    ? BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(4, 4))
                    : ZKProtocol.MAX_CHUNK;
                if (maxChunk <= 0 || maxChunk > ZKProtocol.MAX_CHUNK) maxChunk = ZKProtocol.MAX_CHUNK;
                _logger.LogWarning("ATT PREPARE_DATA totalSize={Total} maxChunk={Max}", totalSize, maxChunk);

                // Pull the data: send the chunk request WITHOUT draining the stream
                // (the device may have already pushed the first frames unsolicited right
                // after PREPARE_DATA — draining would lose them). Then read every frame
                // the device sends and collect any that carry payload.
                using var ms = new MemoryStream();
                int guard = 0;
                byte[] req = new byte[8];
                BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(0, 4), 0);
                BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(4, 4), totalSize);
                SendRawNoDrain(ZKProtocol.CMD_DATA_RDY, req);

                while (guard++ < 16)
                {
                    var f = ReadCommandResponse();
                    string hex = f.Data.Length > 0
                        ? BitConverter.ToString(f.Data, 0, Math.Min(f.Data.Length, 48))
                        : "-";
                    _logger.LogWarning("ATT capture frame#{N} code={Code} len={Len} hex={Hex}", guard, f.Code, f.Data.Length, hex);
                    if (f.Data.Length > 0)
                    {
                        ms.Write(f.Data, 0, f.Data.Length);
                    }
                    else if (f.Code == ZKProtocol.CMD_ACK_OK && (ms.Length > 0 || guard > 3))
                    {
                        break;
                    }
                    if (ms.Length >= totalSize) break;
                }
                FreeData();
                return ms.ToArray();
            }

            // Some devices return the data inline (no PREPARE_DATA).
            if (resp.Code == ZKProtocol.CMD_DATA && resp.Data.Length > 8)
            {
                var records = resp.Data[8..];
                FreeData();
                return records;
            }

            _logger.LogWarning("ReadAttendanceDirect: unsupported resp code={Code}", resp.Code);
            FreeData();
            return Array.Empty<byte>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReadAttendanceDirect error");
            try { FreeData(); } catch { }
            return Array.Empty<byte>();
        }
        finally
        {
            // Re-enable the device so it can resume normal check-in operation.
            try { EnableDevice(true); } catch { }
        }
    }

    private byte[] ReadUsersDirect()
    {
        try
        {
            byte[] fctData = new byte[4];
            BinaryPrimitives.WriteUInt16LittleEndian(fctData.AsSpan(0, 2), ZKProtocol.FCT_USER);

            var resp = SendCommand(ZKProtocol.CMD_USERTEMP_RRQ, fctData);
            _logger.LogWarning("ReadUsersDirect: CMD_USERTEMP_RRQ resp.Code={Code} dataLen={Len} (CMD_DATA={Cd})", resp.Code, resp.Data.Length, ZKProtocol.CMD_DATA);

            if (resp.Code == ZKProtocol.CMD_DATA && resp.Data.Length > 8)
            {
                // CMD_DATA inline response includes a small header: [maxChunkSize(4)][totalSize(4)]
                // Strip it and return raw user records
                var records = resp.Data[8..];
                _logger.LogDebug("ReadUsersDirect: CMD_USERTEMP_RRQ returned {Total} bytes ({Rec} records)",
                    records.Length, records.Length / 72);
                FreeData();
                return records;
            }

            // Fallback: CMD_DB_RRQ (7) with FCT_USER
            resp = SendCommand(ZKProtocol.CMD_DB_RRQ, fctData);

            if (resp.Code == ZKProtocol.CMD_DATA && resp.Data.Length > 4)
            {
                var records = resp.Data.Length > 8 ? resp.Data[8..] : resp.Data;
                _logger.LogDebug("ReadUsersDirect: CMD_DB_RRQ returned {Total} bytes", records.Length);
                FreeData();
                return records;
            }

            if (resp.Code != ZKProtocol.CMD_PREPARE_DATA || resp.Data.Length < 4)
            {
                _logger.LogWarning("ReadUsersDirect: both protocols failed, last code={Code}, dataLen={DataLen}", resp.Code, resp.Data.Length);
                return Array.Empty<byte>();
            }

            int totalSize = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(0, 4));
            _logger.LogDebug("ReadUsersDirect: CMD_DB_RRQ totalSize={Size}, reading chunks", totalSize);

            using var ms = new MemoryStream();
            if (resp.Data.Length > 4)
                ms.Write(resp.Data, 4, resp.Data.Length - 4);

            while (ms.Length < totalSize)
            {
                byte[] offsetData = new byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(offsetData, (int)ms.Length);
                var chunkResp = SendCommand(ZKProtocol.CMD_DATA, offsetData);

                if (chunkResp.Data.Length == 0)
                {
                    _logger.LogWarning("ReadUsersDirect: empty chunk at offset={Offset}", ms.Length);
                    break;
                }

                ms.Write(chunkResp.Data, 0, chunkResp.Data.Length);
            }

            byte[] result = ms.ToArray();
            _logger.LogDebug("ReadUsersDirect: completed, total {Total} bytes", result.Length);
            FreeData();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReadUsersDirect error");
            return Array.Empty<byte>();
        }
    }

    /// <summary>
    /// Clear all attendance logs from the device.
    /// Sends CMD_CLEAR_ATTLOG (14) with the current session id and checksum, validates
    /// the response code, and logs the raw hex on failure for diagnostics.
    /// </summary>
    public bool ClearAttendanceLogs()
    {
        if (!IsConnected) return false;

        // Disable the device while clearing so no check-in races the clear operation.
        try { EnableDevice(false); } catch { }

        try
        {
            var resp = SendCommand(ZKProtocol.CMD_CLEAR_ATTLOG, Array.Empty<byte>());
            bool ok = resp.Code == ZKProtocol.CMD_ACK_OK;

            if (ok)
            {
                _logger.LogInformation("Cleared attendance logs from device (code={Code})", resp.Code);
            }
            else
            {
                string hex = resp.Data.Length > 0
                    ? Convert.ToHexString(resp.Data.AsSpan(0, Math.Min(resp.Data.Length, 64)))
                    : "(empty)";
                _logger.LogWarning(
                    "ClearAttendanceLogs failed: code={Code} (0x{CodeHex}) IsOk={IsOk} dataLen={Len} hex={Hex}",
                    resp.Code, resp.Code.ToString("X4"), resp.IsOk, resp.Data.Length, hex);
            }

            // Always free any buffer the device may have opened for the command.
            try { FreeData(); } catch { }
            return ok;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ClearAttendanceLogs error");
            return false;
        }
        finally
        {
            try { EnableDevice(true); } catch { }
        }
    }

    /// <summary>
    /// Register for real-time events.
    /// </summary>
    public bool RegisterEvent(int eventFlags)
    {
        if (!IsConnected) return false;
        byte[] data = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(data, eventFlags);
        var resp = SendCommand(ZKProtocol.CMD_REG_EVENT, data);
        return resp.IsOk;
    }

    /// <summary>
    /// Read data from the device using the pyzk buffer protocol.
    /// Sends CMD_PREPARE_BUFFER (1503) to initiate, then reads chunks via CMD_READ_BUFFER (1504),
    /// then frees data with CMD_FREE_DATA (1502).
    /// Matches pyzk's read_with_buffer() implementation exactly.
    /// </summary>
    /// <param name="command">The data command (e.g., CMD_USERTEMP_RRQ=9, CMD_DB_RRQ=7, CMD_ATTLOG_RRQ=13)</param>
    /// <param name="functionCode">Function code (e.g., FCT_USER=5, FCT_FINGERTMP=2). 0 for commands that don't use it.</param>
    /// <param name="ext">Extension parameter. Always 0.</param>
    private byte[] ReadWithBuffer(ushort command, int functionCode = 0, int ext = 0)
    {
        const int MAX_CHUNK = 0xFFC0; // 65472 bytes, matches pyzk TCP

        try
        {
            const int maxRetries = 3;
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                // Free any previous buffer before starting new read
                FreeData();

                // pyzk: command_string = pack('<bhii', 1, command, fct, ext) = 11 bytes
                byte[] cmdData = new byte[11];
                cmdData[0] = 1; // version byte
                BinaryPrimitives.WriteUInt16LittleEndian(cmdData.AsSpan(1, 2), command);
                BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(3, 4), functionCode);
                BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(7, 4), ext);

                _logger.LogDebug("ReadWithBuffer attempt {Attempt}: CMD_PREPARE_BUFFER inner_cmd={Command}, fct={Fct}, ext={Ext}",
                    attempt + 1, command, functionCode, ext);

                var resp = SendCommand(ZKProtocol.CMD_PREPARE_BUFFER, cmdData);

                _logger.LogDebug("ReadWithBuffer: CMD_PREPARE_BUFFER response code={Code}, dataLen={DataLen}",
                    resp.Code, resp.Data.Length);

                if (!resp.IsOk && resp.Code != ZKProtocol.CMD_DATA)
                {
                    _logger.LogWarning("ReadWithBuffer attempt {Attempt}: CMD_PREPARE_BUFFER failed with code={Code}",
                        attempt + 1, resp.Code);
                    if (attempt + 1 < maxRetries)
                    {
                        Thread.Sleep(200 * (attempt + 1));
                        continue;
                    }
                    return Array.Empty<byte>();
                }

                // Case 1: Data returned inline (CMD_DATA)
                if (resp.Code == ZKProtocol.CMD_DATA)
                {
                    _logger.LogDebug("ReadWithBuffer: got inline CMD_DATA, {Len} bytes", resp.Data.Length);
                    return resp.Data;
                }

                // Case 2: Size returned, need to read chunks
                if (resp.Data.Length < 5)
                {
                    _logger.LogWarning("ReadWithBuffer: response data too short ({Len} bytes)", resp.Data.Length);
                    return resp.Data.Length > 0 ? resp.Data : Array.Empty<byte>();
                }

                int totalSize = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(1, 4));
                _logger.LogDebug("ReadWithBuffer: totalSize={Size}, reading chunks of max {Chunk} bytes",
                    totalSize, MAX_CHUNK);

                using var ms = new MemoryStream();
                int offset = 0;
                int remaining = totalSize;

                while (remaining > 0)
                {
                    int chunkSize = Math.Min(remaining, MAX_CHUNK);
                    byte[] chunk = ReadChunk(offset, chunkSize);
                    if (chunk.Length == 0)
                    {
                        _logger.LogWarning("ReadWithBuffer: ReadChunk returned 0 bytes at offset={Offset}", offset);
                        break;
                    }
                    ms.Write(chunk, 0, chunk.Length);
                    offset += chunkSize;
                    remaining -= chunkSize;
                }

                FreeData();

                byte[] result = ms.ToArray();
                _logger.LogDebug("ReadWithBuffer: completed, total {Total} bytes", result.Length);
                return result;
            }

            return Array.Empty<byte>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReadWithBuffer error for command {Command}", command);
            return Array.Empty<byte>();
        }
    }

    /// <summary>
    /// Read a chunk of data from the device.
    /// pyzk: __read_chunk sends CMD_READ_BUFFER with pack('ii', start, size)
    /// pyzk retries 3 times with __recieve_chunk
    /// </summary>
    private byte[] ReadChunk(int start, int size)
    {
        for (int retry = 0; retry < 3; retry++)
        {
            // pyzk: command_string = pack('<ii', start, size) = 8 bytes
            byte[] cmdData = new byte[8];
            BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(0, 4), start);
            BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(4, 4), size);

            var resp = SendCommand(ZKProtocol.CMD_READ_BUFFER, cmdData);

            // pyzk __recieve_chunk: checks self.__response == CMD_DATA, returns self.__data
            if (resp.Code == ZKProtocol.CMD_DATA && resp.Data.Length > 0)
            {
                _logger.LogDebug("ReadChunk: got CMD_DATA, {Len} bytes", resp.Data.Length);
                return resp.Data;
            }

            // Also accept CMD_PREPARE_DATA with data
            if (resp.Code == ZKProtocol.CMD_PREPARE_DATA && resp.Data.Length > 0)
            {
                // pyzk __recieve_chunk handles CMD_PREPARE_DATA by reading more
                // For simplicity, return what we have
                _logger.LogDebug("ReadChunk: got CMD_PREPARE_DATA, {Len} bytes", resp.Data.Length);
                return resp.Data;
            }

            // Accept any response with data
            if (resp.Data.Length > 0)
            {
                _logger.LogDebug("ReadChunk: got code={Code}, returning {Len} bytes", resp.Code, resp.Data.Length);
                return resp.Data;
            }

            _logger.LogWarning("ReadChunk attempt {Retry}: code={Code}, dataLen={Len}", retry + 1, resp.Code, resp.Data.Length);
            Thread.Sleep(100);
        }

        _logger.LogWarning("ReadChunk: exhausted retries for start={Start}, size={Size}", start, size);
        return Array.Empty<byte>();
    }

    /// <summary>
    /// Free device data buffer.
    /// pyzk: free_data sends CMD_FREE_DATA (1502)
    /// </summary>
    private void FreeData()
    {
        try
        {
            if (_stream == null) return;

            // Send raw CMD_FREE_DATA instead of using SendCommand, so we can
            // drain ALL stale responses (not just the first one).
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
            var packet = ZKProtocol.BuildPacket(ZKProtocol.CMD_FREE_DATA, Array.Empty<byte>(), _sessionId, _replyId);
            _stream.Write(packet, 0, packet.Length);

            // Drain ALL response packets to clear stale buffer data.
            // After ReadWithBuffer, the device may still send leftover DATA/PREPARE_DATA
            // chunks that would corrupt subsequent commands.
            int drained = 0;
            while (_stream.DataAvailable)
            {
                var header = ReadExact(8);
                if (header == null) break;
                ushort m1 = BinaryPrimitives.ReadUInt16LittleEndian(header.AsSpan(0, 2));
                ushort m2 = BinaryPrimitives.ReadUInt16LittleEndian(header.AsSpan(2, 2));
                if (m1 != ZKProtocol.Magic1 || m2 != ZKProtocol.Magic2) break;
                uint ps = BitConverter.ToUInt32(header, 4);
                if (ps < 8) break;
                var payload = ReadExact((int)ps);
                if (payload == null) break;
                var code = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(0, 2));
                if (code == ZKProtocol.CMD_REG_EVENT)
                    SendInternalAck();
                drained++;
                if (code == ZKProtocol.CMD_ACK_OK) break;
            }
            _logger.LogDebug("FreeData: drained {Count} packets", drained);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "FreeData: error");
        }
    }

    /// <summary>
    /// Read exactly n bytes from the network stream.
    /// </summary>
    private byte[]? ReadExact(int count)
    {
        if (_stream == null) return null;
        byte[] buffer = new byte[count];
        int offset = 0;
        while (offset < count)
        {
            int read;
            try
            {
                read = _stream.Read(buffer, offset, count - offset);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ReadExact: exception reading from stream");
                CleanupConnection();
                return null;
            }
            if (read == 0)
            {
                _logger.LogWarning("ReadExact: connection closed by device (received 0 bytes)");
                CleanupConnection();
                return null;
            }
            offset += read;
        }
        return buffer;
    }

    private void CleanupConnection()
    {
        if (_stream != null)
        {
            try { _stream.Dispose(); } catch { }
            _stream = null;
        }
        if (_tcp != null)
        {
            try { _tcp.Dispose(); } catch { }
            _tcp = null;
        }
        IsConnected = false;
    }

    /// <summary>
    /// Send a command and receive a single response frame.
    /// Matches pyzk's __send_command / __create_header behavior exactly:
    /// - Increments reply_id BEFORE building the packet (inside __create_header)
    /// - Reads reply_id from device response
    /// - Does NOT update session_id (only set during Connect)
    /// </summary>
    private CommandResponse SendCommand(ushort command, byte[] data)
    {
        if (_stream == null)
        {
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
            return new CommandResponse { Code = 0, IsOk = false };
        }

        try
        {
            // Drain any pending async data before sending command.
            // After REG_EVENT registration, the device streams unsolicited
            // attendance packets (CMD_DATA). If not drained, these get consumed
            // as responses to subsequent commands, corrupting the protocol.
            int drained = 0;
            var buffer = new byte[4096];
            while (_stream.DataAvailable)
            {
                drained += _stream.Read(buffer, 0, buffer.Length);
            }
            if (drained > 0)
                _logger.LogDebug("Drained {Count} bytes before command {Command}", drained, command);

            // pyzk __create_header: increments reply_id, then builds packet with incremented value
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;

            byte[] packet = ZKProtocol.BuildPacket(command, data, _sessionId, _replyId);
            _stream.Write(packet, 0, packet.Length);

            return ReadCommandResponse();
        }
        catch (Exception ex)
        {
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
            _logger.LogDebug(ex, "SendCommand error for cmd={Command}", command);
            CleanupConnection();
            return new CommandResponse { Code = 0, IsOk = false };
        }
    }

    /// <summary>
    /// Write a command packet WITHOUT draining the stream first. Used when pulling
    /// prepared attendance data: the device may have already pushed the first frames
    /// unsolicited right after PREPARE_DATA, and draining would discard them.
    /// </summary>
    private void SendRawNoDrain(ushort command, byte[] data)
    {
        if (_stream == null) return;
        _replyId++;
        if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
        byte[] packet = ZKProtocol.BuildPacket(command, data, _sessionId, _replyId);
        _stream.Write(packet, 0, packet.Length);
    }

    /// <summary>
    /// Read one response frame from the device.
    /// Skips async REG_EVENT packets by discarding them and reading the next response.
    /// Increased skips to 200 to handle async events that pile up between polls.
    /// </summary>
    private CommandResponse ReadCommandResponse(int maxEventSkips = 200)
    {
        if (_stream == null)
            return new CommandResponse { Code = 0, IsOk = false };

        try
        {
            for (int skip = 0; skip < maxEventSkips; skip++)
            {
                byte[] topHeader = ReadExact(8);
                if (topHeader == null)
                    return new CommandResponse { Code = 0, IsOk = false };

                ushort magic1 = BinaryPrimitives.ReadUInt16LittleEndian(topHeader.AsSpan(0, 2));
                ushort magic2 = BinaryPrimitives.ReadUInt16LittleEndian(topHeader.AsSpan(2, 2));
                uint payloadSize = BitConverter.ToUInt32(topHeader, 4);

                if (magic1 != ZKProtocol.Magic1 || magic2 != ZKProtocol.Magic2 || payloadSize < 8)
                    return new CommandResponse { Code = 0, IsOk = false };

                byte[] payload = ReadExact((int)payloadSize);
                if (payload == null)
                    return new CommandResponse { Code = 0, IsOk = false };

                ushort respCode = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(0, 2));
                ushort respSessionId = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(4, 2));
                // IMPORTANT: Do NOT overwrite _replyId from the device response.
                // pyzk maintains an independent counter (increments before each send).
                // The device can return unexpected reply_ids (e.g., REG_EVENT echoes rid=3
                // for our replyId=1), and overwriting derails the sequence.
                byte[] respData = payload[8..];

                // Discard async events (CMD_REG_EVENT = 500) and read next response
                if (respCode == ZKProtocol.CMD_REG_EVENT)
                {
                    _logger.LogDebug("ReadCommandResponse: discarding async REG_EVENT, reading next response (skip={Skip})", skip + 1);
                    SendInternalAck();
                    continue;
                }

                bool isOk = respCode == ZKProtocol.CMD_ACK_OK
                         || respCode == ZKProtocol.CMD_PREPARE_DATA
                         || respCode == ZKProtocol.CMD_ACK_DATA
                         || respCode == ZKProtocol.CMD_DATA;

                return new CommandResponse
                {
                    Code = respCode,
                    IsOk = isOk,
                    Data = respData,
                    SessionId = respSessionId
                };
            }

            _logger.LogWarning("ReadCommandResponse: exceeded max event skips ({MaxSkips})", maxEventSkips);
            return new CommandResponse { Code = 0, IsOk = false };
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "ReadCommandResponse error");
            CleanupConnection();
            return new CommandResponse { Code = 0, IsOk = false };
        }
    }

    /// <summary>
    /// Send CMD_ACK_OK to acknowledge a data packet.
    /// pyzk: uses USHRT_MAX - 1 as reply_id for ack_ok.
    /// </summary>
    private void SendInternalAck()
    {
        if (_stream == null) return;
        try
        {
            // pyzk __ack_ok uses const.USHRT_MAX - 1 for reply_id
            byte[] packet = ZKProtocol.BuildPacket(ZKProtocol.CMD_ACK_OK, Array.Empty<byte>(), _sessionId, (ushort)(ZKProtocol.USHRT_MAX - 1));
            _stream.Write(packet, 0, packet.Length);
        }
        catch { }
    }

    /// <summary>
    /// Send CMD_ACK_OK to acknowledge a data packet.
    /// pyzk: uses USHRT_MAX - 1 as reply_id for ack_ok.
    /// </summary>
    private void SendAckOk()
    {
        if (_stream == null) return;
        try
        {
            // pyzk __ack_ok uses const.USHRT_MAX - 1 for reply_id
            byte[] packet = ZKProtocol.BuildPacket(ZKProtocol.CMD_ACK_OK, Array.Empty<byte>(), _sessionId, (ushort)(ZKProtocol.USHRT_MAX - 1));
            _stream.Write(packet, 0, packet.Length);
        }
        catch { }
    }

    private static VerifyMethod MapVerifyMethod(byte status)
    {
        return status switch
        {
            0 => VerifyMethod.Fingerprint,
            5 => VerifyMethod.RFIDCard,
            15 => VerifyMethod.Face,
            _ => VerifyMethod.Fingerprint
        };
    }

    private static string ReadNullTerminated(byte[] data, int offset, int maxLength)
    {
        int end = Math.Min(offset + maxLength, data.Length);
        int nullIdx = Array.IndexOf(data, (byte)0, offset, end - offset);
        if (nullIdx < 0) nullIdx = end;
        return System.Text.Encoding.ASCII.GetString(data, offset, nullIdx - offset);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Disconnect();
            _disposed = true;
        }
    }

    /// <summary>
    /// Raw diagnostic: send a command with data and return response code + hex data.
    /// </summary>
    public (ushort code, int dataLen, string hex, ushort sid, ushort rid) TestCommandRaw(ushort command, byte[] data)
    {
        if (!IsConnected) return (0, 0, "not connected", _sessionId, _replyId);
        var resp = SendCommand(command, data);
        string hex = resp.Data.Length > 0
            ? Convert.ToHexString(resp.Data.AsSpan(0, Math.Min(64, resp.Data.Length)))
            : "(empty)";
        return (resp.Code, resp.Data.Length, hex, resp.SessionId, _replyId);
    }

    /// <summary>
    /// Raw diagnostic: return hex of raw bytes received when sending a command.
    /// This bypasses magic-byte validation to show what the device actually sends.
    /// </summary>
    public (string sendHex, ushort recvMagic1, ushort recvMagic2, uint payloadSize, string recvPayloadHex, ushort code, int dataLen, ushort sid, ushort rid) RawSendRecv(ushort command, byte[] data)
    {
        if (!IsConnected || _stream == null) return ("", 0, 0, 0, "", 0, 0, 0, 0);

        try
        {
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;

            byte[] packet = ZKProtocol.BuildPacket(command, data, _sessionId, _replyId);
            string sendHex = Convert.ToHexString(packet.AsSpan(0, Math.Min(128, packet.Length)));

            _stream.Write(packet, 0, packet.Length);

            byte[] topHeader = new byte[8];
            int offset = 0;
            while (offset < 8)
            {
                int read = _stream.Read(topHeader, offset, 8 - offset);
                if (read == 0) return (sendHex, 0, 0, 0, "(connection closed)", 0, 0, 0, 0);
                offset += read;
            }

            ushort magic1 = BinaryPrimitives.ReadUInt16LittleEndian(topHeader.AsSpan(0, 2));
            ushort magic2 = BinaryPrimitives.ReadUInt16LittleEndian(topHeader.AsSpan(2, 2));
            uint payloadSize = BitConverter.ToUInt32(topHeader, 4);

            byte[] payload = new byte[payloadSize];
            offset = 0;
            while (offset < (int)payloadSize)
            {
                int read = _stream.Read(payload, offset, (int)payloadSize - offset);
                if (read == 0) return (sendHex, magic1, magic2, payloadSize, "(connection closed during payload)", 0, 0, 0, 0);
                offset += read;
            }

            string payloadHex = Convert.ToHexString(payload.AsSpan(0, Math.Min(64, payload.Length)));

            ushort respCode = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(0, 2));
            ushort respSid = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(4, 2));
            ushort respRid = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(6, 2));

            // Do NOT overwrite _replyId — same reason as ReadCommandResponse

            return (sendHex, magic1, magic2, payloadSize, payloadHex, respCode, (int)(payloadSize - 8), respSid, respRid);
        }
        catch (Exception ex)
        {
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
            return ("", 0, 0, 0, $"error: {ex.Message}", 0, 0, 0, 0);
        }
    }

    private class CommandResponse
    {
        public ushort Code { get; set; }
        public bool IsOk { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public ushort SessionId { get; set; }
    }
}
