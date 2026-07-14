@echo off
title Uninstall Gym Management Service
setlocal enabledelayedexpansion

:: Must run as admin
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo ================================================
    echo  ERROR: You must run this as Administrator.
    echo ================================================
    echo Right-click uninstall-service.bat and select
    echo "Run as administrator".
    pause
    exit /b 1
)

set SERVICE_NAME=GymManagementApi

:: Check if service exists
sc query %SERVICE_NAME% >nul 2>&1
if %errorlevel% neq 0 (
    echo [INFO] Service "%SERVICE_NAME%" is not installed.
    pause
    exit /b 0
)

:: Stop and delete
net stop %SERVICE_NAME% >nul 2>&1
sc delete %SERVICE_NAME% >nul 2>&1

if %errorlevel% equ 0 (
    echo [OK] Service "%SERVICE_NAME%" has been removed.
) else (
    echo [WARN] Service removal may have failed. Try running as Administrator.
)

pause
