FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Gym.Shared/Gym.Shared.csproj src/Gym.Shared/
COPY src/Gym.Domain/Gym.Domain.csproj src/Gym.Domain/
COPY src/Gym.Application/Gym.Application.csproj src/Gym.Application/
COPY src/Gym.Infrastructure/Gym.Infrastructure.csproj src/Gym.Infrastructure/
COPY src/Gym.API/Gym.API.csproj src/Gym.API/
RUN dotnet restore src/Gym.API/Gym.API.csproj

COPY src/ .
RUN dotnet publish src/Gym.API/Gym.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5000
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Gym.API.dll"]
