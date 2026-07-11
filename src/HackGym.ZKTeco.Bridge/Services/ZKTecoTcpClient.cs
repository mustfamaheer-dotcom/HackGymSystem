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

            if (resp.Code == ZKProtocol.CMD_ACK_OK || resp.Code == 0)
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

        byte[] userdata = ReadWithBuffer(ZKProtocol.CMD_USERTEMP_RRQ, ZKProtocol.FCT_USER);
        _logger.LogWarning("ReadWithBuffer for users returned {Len} bytes, first 32 hex: {Hex}",
            userdata.Length, userdata.Length > 0 ? Convert.ToHexString(userdata.AsSpan(0, Math.Min(32, userdata.Length))) : "(empty)");
        if (userdata.Length <= 4) return users;

        int totalSize = BinaryPrimitives.ReadInt32LittleEndian(userdata.AsSpan(0, 4));
        int packetSize = sizes.Users > 0 ? totalSize / sizes.Users : 0;
        _logger.LogDebug("User data: totalSize={Total}, usersFromSizes={Users}, computed packetSize={PacketSize}, dataLen={DataLen}",
            totalSize, sizes.Users, packetSize, userdata.Length - 4);
        userdata = userdata[4..];

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
                string password = ReadNullTerminated(userdata, 3, 8);
                string name = ReadNullTerminated(userdata, 11, 24).Trim();
                // card at 35 (4 bytes) - skip
                string groupId = ReadNullTerminated(userdata, 40, 7).Trim();
                string userId = ReadNullTerminated(userdata, 48, 24).Trim();

                if (string.IsNullOrEmpty(name))
                    name = $"NN-{userId}";

                users.Add(new ZKUserInfo
                {
                    EnrollmentId = userId,
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
        return users;
    }

    /// <summary>
    /// Get attendance logs from the device.
    /// Uses CMD_DB_RRQ + FCT_ATTLOG with buffer-based transfer.
    /// </summary>
    public List<ZKAttendanceEvent> GetAttendanceLogs()
    {
        var logs = new List<ZKAttendanceEvent>();
        if (!IsConnected) return logs;

        byte[] data = ReadWithBuffer(ZKProtocol.CMD_ATTLOG_RRQ);
        if (data.Length <= 4) return logs;

        int totalSize = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(0, 4));
        data = data[4..];
        int offset = 0;

        while (offset + 10 <= data.Length && offset + 10 <= totalSize)
        {
            try
            {
                // Attendance record format (varies by device):
                // 10 bytes: uid(2) + status(1) + punch(1) + timestamp(6)
                // 12 bytes: uid(4) + status(1) + punch(1) + timestamp(6)
                // 14 bytes: uid(2) + status(1) + punch(1) + timestamp(6) + reserved(4)
                // 32 bytes: userId_string(24) + status(1) + punch(1) + timestamp(6)
                // 36 bytes: userId_string(24) + status(1) + punch(1) + timestamp(6) + reserved(4)
                // 52 bytes: userId_string(24) + status(1) + punch(1) + timestamp(6) + reserved(20)

                int recordSize = 10;
                string userId;
                byte status, punch;
                byte[] timeHex;

                // Try to detect record size by checking patterns
                if (offset + 52 <= data.Length && (data.Length - offset) >= 52)
                {
                    // Try 52-byte format first (most common for face devices)
                    var nameBytes = data[offset..(offset + 24)];
                    status = data[offset + 24];
                    punch = data[offset + 25];
                    timeHex = data[(offset + 26)..(offset + 32)];
                    userId = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0').Trim();
                    recordSize = 52;
                }
                else if (offset + 36 <= data.Length)
                {
                    var nameBytes = data[offset..(offset + 24)];
                    status = data[offset + 24];
                    punch = data[offset + 25];
                    timeHex = data[(offset + 26)..(offset + 32)];
                    userId = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0').Trim();
                    recordSize = 36;
                }
                else if (offset + 12 <= data.Length)
                {
                    // 12-byte format
                    uint uidVal = BitConverter.ToUInt32(data, offset);
                    status = data[offset + 4];
                    punch = data[offset + 5];
                    timeHex = data[(offset + 6)..(offset + 12)];
                    userId = uidVal.ToString();
                    recordSize = 12;
                }
                else
                {
                    // 10-byte format (smallest)
                    ushort uidVal = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));
                    status = data[offset + 2];
                    punch = data[offset + 3];
                    timeHex = data[(offset + 4)..(offset + 10)];
                    userId = uidVal.ToString();
                    recordSize = 10;
                }

                DateTime timestamp = ZKProtocol.DecodeTimeHex(timeHex);

                logs.Add(new ZKAttendanceEvent
                {
                    EnrollmentId = userId,
                    Method = MapVerifyMethod(status),
                    Timestamp = timestamp.ToUniversalTime(),
                    Direction = punch,
                    MachineNumber = 1
                });

                offset += recordSize;
            }
            catch
            {
                // If parsing fails, skip one byte and try again
                offset++;
            }
        }

        _logger.LogInformation("Read {Count} attendance logs from device", logs.Count);
        return logs;
    }

    /// <summary>
    /// Clear all attendance logs from the device.
    /// </summary>
    public bool ClearAttendanceLogs()
    {
        if (!IsConnected) return false;

        EnableDevice(false);
        try
        {
            var resp = SendCommand(ZKProtocol.CMD_CLEAR_ATTLOG, Array.Empty<byte>());
            return resp.IsOk;
        }
        finally
        {
            EnableDevice(true);
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
            // pyzk: command_string = pack('<bhii', 1, command, fct, ext) = 11 bytes
            byte[] cmdData = new byte[11];
            cmdData[0] = 1; // version byte
            BinaryPrimitives.WriteUInt16LittleEndian(cmdData.AsSpan(1, 2), command); // command as short
            BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(3, 4), functionCode); // fct
            BinaryPrimitives.WriteInt32LittleEndian(cmdData.AsSpan(7, 4), ext); // ext

            _logger.LogDebug("ReadWithBuffer(CMD_PREPARE_BUFFER): inner_cmd={Command}, fct={Fct}, ext={Ext}",
                command, functionCode, ext);

            var resp = SendCommand(ZKProtocol.CMD_PREPARE_BUFFER, cmdData);

            _logger.LogDebug("ReadWithBuffer: CMD_PREPARE_BUFFER response code={Code}, dataLen={DataLen}", resp.Code, resp.Data.Length);

            if (!resp.IsOk && resp.Code != ZKProtocol.CMD_DATA)
            {
                _logger.LogWarning("ReadWithBuffer: CMD_PREPARE_BUFFER failed with code={Code}", resp.Code);
                return Array.Empty<byte>();
            }

            // Case 1: Data returned inline (small datasets, pyzk checks cmd_response['code'] == CMD_DATA)
            if (resp.Code == ZKProtocol.CMD_DATA)
            {
                _logger.LogDebug("ReadWithBuffer: got inline CMD_DATA, {Len} bytes", resp.Data.Length);
                return resp.Data;
            }

            // Case 2: Size returned, need to read chunks
            // pyzk: size = unpack('I', self.__data[1:5])[0]  — skips first byte, reads 4-byte LE int
            if (resp.Data.Length < 5)
            {
                _logger.LogWarning("ReadWithBuffer: response data too short ({Len} bytes)", resp.Data.Length);
                return resp.Data.Length > 0 ? resp.Data : Array.Empty<byte>();
            }

            int totalSize = BinaryPrimitives.ReadInt32LittleEndian(resp.Data.AsSpan(1, 4));
            _logger.LogDebug("ReadWithBuffer: totalSize={Size}, reading chunks of max {Chunk} bytes", totalSize, MAX_CHUNK);

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

            // pyzk: self.free_data()
            FreeData();

            byte[] result = ms.ToArray();
            _logger.LogDebug("ReadWithBuffer: completed, total {Total} bytes", result.Length);
            return result;
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
    /// </summary>
    private byte[] ReadChunk(int start, int size)
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

        // Sometimes device may respond with the data differently
        if (resp.Data.Length > 0)
        {
            _logger.LogDebug("ReadChunk: got code={Code}, returning {Len} bytes", resp.Code, resp.Data.Length);
            return resp.Data;
        }

        _logger.LogWarning("ReadChunk: unexpected response code={Code}, dataLen={Len}", resp.Code, resp.Data.Length);
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
            SendCommand(ZKProtocol.CMD_FREE_DATA, Array.Empty<byte>());
            _logger.LogDebug("FreeData: sent CMD_FREE_DATA");
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "FreeData: error sending CMD_FREE_DATA");
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
            int read = _stream.Read(buffer, offset, count - offset);
            if (read == 0) return null;
            offset += read;
        }
        return buffer;
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
            return new CommandResponse { Code = 0, IsOk = false };

        try
        {
            // pyzk __create_header: increments reply_id, then builds packet with incremented value
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;

            byte[] packet = ZKProtocol.BuildPacket(command, data, _sessionId, _replyId);
            _stream.Write(packet, 0, packet.Length);

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
            // pyzk: self.__reply_id = self.__header[3] (reads from response)
            _replyId = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(6, 2));
            // NOTE: pyzk does NOT update session_id from response — only during connect()
            byte[] respData = payload[8..];

            bool isOk = respCode == ZKProtocol.CMD_ACK_OK
                     || respCode == ZKProtocol.CMD_PREPARE_DATA
                     || respCode == ZKProtocol.CMD_ACK_DATA
                     || respCode == ZKProtocol.CMD_DATA
                     || respCode == 0;

            return new CommandResponse
            {
                Code = respCode,
                IsOk = isOk,
                Data = respData,
                SessionId = respSessionId
            };
        }
        catch (Exception ex)
        {
            _replyId++;
            if (_replyId >= ZKProtocol.USHRT_MAX) _replyId -= ZKProtocol.USHRT_MAX;
            _logger.LogDebug(ex, "SendCommand error for cmd={Command}", command);
            return new CommandResponse { Code = 0, IsOk = false };
        }
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
    public (ushort code, int dataLen, string hex) TestCommandRaw(ushort command, byte[] data)
    {
        if (!IsConnected) return (0, 0, "not connected");
        var resp = SendCommand(command, data);
        string hex = resp.Data.Length > 0
            ? Convert.ToHexString(resp.Data.AsSpan(0, Math.Min(64, resp.Data.Length)))
            : "(empty)";
        return (resp.Code, resp.Data.Length, hex);
    }

    private class CommandResponse
    {
        public ushort Code { get; set; }
        public bool IsOk { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public ushort SessionId { get; set; }
    }
}
