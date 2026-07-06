using HackGym.ZKTeco.Bridge;
using HackGym.ZKTeco.Bridge.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "HackGym ZKTeco Bridge";
});

builder.Services.Configure<ZKTecoConfig>(builder.Configuration.GetSection("ZKTeco"));

builder.Services.AddGrpc();
builder.Services.AddHttpClient("MainApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MainApi:BaseUrl") ?? "http://localhost:5000");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddSingleton<ZKDeviceManager>();
builder.Services.AddSingleton<DeviceHealthMonitor>();
builder.Services.AddHostedService<AttendancePollingWorker>();
builder.Services.AddHostedService<DeviceHealthMonitor>();

var host = builder.Build();

// Connect to device on startup
var deviceManager = host.Services.GetRequiredService<ZKDeviceManager>();
var logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
if (deviceManager.Connect())
{
    logger.LogInformation("Connected to ZKTeco device on startup");
}
else
{
    logger.LogWarning("Failed to connect to ZKTeco device on startup, will retry in background");
}

host.Run();
