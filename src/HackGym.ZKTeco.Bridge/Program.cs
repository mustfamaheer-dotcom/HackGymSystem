using HackGym.ZKTeco.Bridge;
using HackGym.ZKTeco.Bridge.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "HackGym ZKTeco Bridge";
});

builder.Services.Configure<ZKTecoConfig>(builder.Configuration.GetSection("ZKTeco"));
var mainApiKey = builder.Configuration.GetValue<string>("MainApi:ApiKey") ?? "";
builder.Services.AddHttpClient("MainApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MainApi:BaseUrl") ?? "http://localhost:5000");
    client.Timeout = TimeSpan.FromSeconds(10);
    if (!string.IsNullOrEmpty(mainApiKey))
        client.DefaultRequestHeaders.Add("X-API-Key", mainApiKey);
});

builder.Services.AddSingleton<ZKDeviceManager>();
builder.Services.AddSingleton<BridgeWebSocketClient>();
builder.Services.AddSingleton<DeviceHealthMonitor>();
builder.Services.AddHostedService<AttendancePollingWorker>();
builder.Services.AddHostedService<DeviceHealthMonitor>();
builder.Services.AddHostedService<BridgeWebSocketHostedService>();

var app = builder.Build();

var deviceManager = app.Services.GetRequiredService<ZKDeviceManager>();

var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    if (deviceManager.Connect())
        startupLogger.LogInformation("Connected to ZKTeco device on startup");
    else
        startupLogger.LogWarning("Failed to connect to ZKTeco device on startup, will retry in background");
}
catch (Exception ex)
{
    startupLogger.LogWarning(ex, "Failed to connect to ZKTeco device on startup");
}

app.MapPost("/zkteco.bridge.ZKTecoBridge/TestConnection", () =>
{
    var (success, latencyMs) = deviceManager.TestConnection();
    return Results.Ok(new
    {
        success,
        roundTripLatencyMs = latencyMs,
        errorMessage = success ? null : "Connection test failed"
    });
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/GetDeviceStatus", () =>
{
    var (enrolledCount, freeMemory, firmwareVersion) = deviceManager.IsConnected
        ? deviceManager.GetDeviceStatus()
        : (0, 0L, null);
    return Results.Ok(new
    {
        isConnected = deviceManager.IsConnected,
        enrolledUserCount = enrolledCount,
        freeMemory,
        firmwareVersion = firmwareVersion ?? "",
        maxFingerprints = 3000,
        maxFaces = 500
    });
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/CheckHealth", () =>
{
    var (enrolledCount, freeMemory, firmwareVersion) = deviceManager.IsConnected
        ? deviceManager.GetDeviceStatus()
        : (0, 0L, null);
    return Results.Ok(new
    {
        isConnected = deviceManager.IsConnected,
        enrolledUserCount = enrolledCount,
        freeMemory,
        firmwareVersion = firmwareVersion ?? "",
        uptimeMs = deviceManager.IsConnected
            ? (long)(DateTime.UtcNow - deviceManager.ConnectionInfo.LastConnectedAt).TotalMilliseconds
            : 0L
    });
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/ReconcileUsers", () =>
{
    try
    {
        var (deviceIds, success) = deviceManager.GetAllUserIds();
        return Results.Ok(new
        {
            success,
            usersChecked = deviceIds.Count,
            discrepanciesFixed = 0,
            details = success ? Array.Empty<string>() : new[] { "Failed to read users from device" }
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, usersChecked = 0, discrepanciesFixed = 0, details = new[] { ex.Message } });
    }
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/SetUserPrivilege", async (HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body);

        var enrollmentId = data.GetProperty("enrollmentId").GetString() ?? "";
        var privilege = data.GetProperty("privilege").GetInt32();
        var enableExpiry = data.GetProperty("enableExpiry").GetBoolean();
        var expiryYear = data.TryGetProperty("expiryYear", out var ey) ? ey.GetInt32() : 0;
        var expiryMonth = data.TryGetProperty("expiryMonth", out var em) ? em.GetInt32() : 0;
        var expiryDay = data.TryGetProperty("expiryDay", out var ed) ? ed.GetInt32() : 0;

        DateTime? expiry = enableExpiry ? new DateTime(expiryYear, expiryMonth, expiryDay, 23, 59, 59, DateTimeKind.Local) : null;
        var success = deviceManager.SetUserPrivilege(enrollmentId, privilege, expiry);
        return Results.Ok(new { success, errorMessage = success ? null : "Failed to set privilege on device" });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, errorMessage = ex.Message });
    }
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/DeleteUser", async (HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body);
        var enrollmentId = data.GetProperty("enrollmentId").GetString() ?? "";
        var success = deviceManager.DeleteUser(enrollmentId);
        return Results.Ok(new { success, errorMessage = success ? null : "Failed to delete user from device" });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, errorMessage = ex.Message });
    }
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/EnrollFingerprint", async (HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body);
        var memberId = data.GetProperty("memberId").GetString() ?? "";
        var enrollmentId = data.GetProperty("enrollmentId").GetString() ?? "";
        var fingerIndex = data.GetProperty("fingerIndex").GetInt32();
        var timeoutSeconds = data.TryGetProperty("timeoutSeconds", out var ts) ? ts.GetInt32() : 60;

        var (success, error) = deviceManager.EnrollFingerprint(enrollmentId, $"Member_{memberId}", fingerIndex, timeoutSeconds);
        return Results.Ok(new { success, errorMessage = error ?? "" });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, errorMessage = ex.Message });
    }
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/EnrollFace", async (HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body);
        var memberId = data.GetProperty("memberId").GetString() ?? "";
        var enrollmentId = data.GetProperty("enrollmentId").GetString() ?? "";
        var timeoutSeconds = data.TryGetProperty("timeoutSeconds", out var ts) ? ts.GetInt32() : 60;

        var (success, error) = deviceManager.EnrollFace(enrollmentId, $"Member_{memberId}", timeoutSeconds);
        return Results.Ok(new { success, errorMessage = error ?? "" });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { success = false, errorMessage = ex.Message });
    }
});

app.MapPost("/zkteco.bridge.ZKTecoBridge/DiagnoseUsers", () =>
{
    if (!deviceManager.IsConnected)
        return Results.Ok(new { connected = false, error = "Device not connected" });

    try
    {
        var users = deviceManager.GetAllUsersWithDetails();
        var ids = users.Take(20).Select(u => new { u.EnrollmentId, u.Name, u.Privilege });
        return Results.Ok(new
        {
            connected = true,
            userCount = users.Count,
            sampleUsers = ids,
            totalSample = users.Count
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { connected = true, error = ex.Message });
    }
});

app.MapGet("/diagnose/raw", () =>
{
    if (!deviceManager.IsConnected)
        return Results.Ok(new { connected = false, error = "Device not connected" });

    try
    {
        var diag = deviceManager.DiagnoseProtocols();
        return Results.Ok(new { connected = true, diagnostics = diag });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { connected = true, error = ex.Message, stackTrace = ex.StackTrace });
    }
});

app.MapGet("/diagnose/rawbytes", () =>
{
    try
    {
        // Fresh connect for reliable diagnostics
        deviceManager.Disconnect();
        var connected = deviceManager.Connect();
        if (!connected)
            return Results.Ok(new { connected = false, error = "Device not connected" });

        var rawDiags = new System.Collections.Generic.Dictionary<string, object>();

        // Test 1: CMD_GET_VERSION (simple command, should return version string)
        var (s1, m1, m2, ps, ph, code1, dl1, sid1, rid1) = deviceManager.TestRawSendRecv(1100, []);
        rawDiags["get_version"] = new { sendHex = s1, recvMagic1 = $"0x{m1:X4}", recvMagic2 = $"0x{m2:X4}", payloadSize = ps, payloadHex = ph, code = code1, dataLen = dl1, respSid = sid1, respRid = rid1 };

        // Test 2: CMD_GET_FREE_SIZES
        var (s2, m2_1, m2_2, ps2, ph2, code2, dl2, sid2, rid2) = deviceManager.TestRawSendRecv(50, []);
        rawDiags["get_free_sizes"] = new { sendHex = s2, recvMagic1 = $"0x{m2_1:X4}", recvMagic2 = $"0x{m2_2:X4}", payloadSize = ps2, payloadHex = ph2, code = code2, dataLen = dl2, respSid = sid2, respRid = rid2 };

        // Test 3: CMD_CONNECT fresh (raw)
        deviceManager.Disconnect();
        deviceManager.Connect();
        var (s3, m3_1, m3_2, ps3, ph3, code3, dl3, sid3, rid3) = deviceManager.TestRawSendRecv(1000, []);
        rawDiags["cmd_connect_raw"] = new { sendHex = s3, recvMagic1 = $"0x{m3_1:X4}", recvMagic2 = $"0x{m3_2:X4}", payloadSize = ps3, payloadHex = ph3, code = code3, dataLen = dl3, respSid = sid3, respRid = rid3 };

        // Test 4: CMD_GET_FREE_SIZES right after fresh connect
        var (s4, m4_1, m4_2, ps4, ph4, code4, dl4, sid4, rid4) = deviceManager.TestRawSendRecv(50, []);
        rawDiags["get_free_sizes_after_connect"] = new { sendHex = s4, recvMagic1 = $"0x{m4_1:X4}", recvMagic2 = $"0x{m4_2:X4}", payloadSize = ps4, payloadHex = ph4, code = code4, dataLen = dl4, respSid = sid4, respRid = rid4 };

        return Results.Ok(new { connected = true, rawDiagnostics = rawDiags });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { connected = true, error = ex.Message, stackTrace = ex.StackTrace });
    }
});

app.MapGet("/diagnose/getusers", () =>
{
    if (!deviceManager.IsConnected)
        return Results.Ok(new { connected = false });

    try
    {
        var users = deviceManager.GetAllUsersWithDetails();
        return Results.Ok(new
        {
            count = users.Count,
            sample = users.Take(5).Select(u => new { u.EnrollmentId, u.Name, u.Privilege })
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { error = ex.Message, stackTrace = ex.StackTrace });
    }
});

app.Run();
