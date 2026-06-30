@echo off
title Gym API Server
color 0A
cd /d "E:\WORK\FreeLance\ORBiT\SYSTEMS\GYMS\C. Amir - Hack Gym\Phase 1\System\src\Gym.API"
echo.
echo  ============================================================
echo   Gym API Server - http://localhost:5000
echo   Default login: admin / Admin@123
echo   Press Ctrl+C to stop.
echo  ============================================================
echo.
dotnet run --no-build -c Release --urls "http://0.0.0.0:5000"
echo.
echo  Server stopped. Press any key to close.
pause >nul
