namespace HackGym.ZKTeco.Bridge.Protocol;

/// <summary>
/// ZKTeco binary protocol constants (extracted from zkprotocol/pyzk).
/// No COM DLL required — pure TCP sockets.
/// </summary>
internal static class ZKProtocol
{
    // --- Packet header ---
    public const ushort Magic1 = 0x5050; // first 2 bytes
    public const ushort Magic2 = 0x827D; // next 2 bytes

    // --- Command IDs ---
    public const ushort CMD_CONNECT = 1000;
    public const ushort CMD_EXIT = 1001;
    public const ushort CMD_ENABLEDEVICE = 1002;
    public const ushort CMD_DISABLEDEVICE = 1003;
    public const ushort CMD_DB_RRQ = 7;
    public const ushort CMD_USERTEMP_RRQ = 9;
    public const ushort CMD_OPTIONS_RRQ = 11;
    public const ushort CMD_ATTLOG_RRQ = 13;
    public const ushort CMD_CLEAR_ATTLOG = 15;
    public const ushort CMD_GET_FREE_SIZES = 50;
    public const ushort CMD_GET_VERSION = 1100;
    public const ushort CMD_AUTH = 1102;
    public const ushort CMD_REG_EVENT = 500;
    public const ushort CMD_PREPARE_DATA = 1500;
    public const ushort CMD_DATA = 1501;
    public const ushort CMD_FREE_DATA = 1502;

    // --- Reply codes ---
    public const ushort CMD_ACK_OK = 2000;
    public const ushort CMD_ACK_ERROR = 2001;
    public const ushort CMD_ACK_DATA = 2002;
    public const ushort CMD_ACK_UNAUTH = 2005;

    // --- Data function codes ---
    public const int FCT_USER = 5;
    public const int FCT_ATTLOG = 1;

    // --- Buffered transfer commands (for newer devices like MB2000) ---
    public const ushort CMD_PREPARE_BUFFER = 1503;
    public const ushort CMD_READ_BUFFER = 1504;

    // --- Event flags ---
    public const int EF_ATTLOG = 1;
    public const int EF_FINGER = 1 << 1;
    public const int EF_VERIFY = 1 << 7;
    public const int EF_ALARM = 1 << 9;

    public const ushort USHRT_MAX = 65535;

    /// <summary>
    /// Calculate the ZK protocol checksum over a payload (without the TCP top header).
    /// </summary>
    public static ushort CalculateChecksum(byte[] payload)
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

    /// <summary>
    /// Build a complete TCP packet: 8-byte top header + payload.
    /// </summary>
    public static byte[] BuildPacket(ushort command, byte[] data, ushort sessionId, ushort replyId)
    {
        // payload = [command(2) + checksum(2) + session(2) + replyId(2) + data]
        var payload = new byte[8 + data.Length];
        BitConverter.GetBytes(command).CopyTo(payload, 0);
        // checksum placeholder at [2..3]
        BitConverter.GetBytes(sessionId).CopyTo(payload, 4);
        BitConverter.GetBytes(replyId).CopyTo(payload, 6);
        data.CopyTo(payload, 8);

        // Calculate checksum over the payload (command + placeholder + session + replyId + data)
        ushort chk = CalculateChecksum(payload);
        BitConverter.GetBytes(chk).CopyTo(payload, 2);

        // TCP top header: [Magic1(2) + Magic2(2) + payloadLen(4 LE)]
        var packet = new byte[8 + payload.Length];
        BitConverter.GetBytes(Magic1).CopyTo(packet, 0);
        BitConverter.GetBytes(Magic2).CopyTo(packet, 2);
        BitConverter.GetBytes((uint)payload.Length).CopyTo(packet, 4);
        payload.CopyTo(packet, 8);

        return packet;
    }

    /// <summary>
    /// Create the commkey bytes for CMD_AUTH based on password and session id.
    /// Port of make_commkey() from pyzk.
    /// </summary>
    public static byte[] MakeCommKey(int key, int sessionId, int ticks = 50)
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
        // XOR with 'Z','K','S','O'
        kBytes[0] ^= (byte)'Z';
        kBytes[1] ^= (byte)'K';
        kBytes[2] ^= (byte)'S';
        kBytes[3] ^= (byte)'O';

        // Swap pairs
        byte tmp = kBytes[0]; kBytes[0] = kBytes[2]; kBytes[2] = tmp;
        tmp = kBytes[1]; kBytes[1] = kBytes[3]; kBytes[3] = tmp;

        byte b = (byte)(0xFF & ticks);
        kBytes[0] ^= b;
        kBytes[1] ^= b;
        // kBytes[2] = b;  // replaced below
        kBytes[3] ^= b;

        return new byte[] { kBytes[0], kBytes[1], b, kBytes[3] };
    }

    /// <summary>
    /// Decode a 4-byte ZK timestamp into a DateTime.
    /// </summary>
    public static DateTime DecodeTime(byte[] t)
    {
        uint val = BitConverter.ToUInt32(t, 0);
        int second = (int)(val % 60); val /= 60;
        int minute = (int)(val % 60); val /= 60;
        int hour = (int)(val % 24); val /= 24;
        int day = (int)(val % 31) + 1; val /= 31;
        int month = (int)(val % 12) + 1; val /= 12;
        int year = (int)val + 2000;

        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
    }

    /// <summary>
    /// Decode a 6-byte hex timestamp (used in attendance log records).
    /// </summary>
    public static DateTime DecodeTimeHex(byte[] hex)
    {
        int year = hex[0] + 2000;
        int month = hex[1];
        int day = hex[2];
        int hour = hex[3];
        int minute = hex[4];
        int second = hex[5];
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
    }
}
