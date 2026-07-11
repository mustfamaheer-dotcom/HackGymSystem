@echo off
title ZKTeco Bridge Service
color 0B
cd /d "%~dp0src\HackGym.ZKTeco.Bridge"
echo.
echo  ============================================================
echo   Hack Gym - ZKTeco Bridge Service
echo   Polling device every 3 seconds...
echo   Press Ctrl+C to stop.
echo  ============================================================
echo.
dotnet run --no-build -c Release
echo.
echo  Bridge stopped.
pause >nul
