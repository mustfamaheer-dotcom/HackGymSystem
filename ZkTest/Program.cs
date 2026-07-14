// Check if Connect_Net takes a password
using System.Reflection;

Type? t = Type.GetTypeFromCLSID(new Guid("00853A19-BD51-419B-9269-2DABE57EB61F"));

// Try all overloads of Connect_Net
Console.WriteLine("=== Trying Connect_Net with various params ===");
dynamic dev = Activator.CreateInstance(t);
try
{
    bool ok = dev.Connect_Net("192.168.1.201", 4370);
    Console.WriteLine($"Connect_Net(ip, port): {ok}");
}
catch (Exception ex) { Console.WriteLine($"Connect_Net(ip, port): {ex.GetType().Name}: {ex.Message}"); }
dev.Disconnect();

dev = Activator.CreateInstance(t);
try
{
    bool ok = dev.Connect_Net("192.168.1.201", 4370, 0);
    Console.WriteLine($"Connect_Net(ip, port, password=0): {ok}");
}
catch (Exception ex) { Console.WriteLine($"Connect_Net(ip, port, password=0): {ex.GetType().Name}: {ex.Message}"); }
dev.Disconnect();

// Test with password 1
dev = Activator.CreateInstance(t);
try
{
    bool ok = dev.Connect_Net("192.168.1.201", 4370, 1);
    Console.WriteLine($"Connect_Net(ip, port, password=1): {ok}");
}
catch (Exception ex) { Console.WriteLine($"Connect_Net(ip, port, password=1): {ex.GetType().Name}: {ex.Message}"); }
dev.Disconnect();

// Now try TCP with password 1
Console.WriteLine("\n=== TCP connect with password=1 ===");
using var tcp = new System.Net.Sockets.TcpClient();
tcp.Connect("192.168.1.201", 4370);
var stream = tcp.GetStream();
if (stream.DataAvailable) _ = stream.Read(new byte[8192], 0, 8192);

ushort sid = 0, rid = 65534;
void NextRid() { rid++; if (rid >= 65535) rid = 0; }

byte[] BuildPkt(ushort cmd, byte[] data)
{
    var pl = new byte[8 + data.Length];
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pl.AsSpan(0, 2), cmd);
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pl.AsSpan(4, 2), sid);
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pl.AsSpan(6, 2), rid);
    data.CopyTo(pl, 8);
    ushort chk = 0;
    for (int i = 0; i + 1 < pl.Length; i += 2)
        chk += System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(pl.AsSpan(i, 2));
    if (pl.Length % 2 != 0) chk += pl[^1];
    chk = (chk >> 16) + (chk & 0xFFFF); chk += (chk >> 16);
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pl.AsSpan(2, 2), (ushort)(~chk & 0xFFFF));
    var pkt = new byte[8 + pl.Length];
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pkt.AsSpan(0, 2), 0x5050);
    System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(pkt.AsSpan(2, 2), 0x7D82);
    System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(pkt.AsSpan(4, 4), (uint)pl.Length);
    pl.CopyTo(pkt, 8);
    return pkt;
}

(ushort Code, byte[] Payload) Send(ushort cmd, byte[] data)
{
    NextRid();
    var pkt = BuildPkt(cmd, data);
    stream.Write(pkt, 0, pkt.Length);
    byte[] hdr = new byte[8]; int r = 0;
    while (r < 8) r += stream.Read(hdr, r, 8 - r);
    uint plen = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(hdr.AsSpan(4));
    byte[] pl = new byte[plen]; r = 0;
    while (r < plen) r += stream.Read(pl, r, (int)plen - r);
    ushort code = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(pl.AsSpan(0, 2));
    return (code, pl);
}

// Try passwords 0, 1, 12345, 999999
foreach (int pw in new[] { 0, 1, 12345 })
{
    Console.WriteLine($"\n--- TCP with password={pw} ---");
    // Reconnect
    tcp.Close();
    await Task.Delay(500);
    tcp = new System.Net.Sockets.TcpClient();
    tcp.Connect("192.168.1.201", 4370);
    stream = tcp.GetStream();
    if (stream.DataAvailable) _ = stream.Read(new byte[8192], 0, 8192);
    sid = 0; rid = 65534;

    var cResp = Send(1000, Array.Empty<byte>());
    sid = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(cResp.Payload.AsSpan(4, 2));
    Console.WriteLine($"Connect: Code={cResp.Code} Session={sid}");

    if (cResp.Code == 2005)
    {
        uint k = 0;
        for (int i = 0; i < 32; i++)
            if ((pw & (1 << i)) != 0) k = (k << 1) | 1; else k <<= 1;
        k += sid;
        byte[] b = System.BitConverter.GetBytes(k);
        b[0] ^= (byte)'Z'; b[1] ^= (byte)'K'; b[2] ^= (byte)'S'; b[3] ^= (byte)'O';
        byte t2 = b[0]; b[0] = b[2]; b[2] = t2;
        t2 = b[1]; b[1] = b[3]; b[3] = t2;
        byte bb = (byte)(0xFF & 50);
        b[0] ^= bb; b[1] ^= bb; b[3] ^= bb;
        Console.WriteLine($"Auth key: {System.BitConverter.ToString(b)}");

        var aResp = Send(1102, b);
        Console.WriteLine($"Auth: Code={aResp.Code}");

        if (aResp.Code == 2000)
        {
            Console.WriteLine("AUTH OK! Trying to read attendance...");
        }
    }
}

tcp.Close();
Console.WriteLine("\nDone.");
