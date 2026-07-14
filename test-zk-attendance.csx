using System.Net.Sockets;
using System.Buffers.Binary;
using HackGym.ZKTeco.Bridge.Protocol;

// Test direct connection to ZK device and read attendance logs
Console.WriteLine("=== ZK Device Attendance Recovery Test ===");
Console.WriteLine("Device: 192.168.1.201:4370\n");

var tcp = new TcpClient();
tcp.ReceiveTimeout = 5000;
tcp.SendTimeout = 5000;

Console.WriteLine("Connecting...");
var result = tcp.BeginConnect("192.168.1.201", 4370, null, null);
if (!result.AsyncWaitHandle.WaitOne(5000) || !tcp.Connected)
{
    Console.WriteLine("❌ FAILED: Cannot connect to device");
    return;
}
tcp.EndConnect(result);
Console.WriteLine("✅ Connected\n");

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
var connectResp = SendCommand(stream, ZKProtocol.CMD_CONNECT, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"  Response: Code={connectResp.Code}, SessionId={connectResp.SessionId}, DataLen={connectResp.Data.Length}");
sessionId = connectResp.SessionId;

if (connectResp.Code == ZKProtocol.CMD_ACK_UNAUTH)
{
    Console.WriteLine("Device requires auth, sending CMD_AUTH...");
    byte[] commKey = ZKProtocol.MakeCommKey(0, sessionId);
    var authResp = SendCommand(stream, ZKProtocol.CMD_AUTH, commKey, ref sessionId, ref replyId);
    Console.WriteLine($"  Auth response: Code={authResp.Code}");
}

Console.WriteLine($"\n✅ Session established (ID={sessionId})\n");

// TEST 1: CMD_ATTLOG_RRQ (direct - new protocol)
Console.WriteLine("=== TEST 1: CMD_ATTLOG_RRQ (direct command 13) ===");
var attResp1 = SendCommand(stream, ZKProtocol.CMD_ATTLOG_RRQ, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"Code={attResp1.Code}, DataLen={attResp1.Data.Length}");
if (attResp1.Data.Length > 8)
{
    Console.WriteLine($"✅ SUCCESS: Got {attResp1.Data.Length - 8} bytes of attendance data");
    ParseAttendanceLogs(attResp1.Data[8..]);
}
else
{
    Console.WriteLine($"❌ Empty response - trying fallback...");
}

Console.WriteLine();

// TEST 2: CMD_DB_RRQ + FCT_ATTLOG (old buffered protocol)
Console.WriteLine("=== TEST 2: CMD_DB_RRQ + FCT_ATTLOG (buffered) ===");
byte[] fctData = new byte[4];
BinaryPrimitives.WriteUInt16LittleEndian(fctData.AsSpan(0, 2), ZKProtocol.FCT_ATTLOG);
var attResp2 = SendCommand(stream, ZKProtocol.CMD_DB_RRQ, fctData, ref sessionId, ref replyId);
Console.WriteLine($"Code={attResp2.Code}, DataLen={attResp2.Data.Length}");

if (attResp2.Code == ZKProtocol.CMD_PREPARE_DATA && attResp2.Data.Length >= 4)
{
    int totalSize = BinaryPrimitives.ReadInt32LittleEndian(attResp2.Data.AsSpan(0, 4));
    Console.WriteLine($"PREPARE_DATA: totalSize={totalSize}");
    
    if (totalSize > 0)
    {
        // Pull data
        byte[] req = new byte[8];
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(0, 4), 0);
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(4, 4), totalSize);
        SendRaw(stream, ZKProtocol.CMD_DATA_RDY, req, sessionId, ref replyId);
        
        using var ms = new MemoryStream();
        for (int i = 0; i < 16; i++)
        {
            var frame = ReadResponse(stream);
            Console.WriteLine($"  Frame {i+1}: Code={frame.Code}, Len={frame.Data.Length}");
            if (frame.Data.Length > 0)
                ms.Write(frame.Data, 0, frame.Data.Length);
            if (frame.Code == ZKProtocol.CMD_ACK_OK || ms.Length >= totalSize)
                break;
        }
        
        if (ms.Length > 8)
        {
            Console.WriteLine($"✅ SUCCESS: Retrieved {ms.Length - 8} bytes via buffered protocol");
            ParseAttendanceLogs(ms.ToArray()[8..]);
        }
    }
}
else if (attResp2.Data.Length > 8)
{
    Console.WriteLine($"✅ Direct data: {attResp2.Data.Length - 8} bytes");
    ParseAttendanceLogs(attResp2.Data[8..]);
}
else
{
    Console.WriteLine($"❌ Also empty - device may have no logs or pointer stuck");
}

Console.WriteLine();

// TEST 3: Check device info
Console.WriteLine("=== TEST 3: Device Info ===");
var sizesResp = SendCommand(stream, ZKProtocol.CMD_GET_FREE_SIZES, Array.Empty<byte>(), ref sessionId, ref replyId);
if (sizesResp.Data.Length >= 8)
{
    int users = BinaryPrimitives.ReadInt32LittleEndian(sizesResp.Data.AsSpan(0, 4));
    int recCap = BinaryPrimitives.ReadInt32LittleEndian(sizesResp.Data.AsSpan(4, 4));
    Console.WriteLine($"Enrolled users: {users}");
    Console.WriteLine($"Attendance capacity: {recCap}");
}

tcp.Close();
Console.WriteLine("\n=== Test Complete ===");

// Helper methods
static (ushort Code, ushort SessionId, byte[] Data) SendCommand(NetworkStream s, ushort cmd, byte[] data, ref ushort sessionId, ref ushort replyId)
{
    byte[] packet = ZKProtocol.BuildPacket(cmd, data, sessionId, replyId);
    s.Write(packet, 0, packet.Length);
    return ReadResponse(s);
}

static void SendRaw(NetworkStream s, ushort cmd, byte[] data, ushort sessionId, ref ushort replyId)
{
    byte[] packet = ZKProtocol.BuildPacket(cmd, data, sessionId, replyId);
    s.Write(packet, 0, packet.Length);
}

static (ushort Code, ushort SessionId, byte[] Data) ReadResponse(NetworkStream s)
{
    byte[] header = new byte[8];
    s.Read(header, 0, 8);
    uint payloadLen = BinaryPrimitives.ReadUInt32LittleEndian(header.AsSpan(4));
    byte[] payload = new byte[payloadLen];
    s.Read(payload, 0, payloadLen);
    
    ushort code = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(0, 2));
    ushort sessionId = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(4, 2));
    byte[] data = payload[8..].ToArray();
    
    return (code, sessionId, data);
}

static void ParseAttendanceLogs(byte[] data)
{
    if (data.Length < 40)
    {
        Console.WriteLine($"  Data too short: {data.Length} bytes");
        return;
    }
    
    int count = (data.Length - 4) / 40;  // First 4 bytes = count header
    Console.WriteLine($"  Record count header: {BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(0, 4))}");
    Console.WriteLine($"  Parsing {Math.Min(5, count)} sample records:\n");
    
    int offset = 0;
    for (int i = 0; i < Math.Min(5, count); i++)
    {
        if (offset + 40 > data.Length) break;
        
        int uid = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));
        string enrollmentId = System.Text.Encoding.ASCII.GetString(data, offset + 2, 9).TrimEnd('\0').Trim();
        byte[] timeBytes = data[(offset + 27)..(offset + 31)];
        DateTime timestamp = ZKProtocol.DecodeTime(timeBytes);
        
        Console.WriteLine($"  [{i+1}] UID={uid}, EnrollmentId='{enrollmentId}', Time={timestamp:yyyy-MM-dd HH:mm:ss}");
        offset += 40;
    }
    
    if (count > 5)
        Console.WriteLine($"  ... and {count - 5} more records");
}