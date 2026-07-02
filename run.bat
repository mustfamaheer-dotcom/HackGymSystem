@echo off
title Hack Gym - Launcher
color 0E
setlocal enabledelayedexpansion

:: ============================================================
::  Hack Gym - Portable Launcher
::  Works from any folder after git clone.
::  Auto-detects SQL Server (LocalDB or full instance),
::  restores packages, builds, and starts the server.
:: ============================================================

cd /d "%~dp0"
set "ROOT=%~dp0"
set "API_PROJECT=%ROOT%src\Gym.API"

:: -----------------------------------------------------------
::  STEP 1 - Check .NET SDK
:: -----------------------------------------------------------
cls
echo.
echo  ============================================================
echo        HACK GYM - System Launcher
echo  ============================================================
echo.
echo  [1/5] Checking prerequisites...

where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  ERROR: .NET SDK 10.0 is not installed.
    echo  Download from: https://dotnet.microsoft.com/download/dotnet/10.0
    echo.
    pause
    exit /b 1
)
for /f "tokens=*" %%V in ('dotnet --version 2^>nul') do set "DOTNET_VER=%%V"
echo   .NET SDK : %DOTNET_VER%

:: -----------------------------------------------------------
::  STEP 2 - Detect SQL Server
:: -----------------------------------------------------------
echo.
echo  [2/5] Detecting database...

:: Try LocalDB first
set "CONN_STR=Server=(localdb)\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
set "DB_TYPE=LocalDB"
set "SQL_FOUND="

:: Test LocalDB
where sqlcmd >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "SELECT 1" >nul 2>&1
    if %ERRORLEVEL% EQU 0 set "SQL_FOUND=1"
)

if not defined SQL_FOUND (
    :: Fallback to localhost (full SQL Server)
    set "CONN_STR=Server=localhost;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    set "DB_TYPE=SQL Server"
    sqlcmd -S "localhost" -E -C -Q "SELECT 1" >nul 2>&1
    if %ERRORLEVEL% EQU 0 set "SQL_FOUND=1"
)

if defined SQL_FOUND (
    echo   Database: %DB_TYPE% detected
) else (
    echo.
    echo  WARNING: No SQL Server instance found.
    echo  The app will attempt to create the database on first run,
    echo  but you need SQL Server (LocalDB or full) installed.
    echo  Install LocalDB: https://aka.ms/sqllocaldb
    echo.
    echo  Press any key to continue anyway...
    pause >nul
)

:: -----------------------------------------------------------
::  STEP 3 - Install dotnet-ef (for migrations)
:: -----------------------------------------------------------
echo.
echo  [3/5] Checking dotnet-ef tool...

dotnet tool list --global 2>nul | findstr /i "dotnet-ef" >nul
if %ERRORLEVEL% NEQ 0 (
    echo   Installing dotnet-ef globally...
    dotnet tool install --global dotnet-ef >nul 2>&1
    if %ERRORLEVEL% EQU 0 (
        echo   dotnet-ef: Installed
    ) else (
        echo   WARNING: dotnet-ef install failed (migrations at startup will still work)
    )
) else (
    echo   dotnet-ef: OK
)

:: -----------------------------------------------------------
::  STEP 4 - Restore and Build
:: -----------------------------------------------------------
echo.
echo  [4/5] Restoring packages and building...

dotnet restore "%API_PROJECT%" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo   WARNING: dotnet restore had issues, continuing...
)

dotnet build "%API_PROJECT%" --nologo -c Release
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo  ERROR: Build failed. Check errors above.
    pause
    exit /b 1
)
echo   Build: Complete

:: -----------------------------------------------------------
::  STEP 5 - Launch
:: -----------------------------------------------------------
echo.
echo  [5/5] Starting server...

:: Set connection string via environment variable (overrides appsettings.json)
set "ConnectionStrings__DefaultConnection=%CONN_STR%"

:: Write a fresh run-api.bat (portable, uses relative path)
set "RUN_API=%ROOT%run-api.bat"
(
    echo @echo off
    echo title Gym API Server
    echo color 0A
    echo cd /d "%~dp0src\Gym.API"
    echo echo.
    echo echo  ============================================================
    echo echo   Hack Gym - API Server
    echo echo   URL: http://localhost:5000
    echo echo   Login: admin / Admin@123
    echo echo   Press Ctrl+C to stop.
    echo echo  ============================================================
    echo echo.
    echo dotnet run --no-build -c Release --urls "http://0.0.0.0:5000"
    echo echo.
    echo echo  Server stopped.
    echo pause ^>nul
) > "%RUN_API%"

echo.
echo  ============================================================
echo   Server starting at http://localhost:5000
echo   Login: admin / Admin@123
echo   Close the server window or press Ctrl+C to stop.
echo  ============================================================
echo.
echo.

start "Gym API Server" cmd /c ""%RUN_API%""

:: Open browser after server has had time to start
ping -n 4 127.0.0.1 >nul
start "" http://localhost:5000

echo   Launcher finished. Close this window if desired.
echo.
pause
