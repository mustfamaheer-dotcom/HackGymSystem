================================================
  Gym Management System - Deployment Guide
================================================

Thank you for using the Gym Management System!

This package contains everything needed to run the
application on your Windows 10/11 computer.

------------------------------------------------
  SYSTEM REQUIREMENTS
------------------------------------------------
- Windows 10 or 11 (64-bit)
- .NET 10 Runtime (included in the package)
- SQL Server LocalDB 2019+ (included with Visual Studio
  or available from https://go.microsoft.com/fwlink/?linkid=866658)

If LocalDB is not installed, the system will NOT start.
Contact your IT support to install "SQL Server LocalDB".

------------------------------------------------
  QUICK START (recommended)
------------------------------------------------
1. Double-click "run.bat"
2. A browser will open at http://localhost:5000
3. Log in with your admin credentials (provided separately)

The first run will automatically create the database
and apply all required migrations.

------------------------------------------------
  RUNNING AS A WINDOWS SERVICE
------------------------------------------------
If you want the system to start automatically when
Windows boots:

1. Right-click "install-service.bat" and select
   "Run as administrator"
2. The service "Gym Management API" will be registered
   and started.
3. Access the system at http://localhost:5000

To remove the service:
1. Right-click "uninstall-service.bat" and select
   "Run as administrator"

------------------------------------------------
  CONFIGURATION
------------------------------------------------
- Port: Edit run.bat (change the PORT variable) or
  appsettings.json (Kestrel section). Default is 5000.
- Database: Edit appsettings.json (ConnectionStrings
  section). Default uses LocalDB.
- JWT Secret: Set the JWT__Secret environment variable
  to a string of at least 32 characters. If not set,
  the system will not start for security reasons.

------------------------------------------------
  FILES IN THIS FOLDER
------------------------------------------------
Gym.API.exe        - The main application (do not delete)
appsettings.json   - Configuration file (edit with Notepad)
wwwroot/           - Static files (logos, icons, etc.)
bridge/            - ZKTeco Bridge service (for MB2000 biometric device)
bridge/appsettings.json - Edit device IP and port here (default: 192.168.1.201:4370)
zkemkeeper/        - Optional fingerprint device driver
run.bat            - One-click launcher (double-click me!)
install-service.bat - Install as Windows Service
uninstall-service.bat - Remove Windows Service
logs/              - Application logs (created automatically)

------------------------------------------------
  TROUBLESHOOTING
------------------------------------------------
Problem: "Cannot connect to LocalDB"
  -> Install SQL Server LocalDB 2019 or later.
  -> Run "sqllocaldb start MSSQLLocalDB" in a terminal.

Problem: Port 5000 is already in use
  -> run.bat will automatically try the next port.

Problem: Browser does not open
  -> Manually open http://localhost:5000 (or the port
     shown in the console window).

Problem: "JWT secret is not configured"
  -> Set the JWT__Secret environment variable:
     1. Open a Command Prompt as Administrator
     2. Run: setx JWT__Secret "your-32-char-secret-here!" /M
     3. Restart the application.

For further support, contact your system administrator.
