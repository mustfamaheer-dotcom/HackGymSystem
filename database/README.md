# Database

The database schema is managed exclusively by **Entity Framework Core migrations** under
`src/Gym.Infrastructure/Migrations/`. Do not maintain a separate SQL schema file.

## What creates / updates the database

The API project runs pending migrations automatically on startup
(`src/Gym.API/Program.cs` calls `db.Database.MigrateAsync()` when the host boots).
That means a fresh `GymManagementDb` database is created and seeded on the very
first launch with no manual SQL step.

## Manual migration commands

From the repository root:

```powershell
# Apply pending migrations
dotnet ef database update `
  --project src\Gym.Infrastructure `
  --startup-project src\Gym.API

# Add a new migration after changing an entity or configuration
dotnet ef migrations add <MigrationName> `
  --project src\Gym.Infrastructure `
  --startup-project src\Gym.API

# Revert the last (unapplied) migration
dotnet ef migrations remove `
  --project src\Gym.Infrastructure `
  --startup-project src\Gym.API
```

## Adding the dotnet-ef tool (only required for manual commands)

```powershell
dotnet tool install --global dotnet-ef
```

The startup-time `MigrateAsync` call does not require the tool to be installed.

## Connection string

Set via `ConnectionStrings__DefaultConnection` (environment variable) or
`src/Gym.API/appsettings.json` (`ConnectionStrings:DefaultConnection`).
Defaults to `Server=localhost;Database=GymManagementDb;Trusted_Connection=True;`.
