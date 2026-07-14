using System.Net.Sockets;
using System.Buffers.Binary;

// Test direct connection to ZK device and read attendance logs
Console.WriteLine("=== ZK Device Attendance Recovery Test ===");
Console.WriteLine("Device: 192.168.1.201:4370\n");

var tcp = new TcpClient();
tcp.ReceiveTimeout = 5000;
tcp.SendTimeout = 5000;

Console.WriteLine("Connecting...");
try
{
    var result = tcp.BeginConnect("192.168.1.201", 4370, null, null);
    if (!result.AsyncWaitHandle.WaitOne(5000) || !tcp.Connected)
    {
        Console.WriteLine("FAILED: Cannot connect to device - check IP and network");
        return;
    }
    tcp.EndConnect(result);
    Console.WriteLine("Connected\n");
}
catch (Exception ex)
{
    Console.WriteLine($"FAILED: {ex.Message}");
    return;
}

var stream = tcp.GetStream();
stream.ReadTimeout = 5000;
stream.WriteTimeout = 5000;

// Drain any initial data
if (stream.DataAvailable)
{
    byte[] initial = new byte[8192];
    int drained = stream.Read(initial, 0, 8192);
    Console.WriteLine($"Drained {drained} bytes initial data");
}

ushort sessionId = 0;
ushort replyId = 65534;

// Send CMD_CONNECT
Console.WriteLine("Sending CMD_CONNECT...");
var connectResp = SendCommand(stream, 1000, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"Response: Code={connectResp.Code}, SessionId={connectResp.SessionId}, DataLen={connectResp.Data.Length}");
sessionId = connectResp.SessionId;

if (connectResp.Code == 2005)  // CMD_ACK_UNAUTH
{
    Console.WriteLine("Device requires auth, sending CMD_AUTH...");
    byte[] commKey = MakeCommKey(0, sessionId);
    var authResp = SendCommand(stream, 1102, commKey, ref sessionId, ref replyId);
    Console.WriteLine($"Auth response: Code={authResp.Code}");
}

Console.WriteLine($"\nSession established (ID={sessionId})\n");

// TEST 1: CMD_ATTLOG_RRQ (direct - command 13)
Console.WriteLine("=== TEST 1: CMD_ATTLOG_RRQ (direct command 13) ===");
var attResp1 = SendCommand(stream, 13, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"Code={attResp1.Code}, DataLen={attResp1.Data.Length}");

if (attResp1.Code == 2000 && attResp1.Data.Length > 8)
{
    Console.WriteLine($"SUCCESS: Got {attResp1.Data.Length - 8} bytes of attendance data");
    ParseAttendanceLogs(attResp1.Data[8..]);
}
else if (attResp1.Code == 2000 && attResp1.Data.Length == 0)
{
    Console.WriteLine("Empty response - device has no new logs OR pointer stuck");
}
else
{
    Console.WriteLine($"Unexpected response - trying fallback...");
}

Console.WriteLine();

// Free resources before next attempt
SendCommand(stream, 1502, Array.Empty<byte>(), ref sessionId, ref replyId);  // CMD_FREE_DATA

// TEST 2: CMD_DB_RRQ + FCT_ATTLOG (old buffered protocol)
Console.WriteLine("=== TEST 2: CMD_DB_RRQ + FCT_ATTLOG (buffered) ===");
byte[] fctData = new byte[4];
BinaryPrimitives.WriteUInt16LittleEndian(fctData.AsSpan(0, 2), 1);  // FCT_ATTLOG
var attResp2 = SendCommand(stream, 7, fctData, ref sessionId, ref replyId);  // CMD_DB_RRQ
Console.WriteLine($"Code={attResp2.Code}, DataLen={attResp2.Data.Length}");

if (attResp2.Code == 1500 && attResp2.Data.Length >= 4)  // CMD_PREPARE_DATA
{
    int totalSize = BinaryPrimitives.ReadInt32LittleEndian(attResp2.Data.AsSpan(0, 4));
    Console.WriteLine($"PREPARE_DATA: totalSize={totalSize}");
    
    if (totalSize > 0)
    {
        byte[] req = new byte[8];
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(0, 4), 0);
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(4, 4), totalSize);
        SendRaw(stream, 1504, req, sessionId, ref replyId);  // CMD_DATA_RDY
        
        using var ms = new MemoryStream();
        for (int i = 0; i < 16; i++)
        {
            var frame = ReadResponse(stream);
            Console.WriteLine($"Frame {i+1}: Code={frame.Code}, Len={frame.Data.Length}");
            if (frame.Data.Length > 0)
                ms.Write(frame.Data, 0, frame.Data.Length);
            if (frame.Code == 2000 || ms.Length >= totalSize)  // CMD_ACK_OK
                break;
        }
        
        if (ms.Length > 8)
        {
            Console.WriteLine($"SUCCESS: Retrieved {ms.Length - 8} bytes via buffered protocol");
            ParseAttendanceLogs(ms.ToArray()[8..]);
        }
    }
}
else if (attResp2.Data.Length > 8)
{
    Console.WriteLine($"Direct data: {attResp2.Data.Length - 8} bytes");
    ParseAttendanceLogs(attResp2.Data[8..]);
}
else
{
    Console.WriteLine("Also empty - device likely has no logs stored");
}

// Free resources
SendCommand(stream, 1502, Array.Empty<byte>(), ref sessionId, ref replyId);

Console.WriteLine();

// TEST 3: Check device info
Console.WriteLine("=== TEST 3: Device Info ===");
var sizesResp = SendCommand(stream, 50, Array.Empty<byte>(), ref sessionId, ref replyId);  // CMD_GET_FREE_SIZES
if (sizesResp.Data.Length >= 8)
{
    int users = BinaryPrimitives.ReadInt32LittleEndian(sizesResp.Data.AsSpan(0, 4));
    int recCap = BinaryPrimitives.ReadInt32LittleEndian(sizesResp.Data.AsSpan(4, 4));
    Console.WriteLine($"Enrolled users: {users}");
    Console.WriteLine($"Attendance record capacity: {recCap}");
}

Console.WriteLine("\n=== Test Complete ===");
tcp.Close();

// Helper methods
static (ushort Code, ushort SessionId, byte[] Data) SendCommand(NetworkStream s, ushort cmd, byte[] data, ref ushort sessionId, ref ushort replyId)
{
    byte[] packet = BuildPacket(cmd, data, sessionId, replyId);
    s.Write(packet, 0, packet.Length);
    return ReadResponse(s);
}

static void SendRaw(NetworkStream s, ushort cmd, byte[] data, ushort sessionId, ref ushort replyId)
{
    byte[] packet = BuildPacket(cmd, data, sessionId, replyId);
    s.Write(packet, 0, packet.Length);
}

static (ushort Code, ushort SessionId, byte[] Data) ReadResponse(NetworkStream s)
{
    byte[] header = new byte[8];
    int read = 0;
    while (read < 8)
    {
        int r = s.Read(header, read, 8 - read);
        if (r == 0) throw new Exception("Connection closed");
        read += r;
    }
    
    uint payloadLen = BinaryPrimitives.ReadUInt32LittleEndian(header.AsSpan(4));
    byte[] payload = new byte[payloadLen];
    read = 0;
    while (read < payloadLen)
    {
        int r = s.Read(payload, read, (int)payloadLen - read);
        if (r == 0) throw new Exception("Connection closed");
        read += r;
    }
    
    ushort code = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(0, 2));
    ushort sessionId = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(4, 2));
    byte[] data = payload.Length > 8 ? payload[8..].ToArray() : Array.Empty<byte>();
    
    return (code, sessionId, data);
}

static byte[] BuildPacket(ushort command, byte[] data, ushort sessionId, ushort replyId)
{
    ushort Magic1 = 0x5050;
    ushort Magic2 = 0x7D82;
    
    var payload = new byte[8 + data.Length];
    BitConverter.GetBytes(command).CopyTo(payload, 0);
    // checksum placeholder
    BitConverter.GetBytes(sessionId).CopyTo(payload, 4);
    BitConverter.GetBytes(replyId).CopyTo(payload, 6);
    data.CopyTo(payload, 8);
    
    ushort chk = CalculateChecksum(payload);
    BitConverter.GetBytes(chk).CopyTo(payload, 2);
    
    var packet = new byte[8 + payload.Length];
    BitConverter.GetBytes(Magic1).CopyTo(packet, 0);
    BitConverter.GetBytes(Magic2).CopyTo(packet, 2);
    BitConverter.GetBytes((uint)payload.Length).CopyTo(packet, 4);
    payload.CopyTo(packet, 8);
    
    return packet;
}

static ushort CalculateChecksum(byte[] payload)
{
    uint chk = 0;
    for (int i = 0; i + 1 < payload.Length; i += 2)
        chk += BitConverter.ToUInt16(payload, i);
    if (payload.Length % 2 != 0)
        chk += payload[payload.Length - 1];
    chk = (chk >> 16) + (chk & 0xFFFF);
    chk += (chk >> 16);
    return (ushort)(~chk & 0xFFFF);
}

static byte[] MakeCommKey(int key, int sessionId, int ticks = 50)
{
    uint k = 0;
    for (int i = 0; i < 32; i++)
    {
        if ((key & (1 << i)) != 0)
            k = (k << 1) | 1;
        else
            k <<= 1;
    }
    k += (uint)sessionId;
    
    byte[] kBytes = BitConverter.GetBytes(k);
    kBytes[0] ^= (byte)'Z';
    kBytes[1] ^= (byte)'K';
    kBytes[2] ^= (byte)'S';
    kBytes[3] ^= (byte)'O';
    
    byte tmp = kBytes[0]; kBytes[0] = kBytes[2]; kBytes[2] = tmp;
    tmp = kBytes[1]; kBytes[1] = kBytes[3]; kBytes[3] = tmp;
    
    byte b = (byte)(0xFF & ticks);
    kBytes[0] ^= b;
    kBytes[1] ^= b;
    kBytes[3] ^= b;
    
    return new byte[] { kBytes[0], kBytes[1], b, kBytes[3] };
}

static void ParseAttendanceLogs(byte[] data)
{
    if (data.Length < 4)
    {
        Console.WriteLine($"Data too short: {data.Length} bytes");
        return;
    }
    
    // Some devices have 4-byte count header, some don't
    int recordCount = data.Length / 40;
    Console.WriteLine($"Data length: {data.Length} bytes, ~{recordCount} records\n");
    
    int offset = 0;
    // Check if first 4 bytes look like a count header
    int potentialCount = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(0, 4));
    if (potentialCount > 0 && potentialCount * 40 <= data.Length - 4)
    {
        Console.WriteLine($"Count header: {potentialCount} records");
        offset = 4;
    }
    
    int found = 0;
    for (int i = 0; offset + 40 <= data.Length && found < 10; i++)
    {
        int uid = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));
        string enrollmentId = System.Text.Encoding.ASCII.GetString(data, offset + 2, 9).TrimEnd('\0').Trim();
        
        // Try both time formats (4-byte LE and 6-byte hex)
        byte[] timeBytes = data[(offset + 27)..(offset + 31)];
        DateTime timestamp = DecodeTime(timeBytes);
        
        Console.WriteLine($"[{i+1}] UID={uid}, EnrollmentId='{enrollmentId}', Time={timestamp:yyyy-MM-dd HH:mm:ss}");
        offset += 40;
        found++;
    }
    
    if (recordCount > found)
        Console.WriteLine($"... and {recordCount - found} more records");
}

static DateTime DecodeTime(byte[] t)
{
    uint val = BitConverter.ToUInt32(t, 0);
    int second = (int)(val % 60); val /= 60;
    int minute = (int)(val % 60); val /= 60;
    int hour = (int)(val % 24); val /= 24;
    int day = (int)(val % 31) + 1; val /= 31;
    int month = (int)(val % 12) + 1; val /= 12;
    int year = (int)val + 2000;
    
    try {
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
    } catch {
        return DateTime.MinValue;
    }
}