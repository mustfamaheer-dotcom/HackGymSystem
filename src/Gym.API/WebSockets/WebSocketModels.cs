using System.Text.Json.Serialization;

namespace Gym.API.WebSockets;

public class WebSocketMessage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("messageId")]
    public string? MessageId { get; set; }

    [JsonPropertyName("payload")]
    public System.Text.Json.JsonElement? Payload { get; set; }
}

public class WebSocketAck
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "ack";

    [JsonPropertyName("messageId")]
    public string? MessageId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "ok";

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class AttendancePushPayload
{
    [JsonPropertyName("enrollmentId")]
    public string EnrollmentId { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("direction")]
    public int Direction { get; set; }

    [JsonPropertyName("verifyMethod")]
    public int VerifyMethod { get; set; }
}

public class HeartbeatPayload
{
    [JsonPropertyName("enrolledUserCount")]
    public int EnrolledUserCount { get; set; }

    [JsonPropertyName("freeMemory")]
    public long FreeMemory { get; set; }

    [JsonPropertyName("firmwareVersion")]
    public string FirmwareVersion { get; set; } = string.Empty;

    [JsonPropertyName("isConnected")]
    public bool IsConnected { get; set; }

    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; } = string.Empty;

    [JsonPropertyName("port")]
    public int Port { get; set; }
}

public class DeviceInfoPayload
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; } = string.Empty;

    [JsonPropertyName("firmwareVersion")]
    public string FirmwareVersion { get; set; } = string.Empty;

    [JsonPropertyName("enrolledUserCount")]
    public int EnrolledUserCount { get; set; }

    [JsonPropertyName("freeMemory")]
    public long FreeMemory { get; set; }

    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; } = string.Empty;

    [JsonPropertyName("port")]
    public int Port { get; set; }
}

public class SyncUserPayload
{
    [JsonPropertyName("enrollmentId")]
    public string EnrollmentId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("privilege")]
    public int Privilege { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}
