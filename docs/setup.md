# Setup Guide

## Prerequisites

| Tool | Version | Purpose |
|---|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ | Backend runtime |
| [Node.js](https://nodejs.org/) | 22.x+ | Frontend build |
| [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | LocalDB, Express, or Developer | Database |
| [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code | — | IDE (optional) |

## 1. Clone & Restore

```bash
git clone <repo-url>
cd GymManagement

# Restore .NET packages
dotnet restore

# Restore npm packages
cd gym-web
npm install
cd ..
```

## 2. Database Setup

### Option A — SQL Script (recommended for first run)

Open `database/init.sql` in SQL Server Management Studio (SSMS) or run:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -i database\init.sql
```

This creates the `GymManagementDb` database with all tables, indexes, foreign keys, and seed data.

### Option B — EF Core Migrations

Update the connection string in `src/Gym.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Then apply migrations:

```bash
dotnet ef database update --project src/Gym.Infrastructure --startup-project src/Gym.API
```

## 3. Configuration

Edit `src/Gym.API/appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "<your-256-bit-secret-key>",
    "Issuer": "GymManagement",
    "Audience": "GymManagement",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },
  "Twilio": {
    "AccountSid": "",
    "AuthToken": "",
    "FromNumber": ""
  },
  "WhatsApp": {
    "ApiToken": "",
    "PhoneNumberId": ""
  }
}
```

### Default Admin Credentials

| Username | Password |
|---|---|
| `admin` | `Admin@123` |

## 4. Run Backend

```bash
dotnet run --project src/Gym.API
```

API is available at `http://localhost:5000`.  
OpenAPI spec at `http://localhost:5000/openapi/v1.json`.

## 5. Run Frontend

```bash
cd gym-web
npm run dev
```

Frontend is available at `http://localhost:5173`.  
It proxies `/api/*` to `http://localhost:5000`.

## 6. Run Tests

```bash
dotnet test
```

## 7. EF Core Migrations (for developers)

```bash
# Create a new migration
dotnet ef migrations add <MigrationName> --project src/Gym.Infrastructure --startup-project src/Gym.API

# Apply pending migrations
dotnet ef database update --project src/Gym.Infrastructure --startup-project src/Gym.API

# Remove last migration (not yet applied)
dotnet ef migrations remove --project src/Gym.Infrastructure --startup-project src/Gym.API
```

## 8. Build Frontend for Production

```bash
cd gym-web
npm run build
# Output in gym-web/dist/
```
