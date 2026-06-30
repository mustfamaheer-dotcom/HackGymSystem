# Deployment Guide

## Deployment Topology

```
┌───────────────────────────────────────────────────┐
│                  Windows Server                    │
│                                                    │
│  ┌──────────────┐     ┌──────────────────────┐    │
│  │  IIS / Kestrel │     │   SQL Server          │    │
│  │  Gym.API       │     │   (Express/Standard)  │    │
│  │  :5000         │◄────│   GymManagementDb     │    │
│  └──────┬───────┘     └──────────────────────┘    │
│         │                                          │
│  ┌──────┴───────┐     ┌──────────────────────┐    │
│  │  Gym.Web      │     │   Windows Services    │    │
│  │  (static)     │     │   ┌──────────────┐   │    │
│  │  :5173 / :80  │     │   │ DeviceService │   │    │
│  └──────────────┘     │   ├──────────────┤   │    │
│                        │   │ NotifService │   │    │
│                        │   └──────────────┘   │    │
│                        └──────────────────────┘    │
└───────────────────────────────────────────────────┘
```

## 1. Backend (Gym.API)

### Publish

```bash
dotnet publish src/Gym.API -c Release -o deploy/api
```

### Run as Windows Service

Create `gym-api-service.ps1`:

```powershell
New-Service -Name "GymAPI" `
  -BinaryPathName "C:\gym\api\Gym.API.exe --content-root C:\gym\api" `
  -DisplayName "Gym Management API" `
  -StartupType Automatic

Start-Service -Name "GymAPI"
```

### Run behind IIS

1. Install ASP.NET Core Hosting Bundle on Windows Server
2. Create IIS site pointing to `deploy/api`
3. Set Application Pool to `No Managed Code`
4. Configure URL rewrite for SPA fallback (optional)

### Environment Variables

| Variable | Description |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `JwtSettings__SecretKey` | 256-bit JWT signing key |
| `Twilio__AccountSid` | Twilio account SID |
| `Twilio__AuthToken` | Twilio auth token |
| `Twilio__FromNumber` | Twilio sender number |
| `WhatsApp__ApiToken` | WhatsApp API token |
| `WhatsApp__PhoneNumberId` | WhatsApp phone number ID |

Set via `appsettings.Production.json` or system environment variables.

## 2. Frontend (Gym.Web)

### Build

```bash
cd gym-web
npm ci
npm run build
# Output in gym-web/dist/
```

### Deploy

**Option A — Serve via Gym.API (Recommended)**

Copy `gym-web/dist/` to `src/Gym.API/wwwroot/`. The API's `Program.cs` serves static files from `wwwroot` and configures SPA fallback routing.

```csharp
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
```

**Option B — Standalone web server (IIS, Nginx)**

Copy `gym-web/dist/` to web server root. Configure reverse proxy to forward `/api/*` to `http://localhost:5000`.

Example IIS `web.config`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="SPA Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_URI}" pattern="^/api/" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
    <staticContent>
      <mimeMap fileExtension=".json" mimeType="application/json" />
    </staticContent>
  </system.webServer>
</configuration>
```

## 3. Windows Services

### Device Service

```bash
dotnet publish src/Gym.DeviceService -c Release -o deploy/device-service
```

Install:

```powershell
New-Service -Name "GymDeviceService" `
  -BinaryPathName "C:\gym\device-service\Gym.DeviceService.exe" `
  -DisplayName "Gym Device Service" `
  -Description "ZKTeco MB2000 fingerprint integration" `
  -StartupType Automatic

Start-Service -Name "GymDeviceService"
```

Requirements:
- ZKTeco MB2000 device accessible on network
- `zkemkeeper.dll` registered via `regsvr32` on the server
- COM permission granted to the service account

### Notification Service

```bash
dotnet publish src/Gym.NotificationService -c Release -o deploy/notification-service
```

Install:

```powershell
New-Service -Name "GymNotificationService" `
  -BinaryPathName "C:\gym\notification-service\Gym.NotificationService.exe" `
  -DisplayName "Gym Notification Service" `
  -Description "WhatsApp/SMS/push notification delivery" `
  -StartupType Automatic

Start-Service -Name "GymNotificationService"
```

## 4. Database

### Production Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=GymManagementDb;User Id=gymadmin;Password=<strong-password>;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

### SQL Server Configuration

- Use SQL Server Express, Standard, or Developer edition
- Enable `TCP/IP` protocol in SQL Server Configuration Manager
- Configure firewall to allow SQL Server port (default 1433)
- Set up regular backup schedule (can be automated via BackupLog + Hangfire)

### Migration Strategy

Apply migrations on deploy:

```bash
dotnet ef database update --project src/Gym.Infrastructure --startup-project src/Gym.API
```

Or run `database/init.sql` against the target database (idempotent — uses `IF NOT EXISTS` checks).

## 5. Hangfire Dashboard

Hangfire dashboard is available at `/hangfire` in the API.

**Important:** Restrict access in production:

```csharp
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
```

## 6. SignalR

SignalR hub is at `/hub/dashboard`. The React frontend connects automatically for real-time dashboard updates.

## 7. Security Checklist

- [ ] Change default admin passwords on first login
- [ ] Set strong JWT secret key (256 bits, random)
- [ ] Enable HTTPS with valid certificate
- [ ] Restrict Hangfire dashboard access
- [ ] Configure CORS to allow only the frontend origin
- [ ] Enable audit logging for sensitive operations
- [ ] Set secure flag on auth cookies if using cookies
- [ ] Configure rate limiting on auth endpoints
- [ ] Keep SQL Server port firewalled
- [ ] Run services under least-privilege service accounts

## 8. Monitoring

### Logs

Serilog writes to:
- Console (stdout)
- File (`logs/gym-api-.log`, rolling daily)

Configure file path in `appsettings.json`:

```json
{
  "Serilog": {
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/gym-api-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

### Health Checks

Add health check endpoint at `GET /health`:

```csharp
app.MapHealthChecks("/health");
```

## 9. Backup Strategy

| Frequency | Type | Tool |
|---|---|---|
| Daily | Full database backup | Hangfire job (02:00) |
| Weekly | Full backup + archive | Hangfire job |
| Monthly | Full backup + off-site copy | Manual or script |

Backups logged in `BackupLogs` table with status and file path.

## 10. Scaling Considerations

- Gym.API is stateless — can run multiple instances behind a load balancer
- SignalR requires sticky sessions (use `ServerSentEvents` or `Azure SignalR` for multiple instances)
- Hangfire can use SQL Server as a distributed queue — no additional infrastructure needed
- Static frontend files can be served via CDN for faster loading

## 11. Troubleshooting

| Issue | Likely Cause | Fix |
|---|---|---|
| 401 on API calls | Expired token | Refresh token via `/api/auth/refresh` |
| Device not connecting | Wrong IP/port or zkemkeeper.dll not registered | Verify network, run `regsvr32 zkemkeeper.dll` |
| Migration fails | Connection string or permissions | Verify SQL Server access, run `init.sql` |
| WhatsApp messages not sending | Invalid API token | Check WhatsApp Business API credentials |
| Hangfire jobs not running | Service not started or DB not accessible | Check Windows Service status, verify DB connection |
