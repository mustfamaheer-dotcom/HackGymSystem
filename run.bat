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
::  STEP 2 - Database (SQLite, file-based, no server needed)
:: -----------------------------------------------------------
echo.
echo  [2/5] Configuring database...

set "CONN_STR=Data Source=GymDb.db; Cache=Shared;"
set "DB_TYPE=SQLite (GymDb.db)"
echo   Database: %DB_TYPE%

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
    echo  ERROR: API build failed. Check errors above.
    pause
    exit /b 1
)

:: Also build the ZKTeco Bridge
echo.
echo  [4b] Building ZKTeco Bridge...
dotnet build "%ROOT%src\HackGym.ZKTeco.Bridge" --nologo -c Release
if %ERRORLEVEL% NEQ 0 (
    echo   WARNING: Bridge build failed, ZKTeco features will be unavailable
)
echo   Build: Complete

:: -----------------------------------------------------------
::  STEP 5 - Launch
:: -----------------------------------------------------------
echo.
echo  [5/5] Starting servers...

:: Set connection string via environment variable (overrides appsettings.json)
set "ConnectionStrings__DefaultConnection=%CONN_STR%"
set "JWT__Secret=DevJwtSecretKeyThatIsAtLeast32CharsLong!!"
set "Seed__AdminPassword=Admin@123"

:: Write a fresh run-bridge.bat (portable, uses relative path)
set "RUN_BRIDGE=%ROOT%run-bridge.bat"
(
    echo @echo off
    echo title ZKTeco Bridge Server
    echo color 0B
    echo cd /d "%~dp0src\HackGym.ZKTeco.Bridge"
    echo echo.
    echo echo  ============================================================
    echo echo   Hack Gym - ZKTeco Bridge Server
    echo echo   Port: http://localhost:50051
    echo echo   Press Ctrl+C to stop.
    echo echo  ============================================================
    echo echo.
    echo dotnet run --no-build -c Release
    echo echo.
    echo echo  Server stopped.
    echo pause ^>nul
) > "%RUN_BRIDGE%"

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
echo   Starting ZKTeco Bridge (port 50051) and API (port 5000)
echo   Login: admin / Admin@123
echo   Close the windows or press Ctrl+C to stop.
echo  ============================================================
echo.

start "ZKTeco Bridge Server" cmd /c ""%RUN_BRIDGE%""
ping -n 3 127.0.0.1 >nul

start "Gym API Server" cmd /c ""%RUN_API%""

:: Open browser after server has had time to start
ping -n 4 127.0.0.1 >nul
start "" http://localhost:5000

echo   Launcher finished. Close this window if desired.
echo.
pause
