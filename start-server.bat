@echo off
cd /d "%~dp0src\Gym.API"
dotnet run -c Release --urls "http://0.0.0.0:5000"
