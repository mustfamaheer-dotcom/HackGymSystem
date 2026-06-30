@echo off
cd /d "D:\ORBIT Work\Phase 1\System\System"
dotnet run --no-build --project "src\Gym.API" --urls "http://localhost:5000" > api-output.txt 2>&1
