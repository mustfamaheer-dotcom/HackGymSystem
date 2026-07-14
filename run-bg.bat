@echo off
title Gym API Server (Direct)
color 0A
cd /d "%~dp0src\Gym.API"
echo.
echo  ============================================================
echo   Gym API Server (Direct) - http://localhost:5000
echo   Default login: admin / Admin@123
echo   Press Ctrl+C to stop.
echo  ============================================================
echo.
dotnet run -c Release --urls "http://0.0.0.0:5000"
echo.
echo  Server stopped.
pause >nul
