@echo off
title Gym API Server
color 0A

:: Portable - uses the script's own location
cd /d "%~dp0src\Gym.API"

:: Allow override via environment variable (set by run.bat or manually)
if not defined ConnectionStrings__DefaultConnection (
    set "ConnectionStrings__DefaultConnection=Server=(localdb)\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
)

dotnet run --no-build -c Release --urls "http://localhost:5000"
