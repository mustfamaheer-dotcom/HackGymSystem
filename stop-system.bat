@echo off
title Hack Gym System Stopper
color 0C

echo.
echo  ============================================================
echo   Stopping Hack Gym System...
echo  ============================================================
echo.

:: Stop API server
echo  Stopping Gym API Server...
taskkill /fi "WINDOWTITLE eq Gym API Server*" /f >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo   - Gym API Server stopped
) else (
    echo   - Gym API Server was not running
)

:: Stop Bridge service
echo  Stopping ZKTeco Bridge Service...
taskkill /fi "WINDOWTITLE eq ZKTeco Bridge Service*" /f >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo   - ZKTeco Bridge Service stopped
) else (
    echo   - ZKTeco Bridge Service was not running
)

:: Stop any remaining dotnet processes (optional, uncomment if needed)
:: echo  Stopping any remaining dotnet processes...
:: taskkill /f /im dotnet.exe >nul 2>&1

echo.
echo  ============================================================
echo   All services stopped.
echo  ============================================================
echo.
pause
