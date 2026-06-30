# Client Device Deployment Guide

## Prerequisites — One-Time Setup

Install these on the client's **Windows 10/11** machine:

| Software | Download | Why |
|---|---|---|
| **.NET 10 SDK** | [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0) | Runs the backend API |
| **SQL Server LocalDB** | Included with Visual Studio or standalone [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | Stores all gym data |
| **Node.js 22+** (optional) | [nodejs.org](https://nodejs.org/) | Only needed if you want to edit the frontend |
| **ZKTeco zkemkeeper.dll** | From device CD or ZKTeco support | Required for fingerprint device communication |

### Quick Install Steps

```powershell
# 1. Install .NET SDK (download and run installer)
# 2. Install SQL Server LocalDB
# 3. Verify installations
dotnet --version       # Should show 10.x
sqlcmd -?              # Should show help
```

## Deployment — Copy Files

Copy the entire project folder to the client machine. The recommended location is:

```
C:\GymManagement\
```

## First-Time Setup

### Option A — One-Click Launcher (Recommended)

Double-click **`start-system.bat`** in the project root.

This will:
1. Check prerequisites
2. Create the database (if not exists)
3. Build the frontend
4. Start the server at `http://localhost:5000`
5. Open the browser automatically

### Option B — Manual Setup

```powershell
# 1. Create database
cd C:\GymManagement
dotnet ef database update --project src\Gym.Infrastructure --startup-project src\Gym.API

# 2. Build frontend (if you have Node.js)
cd gym-web
npm install
npm run build
cd ..

# 3. Copy frontend to API
Remove-Item -Recurse -Force src\Gym.API\wwwroot -ErrorAction SilentlyContinue
Copy-Item -Recurse gym-web\dist src\Gym.API\wwwroot

# 4. Run the API
dotnet run --project src\Gym.API --urls "http://localhost:5000"
```

## Running the System

### Everyday Use

**Double-click `start-system.bat`** on the desktop.

The system will:
- Start the API server on `http://localhost:5000`
- Open the default browser to the login page
- The console window will stay open — **keep it running** while using the system

### Default Login

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Owner (full access) |

**Important:** Change the admin password on first login.

## Setting Up the ZKTeco Device

### 1. Install zkemkeeper.dll

```powershell
# Register the COM component (run as Administrator)
C:\Windows\SysWOW64\regsvr32.exe "C:\Program Files\ZKTeco\zkemkeeper.dll"
```

### 2. Configure Device in the System

1. Login to the system at `http://localhost:5000`
2. Go to **Devices** page
3. Click **Add Device**
4. Enter:
   - **Name**: `Main Entrance`
   - **Serial Number**: from the device
   - **Device Type**: `Fingerprint`
   - **IP Address**: e.g. `192.168.1.201`
   - **Port**: `4370` (default ZKTeco port)
   - **Location**: `Main Gate`
5. Click **Create**

### 3. Test Connection

1. Go to **Settings** page
2. Find the **Device Connection Test** widget
3. Select your device from the dropdown
4. Click **Test Connection**
5. You should see: `✓ Connected to 192.168.1.201:4370 successfully`

### 4. Sync Fingerprint Data

1. In **Devices** page, click **Sync** on the device row
2. This downloads all fingerprint templates from the device
3. Members can now check in via fingerprint

## Device IP Configuration

The ZKTeco MB2000 usually has a fixed IP. To configure:

1. On the device, press **OK** to enter menu
2. Navigate to: **Settings → Network**
3. Set:
   - **IP Address**: `192.168.1.201` (choose an available IP)
   - **Subnet Mask**: `255.255.255.0`
   - **Gateway**: `192.168.1.1` (your router)
4. Save and reboot

Ensure the device and the computer are on the **same network**.

## Configuring Settings

1. Go to **Settings** page
2. Configure:

| Setting | Description | Recommended Value |
|---|---|---|
| `GymName` | Display name | Your gym name |
| `DeviceIP` | ZKTeco device IP | `192.168.1.201` |
| `DevicePort` | ZKTeco device port | `4370` |
| `WorkingHoursStart` | Gym opening time | `08:00` |
| `WorkingHoursEnd` | Gym closing time | `22:00` |
| `DefaultCurrency` | Currency symbol | `EGP` |
| `ReminderDays` | Days before expiry to remind | `7,3,1` |
| `BackupPath` | Database backup folder | `C:\GymManagement\Backups` |

## Database Backup

The system automatically backs up the database daily at 02:00 AM.

### Manual Backup

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "BACKUP DATABASE GymManagementDb TO DISK='C:\GymManagement\Backups\gym-backup.bak'"
```

### Restore from Backup

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "RESTORE DATABASE GymManagementDb FROM DISK='C:\GymManagement\Backups\gym-backup.bak' WITH REPLACE"
```

## Creating a Desktop Shortcut

1. Right-click on the desktop → **New → Shortcut**
2. Location: `C:\GymManagement\start-system.bat`
3. Name: `Gym Management System`
4. Right-click the shortcut → **Properties**
5. Click **Change Icon...** → Browse to `C:\GymManagement\icon.ico` (or choose any icon)
6. Click **OK**

Now double-click the desktop icon to start the system.

## Running as Windows Service (Advanced)

### Install the Service

```powershell
# Publish the API
dotnet publish src/Gym.API -c Release -o C:\GymManagement\api

# Create Windows Service
New-Service -Name "GymAPI" `
  -BinaryPathName "C:\GymManagement\api\Gym.API.exe --content-root C:\GymManagement\api" `
  -DisplayName "Gym Management API" `
  -StartupType Automatic

Start-Service -Name "GymAPI"
```

### Configure Auto-Start in Registry

The service will automatically start when Windows boots.

## Troubleshooting

### "Connection failed" on device test

| Cause | Solution |
|---|---|
| Wrong IP address | Check device IP in Settings or Device page |
| Firewall blocking port 4370 | Add firewall rule: `netsh advfirewall firewall add rule name="ZKTeco" dir=in action=allow protocol=TCP localport=4370` |
| Device not powered on | Check device power and network cable |
| Different subnet | Ensure PC and device are on same subnet |

### "Cannot connect to database"

| Cause | Solution |
|---|---|
| SQL Server not running | Start SQL Server: `net start MSSQL$SQLEXPRESS` or `net start MSSQLLocalDB` |
| Database not created | Run: `dotnet ef database update --project src\Gym.Infrastructure --startup-project src\Gym.API` |
| Firewall blocking SQL | Add firewall rule for SQL Server port (1433) |

### Port 5000 already in use

```powershell
# Find what's using port 5000
netstat -ano | findstr :5000

# Kill the process (replace LAST_COLUMN with the PID)
taskkill /PID LAST_COLUMN /F
```

## File Structure on Client Machine

```
C:\GymManagement\
├── start-system.bat          ← Double-click to start
├── database\
│   └── init.sql              ← SQL setup script
├── src\
│   ├── Gym.API\              ← Backend API
│   │   └── wwwroot\           ← Frontend (copied from gym-web\dist)
│   ├── Gym.Application\      ← Logic layer
│   ├── Gym.Domain\           ← Business entities
│   ├── Gym.Infrastructure\   ← Data access
│   └── Gym.Shared\           ← Shared code
├── gym-web\                   ← Frontend source
│   └── dist\                 ← Compiled frontend
├── docs\                      ← Documentation
└── logs\                      ← API logs (created at runtime)
```

## Security Checklist

- [ ] Change default admin password on first login
- [ ] Restrict physical access to the server
- [ ] Keep Windows updated
- [ ] Enable Windows Defender firewall
- [ ] Regularly backup the database
- [ ] Change the JWT secret in `appsettings.json` for production
