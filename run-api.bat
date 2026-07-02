@echo off
title Gym API Server
color 0A
cd /d "%~dp0src\Gym.API"
if not defined ConnectionStrings__DefaultConnection (
    set "ConnectionStrings__DefaultConnection=Server=(localdb)\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
)
echo.
echo  ============================================================
echo   Hack Gym - API Server
echo   URL: http://localhost:5000
echo   Login: admin / Admin@123
echo   Press Ctrl+C to stop.
echo  ============================================================
echo.
dotnet run --no-build -c Release --urls "http://0.0.0.0:5000"
echo.
echo  Server stopped.
pause >nul
