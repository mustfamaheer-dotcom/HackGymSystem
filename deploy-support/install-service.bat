@echo off
title Install Gym Management Service
setlocal enabledelayedexpansion

:: Must run as admin
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo ================================================
    echo  ERROR: You must run this as Administrator.
    echo ================================================
    echo Right-click install-service.bat and select
    echo "Run as administrator".
    pause
    exit /b 1
)

pushd "%~dp0"

set SERVICE_NAME=GymManagementApi
set SERVICE_DISPLAY=Gym Management API
set EXE_PATH=%~dp0Gym.API.exe

:: Check if service already exists
sc query %SERVICE_NAME% >nul 2>&1
if %errorlevel% equ 0 (
    echo [INFO] Service "%SERVICE_NAME%" already exists.
    echo        Stopping and removing old instance...
    net stop %SERVICE_NAME% >nul 2>&1
    sc delete %SERVICE_NAME% >nul 2>&1
    timeout /t 2 /nobreak >nul
)

:: Create the service
sc create %SERVICE_NAME%^
    binPath= "%EXE_PATH% --urls http://0.0.0.0:5000"^
    DisplayName= "%SERVICE_DISPLAY%"^
    start= auto^
    obj= "NT AUTHORITY\NetworkService"

if %errorlevel% neq 0 (
    echo [ERROR] Failed to create service. Exit code: %errorlevel%
    pause
    exit /b 1
)

:: Set description
sc description %SERVICE_NAME% "Gym Management System -- attendance, subscriptions, members, and reports." >nul

:: Start the service
net start %SERVICE_NAME%
if %errorlevel% equ 0 (
    echo.
    echo ================================================
    echo  Service installed and started successfully!
    echo  http://localhost:5000
    echo ================================================
) else (
    echo [WARN] Service created but failed to start. Check Event Viewer.
)

pause
