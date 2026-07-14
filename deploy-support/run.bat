@echo off
title Gym Management System
setlocal enabledelayedexpansion

pushd "%~dp0"

:: ---------------------------------------------------------
:: 1. Check admin rights (needed for LocalDB and zkemkeeper)
:: ---------------------------------------------------------
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo ================================================
    echo  NOT RUNNING AS ADMINISTRATOR
    echo ================================================
    echo.
    echo Some features require admin rights:
    echo   - Starting LocalDB service
    echo   - Registering zkemkeeper COM component
    echo.
    echo If the browser does not open, or if LocalDB fails,
    echo right-click run.bat and select "Run as administrator".
    echo.
    timeout /t 5 /nobreak >nul
)

:: ---------------------------------------------------------
:: 2. Start LocalDB if not running
:: ---------------------------------------------------------
echo [INFO] Checking LocalDB...
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT 1" >nul 2>&1
if %errorlevel% neq 0 (
    echo [INFO] LocalDB is not running. Attempting to start...
    sqllocaldb start MSSQLLocalDB >nul 2>&1
    if !errorlevel! neq 0 (
        echo [WARN] Could not start LocalDB. Attempting to create instance...
        sqllocaldb create MSSQLLocalDB >nul 2>&1
        sqllocaldb start MSSQLLocalDB >nul 2>&1
    )
    timeout /t 3 /nobreak >nul
)
echo [OK] LocalDB is ready.

:: ---------------------------------------------------------
:: 3. Find a free port (start at 5000, increment if taken)
:: ---------------------------------------------------------
set PORT=5000
:checkport
netstat -an | findstr ":%PORT% " >nul 2>&1
if !errorlevel! equ 0 (
    echo [WARN] Port !PORT! is in use, trying next...
    set /a PORT=!PORT!+1
    if !PORT! gtr 5010 (
        echo [ERROR] Could not find a free port between 5000-5010.
        pause
        exit /b 1
    )
    goto checkport
)
echo [OK] Using port !PORT!

:: ---------------------------------------------------------
:: 4. Register zkemkeeper.dll if present
:: ---------------------------------------------------------
if exist "%~dp0zkemkeeper\zkemkeeper.dll" (
    echo [INFO] Registering zkemkeeper COM component...
    regsvr32 /s "%~dp0zkemkeeper\zkemkeeper.dll"
    if !errorlevel! equ 0 (
        echo [OK] zkemkeeper registered.
    ) else (
        echo [WARN] zkemkeeper registration failed (run as admin if needed).
    )
)

:: ---------------------------------------------------------
:: 5. Launch the application
:: ---------------------------------------------------------
echo.
echo ================================================
echo   Starting Gym Management System
echo   Port: !PORT!
echo   URL:  http://localhost:!PORT!
echo ================================================
echo.
start "" "http://localhost:!PORT!"
"%~dp0Gym.API.exe" --urls "http://0.0.0.0:!PORT!"

:: ---------------------------------------------------------
:: 6. Cleanup on exit
:: ---------------------------------------------------------
echo.
echo Application has exited.
pause
