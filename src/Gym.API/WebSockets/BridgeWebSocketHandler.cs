using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Gym.API.Services;
using Microsoft.EntityFrameworkCore;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Services.ZKTeco;
using Gym.Shared.Enums;

namespace Gym.API.WebSockets;

public class BridgeWebSocketHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BridgeWebSocketHandler> _logger;

    private static WebSocket? _activeBridge;
    private static readonly object _bridgeLock = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public BridgeWebSocketHandler(
        IServiceScopeFactory scopeFactory,
        ILogger<BridgeWebSocketHandler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context, CancellationToken ct)
    {
        var config = context.RequestServices.GetRequiredService<Microsoft.Extensions.Options.IOptions<ZKTecoBridgeOptions>>().Value;
        var expectedKey = config.ApiKey;
        var providedKey = context.Request.Headers["X-API-Key"].FirstOrDefault();

        if (string.IsNullOrEmpty(providedKey) || providedKey != expectedKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized", ct);
            return;
        }

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }

        var ws = await context.WebSockets.AcceptWebSocketAsync();

        lock (_bridgeLock)
        {
            if (_activeBridge?.State == WebSocketState.Open)
            {
                try { _activeBridge.CloseAsync(WebSocketCloseStatus.NormalClosure, "Replaced", CancellationToken.None).Wait(1000); } catch { }
            }
            _activeBridge = ws;
        }

        _logger.LogInformation("Bridge WebSocket connected");

        try
        {
            await ReceiveLoop(ws, ct);
        }
        catch (WebSocketException ex)
        {
            _logger.LogWarning(ex, "Bridge WebSocket connection lost");
        }
        finally
        {
            lock (_bridgeLock)
            {
                if (_activeBridge == ws)
                    _activeBridge = null;
            }
            _logger.LogInformation("Bridge WebSocket disconnected");
        }
    }

    private async Task ReceiveLoop(WebSocket ws, CancellationToken ct)
    {
        var buffer = new byte[8192];
        var messageBuilder = new StringBuilder();

        while (ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
            }
            catch (WebSocketException)
            {
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                try { await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None); } catch { }
                break;
            }

            messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

            if (result.EndOfMessage)
            {
                var json = messageBuilder.ToString();
                messageBuilder.Clear();
                await ProcessMessage(ws, json, ct);
            }
        }
    }

    private async Task ProcessMessage(WebSocket ws, string json, CancellationToken ct)
    {
        WebSocketMessage? msg;
        try
        {
            msg = JsonSerializer.Deserialize<WebSocketMessage>(json, _jsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid JSON from Bridge");
            await SendAck(ws, null, "error", "Invalid JSON", ct);
            return;
        }

        if (msg == null || string.IsNullOrEmpty(msg.Type))
        {
            await SendAck(ws, null, "error", "Missing type", ct);
            return;
        }

        try
        {
            switch (msg.Type)
            {
                case "attendance_push":
                    await HandleAttendancePush(ws, msg, ct);
                    break;
                case "heartbeat":
                    await HandleHeartbeat(msg, ct);
                    await SendAck(ws, msg.MessageId, "ok", null, ct);
                    break;
                case "device_offline":
                    await HandleDeviceOffline(msg, ct);
                    await SendAck(ws, msg.MessageId, "ok", null, ct);
                    break;
                case "sync_device_info":
                    await HandleDeviceInfo(msg, ct);
                    await SendAck(ws, msg.MessageId, "ok", null, ct);
                    break;
                case "sync_users":
                    await HandleSyncUsers(msg, ct);
                    await SendAck(ws, msg.MessageId, "ok", null, ct);
                    break;
                default:
                    _logger.LogWarning("Unknown WS message type: {Type}", msg.Type);
                    await SendAck(ws, msg.MessageId, "error", $"Unknown type: {msg.Type}", ct);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing {Type} message", msg.Type);
            await SendAck(ws, msg.MessageId, "error", ex.Message, ct);
        }
    }

    private async Task HandleAttendancePush(WebSocket ws, WebSocketMessage msg, CancellationToken ct)
    {
        var payload = msg.Payload?.Deserialize<AttendancePushPayload>(_jsonOptions);
        if (payload == null || string.IsNullOrEmpty(payload.EnrollmentId))
        {
            _logger.LogWarning("attendance_push received with invalid payload");
            await SendAck(ws, msg.MessageId, "error", "Invalid payload", ct);
            return;
        }

        _logger.LogInformation("attendance_push received: EnrollmentId={EnrollmentId}, Direction={Direction}, Timestamp={Timestamp}",
            payload.EnrollmentId, payload.Direction, payload.Timestamp);

        using var scope = _scopeFactory.CreateScope();
        var attendancePush = scope.ServiceProvider.GetRequiredService<AttendancePushService>();
        var result = await attendancePush.ProcessAttendanceAsync(payload, ct);

        if (result.Success)
        {
            _logger.LogInformation("attendance_push processed OK: EnrollmentId={EnrollmentId}, Type={Type}, AttendanceId={AttendanceId}",
                payload.EnrollmentId, result.Type, result.AttendanceId);
            await SendAck(ws, msg.MessageId, "ok", null, ct);
        }
        else
        {
            _logger.LogWarning("attendance_push FAILED: EnrollmentId={EnrollmentId}, Error={Error}",
                payload.EnrollmentId, result.Error);
            await SendAck(ws, msg.MessageId, "error", result.Error, ct);
        }
    }

    private async Task HandleHeartbeat(WebSocketMessage msg, CancellationToken ct)
    {
        var payload = msg.Payload?.Deserialize<HeartbeatPayload>(_jsonOptions);
        if (payload == null) return;

        using var scope = _scopeFactory.CreateScope();
        var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();

        var device = await deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == payload.IpAddress, ct);
        if (device == null)
        {
            device = new Device("ZKMB2000", payload.IpAddress, payload.Port, "ZKMB2000", "");
            await deviceRepo.AddAsync(device, ct);
        }
        device.MarkOnline();
        deviceRepo.Update(device);
    }

    private async Task HandleDeviceOffline(WebSocketMessage msg, CancellationToken ct)
    {
        var payload = msg.Payload?.Deserialize<HeartbeatPayload>(_jsonOptions);
        if (payload == null) return;

        using var scope = _scopeFactory.CreateScope();
        var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();

        var device = await deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == payload.IpAddress, ct);
        if (device != null)
        {
            device.MarkOffline();
            deviceRepo.Update(device);
        }
    }

    private async Task HandleDeviceInfo(WebSocketMessage msg, CancellationToken ct)
    {
        var payload = msg.Payload?.Deserialize<DeviceInfoPayload>(_jsonOptions);
        if (payload == null) return;

        using var scope = _scopeFactory.CreateScope();
        var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();

        var device = await deviceRepo.FirstOrDefaultAsync(d => d.IPAddress == payload.IpAddress, ct);
        if (device == null)
        {
            device = new Device(payload.Model, payload.IpAddress, payload.Port, payload.Model, payload.SerialNumber);
            device.UpdateFirmware(payload.FirmwareVersion);
            device.MarkOnline();
            await deviceRepo.AddAsync(device, ct);
        }
        else
        {
            device.UpdateFirmware(payload.FirmwareVersion);
            device.MarkOnline();
            if (device.SerialNumber != payload.SerialNumber)
                device.SerialNumber = payload.SerialNumber;
            deviceRepo.Update(device);
        }
    }

    private async Task HandleSyncUsers(WebSocketMessage msg, CancellationToken ct)
    {
        var users = msg.Payload?.Deserialize<List<SyncUserPayload>>(_jsonOptions);
        if (users == null || users.Count == 0) return;

        using var scope = _scopeFactory.CreateScope();
        var mappingRepo = scope.ServiceProvider.GetRequiredService<IDeviceMemberMappingRepository>();
        var memberRepo = scope.ServiceProvider.GetRequiredService<IRepository<Member>>();

        var lastCode = await memberRepo.Query().IgnoreQueryFilters().MaxAsync(m => (int?)m.Code, ct) ?? 0;

        foreach (var user in users)
        {
            var existingMapping = await mappingRepo.GetByEnrollmentIdAsync(user.EnrollmentId, ct);
            if (existingMapping != null) continue;

            var existingMember = await memberRepo.FirstOrDefaultAsync(m => m.FullName == user.Name && !m.IsDeleted, ct);
            Guid memberId;
            if (existingMember != null)
            {
                memberId = existingMember.Id;
            }
            else
            {
                lastCode++;
                var newMember = new Member($"AUTO-{user.EnrollmentId}", user.Name, $"D{user.EnrollmentId}", DateTime.UtcNow) { Code = lastCode, NationalId = $"D{user.EnrollmentId}" };
                await memberRepo.AddAsync(newMember, ct);
                memberId = newMember.Id;
            }

            var mapping = new DeviceMemberMapping(memberId, user.EnrollmentId, BiometricType.Face, null);
            await mappingRepo.SaveMappingAsync(mapping, ct);
        }
    }

    private async Task SendAck(WebSocket ws, string? messageId, string status, string? error, CancellationToken ct)
    {
        var ack = new WebSocketAck
        {
            MessageId = messageId,
            Status = status,
            Error = error
        };
        var json = JsonSerializer.Serialize(ack, _jsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        try
        {
            if (ws.State == WebSocketState.Open)
                await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
        }
        catch { }
    }

    public static async Task SendToBridge(string json, CancellationToken ct)
    {
        WebSocket? ws;
        lock (_bridgeLock)
        {
            ws = _activeBridge;
        }
        if (ws == null || ws.State != WebSocketState.Open) return;

        var bytes = Encoding.UTF8.GetBytes(json);
        try
        {
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
        }
        catch
        {
            lock (_bridgeLock)
            {
                if (_activeBridge == ws)
                    _activeBridge = null;
            }
        }
    }
}
