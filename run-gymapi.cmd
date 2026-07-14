@echo off
set JWT__Secret=DevJwtSecretKeyThatIsAtLeast32CharsLong!!
set Seed__AdminPassword=Admin@123
set ConnectionStrings__DefaultConnection=Data Source=D:\Hack gym system\GymDb.db; Cache=Shared;
dotnet run --project "D:\Hack gym system\src\Gym.API\Gym.API.csproj" -c Release --no-build > "D:\Hack gym system\gymapi.log" 2>&1
