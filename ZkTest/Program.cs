using System.Net.Sockets;
using System.Buffers.Binary;

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
        Console.WriteLine("FAILED: Cannot connect - device offline or wrong IP");
        return;
    }
    tcp.EndConnect(result);
    Console.WriteLine("CONNECTED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"FAILED: {ex.Message}");
    return;
}

var stream = tcp.GetStream();
stream.ReadTimeout = 5000;
stream.WriteTimeout = 5000;

if (stream.DataAvailable)
{
    byte[] initial = new byte[8192];
    int drained = stream.Read(initial, 0, 8192);
    Console.WriteLine($"Drained {drained} bytes initial data");
}

ushort sessionId = 0;
ushort replyId = 65534;

Console.WriteLine("Sending CMD_CONNECT...");
var connectResp = SendCommand(stream, 1000, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"Code={connectResp.Code}, SessionId={connectResp.SessionId}, DataLen={connectResp.Data.Length}");
sessionId = connectResp.SessionId;

if (connectResp.Code == 2005)
{
    Console.WriteLine("Auth required, sending CMD_AUTH...");
    byte[] commKey = MakeCommKey(0, sessionId);
    var authResp = SendCommand(stream, 1102, commKey, ref sessionId, ref replyId);
    Console.WriteLine($"Auth Code={authResp.Code}");
}

Console.WriteLine($"SESSION={sessionId}\n");

// TEST 1: CMD_ATTLOG_RRQ (13)
Console.WriteLine("=== TEST 1: CMD_ATTLOG_RRQ (13) ===");
var att1 = SendCommand(stream, 13, Array.Empty<byte>(), ref sessionId, ref replyId);
Console.WriteLine($"Code={att1.Code}, DataLen={att1.Data.Length}");
if (att1.Code == 2000 && att1.Data.Length > 8)
{
    Console.WriteLine($"GOT DATA: {att1.Data.Length - 8} bytes");
    ParseLogs(att1.Data[8..]);
}
else if (att1.Code == 2000)
{
    Console.WriteLine("EMPTY - no new logs or pointer stuck");
}
SendCommand(stream, 1502, Array.Empty<byte>(), ref sessionId, ref replyId);

// TEST 2: CMD_DB_RRQ+FCT_ATTLOG
Console.WriteLine("\n=== TEST 2: CMD_DB_RRQ+FCT_ATTLOG ===");
byte[] fct = new byte[4];
BinaryPrimitives.WriteUInt16LittleEndian(fct.AsSpan(0, 2), 1);
var att2 = SendCommand(stream, 7, fct, ref sessionId, ref replyId);
Console.WriteLine($"Code={att2.Code}, DataLen={att2.Data.Length}");

if (att2.Code == 1500 && att2.Data.Length >= 4)
{
    int total = BinaryPrimitives.ReadInt32LittleEndian(att2.Data.AsSpan(0, 4));
    Console.WriteLine($"PREPARE_DATA size={total}");
    if (total > 0)
    {
        byte[] req = new byte[8];
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(0, 4), 0);
        BinaryPrimitives.WriteInt32LittleEndian(req.AsSpan(4, 4), total);
        SendRaw(stream, 1504, req, sessionId, ref replyId);
        
        using var ms = new MemoryStream();
        for (int i = 0; i < 16; i++)
        {
            var f = ReadResponse(stream);
            if (f.Data.Length > 0) ms.Write(f.Data, 0, f.Data.Length);
            if (f.Code == 2000 || ms.Length >= total) break;
        }
        if (ms.Length > 8)
        {
            Console.WriteLine($"GOT DATA: {ms.Length - 8} bytes via buffered");
            ParseLogs(ms.ToArray()[8..]);
        }
    }
}
else if (att2.Data.Length > 8)
{
    Console.WriteLine($"DIRECT: {att2.Data.Length - 8} bytes");
    ParseLogs(att2.Data[8..]);
}
SendCommand(stream, 1502, Array.Empty<byte>(), ref sessionId, ref replyId);

// TEST 3: Device info
Console.WriteLine("\n=== DEVICE INFO ===");
var sizes = SendCommand(stream, 50, Array.Empty<byte>(), ref sessionId, ref replyId);
if (sizes.Data.Length >= 8)
{
    int users = BinaryPrimitives.ReadInt32LittleEndian(sizes.Data.AsSpan(0, 4));
    int cap = BinaryPrimitives.ReadInt32LittleEndian(sizes.Data.AsSpan(4, 4));
    Console.WriteLine($"Users={users}, Capacity={cap}");
}

tcp.Close();
Console.WriteLine("\nDONE");

static (ushort Code, ushort SessionId, byte[] Data) SendCommand(NetworkStream s, ushort cmd, byte[] data, ref ushort sessionId, ref ushort replyId)
{
    byte[] pkt = BuildPacket(cmd, data, sessionId, replyId);
    s.Write(pkt, 0, pkt.Length);
    return ReadResponse(s);
}

static void SendRaw(NetworkStream s, ushort cmd, byte[] data, ushort sessionId, ref ushort replyId)
{
    byte[] pkt = BuildPacket(cmd, data, sessionId, replyId);
    s.Write(pkt, 0, pkt.Length);
}

static (ushort Code, ushort SessionId, byte[] Data) ReadResponse(NetworkStream s)
{
    byte[] hdr = new byte[8];
    int r = 0;
    while (r < 8) { int x = s.Read(hdr, r, 8 - r); if (x == 0) throw new Exception("Closed"); r += x; }
    uint len = BinaryPrimitives.ReadUInt32LittleEndian(hdr.AsSpan(4));
    byte[] pay = new byte[len];
    r = 0;
    while (r < len) { int x = s.Read(pay, r, (int)len - r); if (x == 0) throw new Exception("Closed"); r += x; }
    ushort code = BinaryPrimitives.ReadUInt16LittleEndian(pay.AsSpan(0, 2));
    ushort sid = BinaryPrimitives.ReadUInt16LittleEndian(pay.AsSpan(4, 2));
    byte[] data = pay.Length > 8 ? pay[8..].ToArray() : Array.Empty<byte>();
    return (code, sid, data);
}

static byte[] BuildPacket(ushort cmd, byte[] data, ushort sessionId, ushort replyId)
{
    var pay = new byte[8 + data.Length];
    BitConverter.GetBytes(cmd).CopyTo(pay, 0);
    BitConverter.GetBytes(sessionId).CopyTo(pay, 4);
    BitConverter.GetBytes(replyId).CopyTo(pay, 6);
    data.CopyTo(pay, 8);
    ushort chk = CalcChecksum(pay);
    BitConverter.GetBytes(chk).CopyTo(pay, 2);
    var pkt = new byte[8 + pay.Length];
    BitConverter.GetBytes(0x5050).CopyTo(pkt, 0);
    BitConverter.GetBytes(0x7D82).CopyTo(pkt, 2);
    BitConverter.GetBytes((uint)pay.Length).CopyTo(pkt, 4);
    pay.CopyTo(pkt, 8);
    return pkt;
}

static ushort CalcChecksum(byte[] pay)
{
    uint c = 0;
    for (int i = 0; i + 1 < pay.Length; i += 2) c += BitConverter.ToUInt16(pay, i);
    if (pay.Length % 2 != 0) c += pay[pay.Length - 1];
    c = (c >> 16) + (c & 0xFFFF); c += (c >> 16);
    return (ushort)(~c & 0xFFFF);
}

static byte[] MakeCommKey(int key, int sess, int ticks = 50)
{
    uint k = 0;
    for (int i = 0; i < 32; i++) k = (key & (1 << i)) != 0 ? (k << 1) | 1 : k << 1;
    k += (uint)sess;
    byte[] b = BitConverter.GetBytes(k);
    b[0] ^= (byte)'Z'; b[1] ^= (byte)'K'; b[2] ^= (byte)'S'; b[3] ^= (byte)'O';
    byte t = b[0]; b[0] = b[2]; b[2] = t; t = b[1]; b[1] = b[3]; b[3] = t;
    byte x = (byte)(0xFF & ticks);
    b[0] ^= x; b[1] ^= x; b[3] ^= x;
    return new[] { b[0], b[1], x, b[3] };
}

static void ParseLogs(byte[] d)
{
    if (d.Length < 40) { Console.WriteLine("Too short"); return; }
    Console.WriteLine($"Records: ~{d.Length / 40}\n");
    for (int i = 0, off = 0; off + 40 <= d.Length && i < 10; i++, off += 40)
    {
        int uid = BinaryPrimitives.ReadUInt16LittleEndian(d.AsSpan(off, 2));
        string eid = System.Text.Encoding.ASCII.GetString(d, off + 2, 9).TrimEnd('\0').Trim();
        byte[] tb = d[(off + 27)..(off + 31)];
        DateTime ts = DecodeTime(tb);
        Console.WriteLine($"{i+1}. UID={uid}, Eid='{eid}', Time={ts:yyyy-MM-dd HH:mm:ss}");
    }
}

static DateTime DecodeTime(byte[] t)
{
    uint v = BitConverter.ToUInt32(t, 0);
    int s = (int)(v % 60); v /= 60;
    int m = (int)(v % 60); v /= 60;
    int h = (int)(v % 24); v /= 24;
    int d = (int)(v % 31) + 1; v /= 31;
    int mo = (int)(v % 12) + 1; v /= 12;
    int y = (int)v + 2000;
    try { return new DateTime(y, mo, d, h, m, s); } catch { return DateTime.MinValue; }
}