@echo off
setlocal
set JWT__Secret=DevJwtSecretKeyThatIsAtLeast32CharsLong!!
set Seed__AdminPassword=Admin@123
set ConnectionStrings__DefaultConnection=Data Source=D:\Hack gym system\GymDb.db; Cache=Shared;

echo Starting Gym.API (port 5000)...
start "GymAPI" cmd /c "dotnet run --project \"D:\Hack gym system\src\Gym.API\Gym.API.csproj\" -c Release --no-build > \"D:\Hack gym system\gymapi.log\" 2>&1"

ping -n 7 127.0.0.1 > nul

echo Starting ZKTeco Bridge (port 50051)...
start "ZKTBridge" cmd /c "dotnet run --project \"D:\Hack gym system\src\HackGym.ZKTeco.Bridge\HackGym.ZKTeco.Bridge.csproj\" -c Release --no-build > \"D:\Hack gym system\bridge.log\" 2>&1"

echo Both started (logs: gymapi.log, bridge.log)
endlocal
