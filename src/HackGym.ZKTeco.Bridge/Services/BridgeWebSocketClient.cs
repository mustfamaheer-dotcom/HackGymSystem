using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using HackGym.ZKTeco.Bridge.Models;

namespace HackGym.ZKTeco.Bridge.Services;

public class BridgeWebSocketClient : IAsyncDisposable
{
    private readonly ILogger<BridgeWebSocketClient> _logger;
    private readonly ZKTecoConfig _config;
    private readonly string _wsUrl;
    private readonly string _apiKey;
    private ClientWebSocket? _ws;
    private CancellationTokenSource? _receiveCts;
    private Task? _receiveTask;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly ConcurrentQueue<(string Json, TaskCompletionSource<bool>? Tcs)> _sendQueue = new();
    private bool _isConnected;
    private const int SendBufferLimit = 1000;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public bool IsConnected => _isConnected;

    public event Func<string, JsonElement?, Task>? OnCommand;

    public BridgeWebSocketClient(
        ILogger<BridgeWebSocketClient> logger,
        Microsoft.Extensions.Options.IOptions<ZKTecoConfig> config)
    {
        _logger = logger;
        _config = config.Value;
        _wsUrl = config.Value.GymApiWebSocketUrl ?? "ws://localhost:5000/ws/bridge";
        _apiKey = config.Value.MainApiKey ?? "";
    }

    public async Task ConnectAsync(CancellationToken ct)
    {
        if (_isConnected) return;

        try
        {
            _ws?.Dispose();
            _ws = new ClientWebSocket();
            _ws.Options.SetRequestHeader("X-API-Key", _apiKey);

            _logger.LogInformation("Connecting to Gym API WebSocket at {Url}", _wsUrl);
            await _ws.ConnectAsync(new Uri(_wsUrl), ct);

            _isConnected = true;
            _logger.LogInformation("Connected to Gym API WebSocket");

            // Start receive loop
            _receiveCts?.Cancel();
            _receiveCts = new CancellationTokenSource();
            _receiveTask = ReceiveLoopAsync(_receiveCts.Token);

            // Flush any queued messages
            await FlushSendQueueAsync(ct);
        }
        catch (Exception ex)
        {
            // Expected during API startup (connection refused). The hosted service handles
            // reconnection with exponential backoff; keep this quiet to avoid log spam.
            _logger.LogDebug(ex, "Failed to connect to Gym API WebSocket");
            _isConnected = false;
            throw;
        }
    }

    public async Task SendMessageAsync(string type, object? payload, CancellationToken ct = default)
    {
        var msg = new Dictionary<string, object?>
        {
            ["type"] = type,
            ["messageId"] = Guid.NewGuid().ToString("N"),
            ["payload"] = payload
        };
        var json = JsonSerializer.Serialize(msg, _jsonOptions);

        if (!_isConnected || _ws?.State != WebSocketState.Open)
        {
            if (_sendQueue.Count < SendBufferLimit)
                _sendQueue.Enqueue((json, null));
            return;
        }

        try
        {
            await _sendLock.WaitAsync(ct);
            try
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
            }
            finally
            {
                _sendLock.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "WS send failed, queueing message");
            if (_sendQueue.Count < SendBufferLimit)
                _sendQueue.Enqueue((json, null));
            _isConnected = false;
        }
    }

    public async Task DisconnectAsync()
    {
        _receiveCts?.Cancel();
        if (_ws?.State == WebSocketState.Open)
        {
            try
            {
                await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown", CancellationToken.None);
            }
            catch { }
        }
        _isConnected = false;
        if (_receiveTask != null)
        {
            try { await _receiveTask; } catch { }
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken ct)
    {
        var buffer = new byte[8192];
        var messageBuilder = new StringBuilder();

        try
        {
            while (!ct.IsCancellationRequested && _ws?.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                try
                {
                    result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                }
                catch (OperationCanceledException) { break; }
                catch (WebSocketException) { break; }

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.EndOfMessage)
                {
                    var json = messageBuilder.ToString();
                    messageBuilder.Clear();

                    try
                    {
                        using var doc = JsonDocument.Parse(json);
                        var type = doc.RootElement.GetProperty("type").GetString();

                        if (type == "ack") continue; // ignore acks from API

                        if (OnCommand != null && type != null)
                        {
                            var payload = doc.RootElement.TryGetProperty("payload", out var p) ? p : (JsonElement?)null;
                            await OnCommand.Invoke(type, payload);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning(ex, "Invalid JSON from Gym API");
                    }
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "WebSocket receive error");
        }
        finally
        {
            _isConnected = false;
        }
    }

    private async Task FlushSendQueueAsync(CancellationToken ct)
    {
        while (_sendQueue.TryDequeue(out var item))
        {
            try
            {
                await _sendLock.WaitAsync(ct);
                try
                {
                    if (_ws?.State == WebSocketState.Open)
                    {
                        var bytes = Encoding.UTF8.GetBytes(item.Json);
                        await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
                    }
                }
                finally
                {
                    _sendLock.Release();
                }
            }
            catch
            {
                _sendQueue.Enqueue(item);
                break;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
        _sendLock.Dispose();
        _ws?.Dispose();
        _receiveCts?.Cancel();
        _receiveCts?.Dispose();
    }
}
