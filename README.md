# Hack Gym Management System

> **Version:** 1.0.0 | **Platform:** Windows (offline) | **Runtime:** .NET 10 | **Database:** SQL Server Express

A full-featured, offline gym management system built with ASP.NET Core MVC. Runs entirely on a single Windows machine with no internet dependency. Manages members, memberships, attendance, payments, notifications, fingerprint devices, and system users.

---

## Table of Contents

1. [System Overview](#1-system-overview)
2. [Quick Start](#2-quick-start)
3. [Default Credentials](#3-default-credentials)
4. [Architecture Overview](#4-architecture-overview)
5. [Pages & Features (MVC)](#5-pages--features-mvc)
6. [REST API](#6-rest-api)
7. [Users & Roles](#7-users--roles)
8. [Navigation Structure](#8-navigation-structure)
9. [Database Schema](#9-database-schema)
10. [Project Structure](#10-project-structure)
11. [Middlewares & Pipeline](#11-middlewares--pipeline)
12. [Troubleshooting](#12-troubleshooting)

---

## 1. System Overview

**Hack Gym Management System** manages daily gym operations entirely offline on a single Windows PC.

### Core Capabilities

| Feature | Description |
|---------|-------------|
| **Member Management** | Full CRUD with soft delete, restore, search, and Excel import |
| **Subscription Packages** | Create pricing packages with name, duration, price, free period, freeze days |
| **Membership Plans** | Plans with duration, price, max visits, freeze days, descriptions |
| **Member Subscriptions** | Create, renew, freeze, unfreeze, and cancel memberships |
| **Attendance** | Check-in/check-out via fingerprint device or manual entry |
| **Payments** | Record payments with multiple methods (Cash, Visa, Instapay, Wallet) |
| **Offers & Discounts** | Percentage or fixed-value promotional offers linked to plans |
| **Fingerprint Devices** | Manage ZKTeco devices, test connections, sync attendance data |
| **Notifications** | Create, send, and track WhatsApp/SMS/Email notifications |
| **Dashboard & Analytics** | Statistics: active members, today's attendance, revenue, charts |
| **User Management** | Role-based system users (Owner, Receptionist) with permissions |
| **System Settings** | Configurable settings: gym name, working hours, backup path, etc. |
| **Audit Logging** | Track all user actions with before/after JSON snapshots |
| **Database Backup** | Manual backup and restore functionality |

---

## 2. Quick Start

### Prerequisites

| Software | Purpose |
|----------|---------|
| .NET 10 SDK | Backend runtime |
| SQL Server Express (or higher) | Database (named instance: `SQLEXPRESS`) |

### Running the Application

**Option A — Double-click** `run.bat` in the project root. It:
1. Kills any process on port 5000
2. Builds the project in Release mode
3. Starts the API at `http://localhost:5000`
4. Opens browser

**Option B — Manual:**
```
dotnet run -c Release --urls http://0.0.0.0:5000 --project src/Gym.API
```

### Database Setup

The database is created automatically on first run via `EnsureCreatedAsync()`. To reset the database:

```
sqlcmd -S localhost\SQLEXPRESS -i database\init.sql
```

Backup files are in `backups/` folder:
- `GymManagementDb_20260627_162002.bak`
- `GymManagementDb_20260630_013616.bak`

### Configuration

Edit `src/Gym.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Secret": "SuperSecretKeyForGymManagementSystem2026!@#$%^&*()",
    "Issuer": "GymManagementAPI",
    "Audience": "GymManagementApp",
    "AccessTokenExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}
```

> **Note:** The connection string uses `localhost\SQLEXPRESS` (SQL Server Express named instance).

---

## 3. Default Credentials

| Username | Password | Role | Access Level |
|----------|----------|------|-------------|
| `admin` | `Admin@123` | Owner | Full system access (all features) |

After logging in, additional users can be created from the **Users** page (Owner only).

**Base URL:** `http://localhost:5000`

---

## 4. Architecture Overview

### Solution Projects (7 .NET Projects)

```
Hack Gym System
├── Gym.Shared           Shared kernel: BaseEntity, Result<T>, Guard, Enums
├── Gym.Domain           Domain entities (17), repository interfaces
├── Gym.Application      CQRS commands/queries, DTOs, FluentValidation, AutoMapper
├── Gym.Infrastructure   EF Core DbContext, repositories, auth, services
├── Gym.API              ASP.NET Core MVC + REST API (the main app)
├── Gym.DeviceService    Windows Service for ZKTeco fingerprint devices
└── Gym.NotificationService  Windows Service for WhatsApp/SMS notifications
```

### Key Patterns

| Pattern | Implementation |
|---------|---------------|
| **Clean Architecture** | Strict dependency inversion — inner layers have no outer dependencies |
| **CQRS** | Commands and queries separated via MediatR |
| **Repository** | Generic `IRepository<T>` with Unit of Work |
| **JWT Auth** | Access tokens (60 min) via HttpOnly cookie |
| **AutoMapper** | Convention-based DTO mapping (`IMapFrom<T>`) |
| **FluentValidation** | Auto-executed via MediatR pipeline behavior |
| **Soft Delete** | Members use `IsDeleted` flag instead of hard delete |
| **Audit Logging** | Tracks who changed what with before/after JSON snapshots |

### Request Pipeline

```
Startup → Serilog Logging
→ Try DB MigrateAsync → Fallback EnsureCreatedAsync
→ DeveloperExceptionPage (dev only)
→ OpenAPI (dev only)
→ ExceptionMiddleware (global error handler)
→ CORS
→ Authentication (JWT Bearer via cookie)
→ Authorization
→ Serve Static Files (wwwroot)
→ MapControllers (REST + MVC)
→ Default Route: {controller=Account}/{action=Login}/{id?}
```

---

## 5. Pages & Features (MVC)

All pages are server-rendered Razor Views with a consistent layout (sidebar + topbar + receipt-card styling). The sidebar tracks which page is active using the controller name.

### 5.1 Authentication

#### Login Page (`/Account/Login`)

| Aspect | Details |
|--------|---------|
| Route | `GET /Account/Login`, `POST /Account/Login` |
| Access | Anonymous (no login required) |
| Controller | `AccountController` |

- Form with username + password fields
- On success: sets `accessToken` HttpOnly cookie (7-day expiry), redirects to Home
- On failure: shows error message on the same page
- Users already authenticated are redirected to Home automatically
- Logout: `POST /logout` — deletes cookie, redirects to Login

### 5.2 Dashboard

#### Home Page (`/Home`)

| Aspect | Details |
|--------|---------|
| Route | `GET /Home` |
| Access | Owner, Receptionist |
| Controller | `HomeMvcController` |

Summary dashboard showing:
- Quick stats cards (active members, today's attendance, etc.)
- Uses MediatR `GetDashboardQuery`

#### Analytics Dashboard (`/Dashboard`)

| Aspect | Details |
|--------|---------|
| Route | `GET /Dashboard` |
| Access | Owner, Receptionist |
| Controller | `DashboardMvcController` |

Detailed analytics with:
- Charts and statistics
- Member growth, attendance trends, revenue data
- Uses MediatR `GetDetailedDashboardQuery`

### 5.3 Members (`/Members`)

| Aspect | Details |
|--------|---------|
| Route | `/Members` |
| Access | Owner, Receptionist |
| Controller | `MembersMvcController` |

**Full CRUD** with the most features of any module:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Members` | Paginated table with search (name, phone, national ID) and sort by columns |
| **Create** | `GET/POST /Members/create` | New member form with all fields: personal info, subscription details, payment method |
| **Edit** | `GET/POST /Members/edit/{id}` | Update member details |
| **Details** | `GET /Members/details/{id}` | Full member profile with membership history + last 50 attendances |
| **Delete** | `GET/POST /Members/delete/{id}` | Soft-delete confirmation |
| **Search** | `GET /Members/search` | Advanced multi-filter search (name, national ID, phone, receipt#, package, subscription status, payment status, expiring soon, outstanding balance) |
| **Import** | `GET/POST /Members/import` | Excel (.xlsx) import for bulk member creation |

Member fields: ReceiptNumber, FullName, Nationality, NationalId, PhoneNumber, Email, DateOfBirth, Gender, Company, Address, Weight, HasDisease, DiseaseType, ReferralSource, Package, SubscriptionPrice, PaidAmount, RemainingAmount, DurationMonths, FreeMonths, FreezeDays, StartDate, EndDate, PaymentMethod, FingerprintDeviceId, MemberSignature, AdminSignature

### 5.4 Packages (`/Packages`)

| Aspect | Details |
|--------|---------|
| Route | `/Packages` |
| Access | Owner only |
| Controller | `PackagesMvcController` |

Manage subscription packages:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Packages` | Package list with search |
| **Create** | `GET/POST /Packages/create` | New package (name, duration months, price, free period, max freeze days) |
| **Edit** | `GET/POST /Packages/edit/{id}` | Update package |
| **Details** | `GET /Packages/details/{id}` | Package details |
| **Delete** | `GET/POST /Packages/delete/{id}` | Delete confirmation |

### 5.5 Plans (`/Plans`)

| Aspect | Details |
|--------|---------|
| Route | `/Plans` |
| Access | Owner only |
| Controller | `PlansMvcController` |

Manage membership plans (different from packages — these are plan types with visit limits):

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Plans` | Plan list with search |
| **Create** | `GET/POST /Plans/create` | New plan (name, price, duration days, max visits, freeze days, description) |
| **Edit** | `GET/POST /Plans/edit/{id}` | Update plan |
| **Details** | `GET /Plans/details/{id}` | Plan details |
| **Delete** | `GET/POST /Plans/delete/{id}` | Delete confirmation |

### 5.6 Memberships (`/Memberships`)

| Aspect | Details |
|--------|---------|
| Route | `/Memberships` |
| Access | Owner, Receptionist |
| Controller | `MembershipsMvcController` |

Read-only view of member subscriptions:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Memberships` | Paginated list of all memberships |
| **Details** | `GET /Memberships/details/{id}` | Single membership details |

### 5.7 Attendance (`/Attendance`)

| Aspect | Details |
|--------|---------|
| Route | `/Attendance` |
| Access | Owner, Receptionist |
| Controller | `AttendancesMvcController` |

Read-only view of attendance records:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Attendance` | Paginated attendance list |
| **Details** | `GET /Attendance/details/{id}` | Single attendance record details |

### 5.8 Payments (`/Payments`)

| Aspect | Details |
|--------|---------|
| Route | `/Payments` |
| Access | Owner, Receptionist |
| Controller | `PaymentsMvcController` |

Manage payment transactions:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Payments` | Paginated payment list |
| **Create** | `GET/POST /Payments/create` | New payment (select member from dropdown, amount, method, etc.) |
| **Details** | `GET /Payments/details/{id}` | Payment details |
| **Delete** | `GET/POST /Payments/delete/{id}` | Delete payment |

### 5.9 Offers (`/Offers`)

| Aspect | Details |
|--------|---------|
| Route | `/Offers` |
| Access | Owner only |
| Controller | `OffersMvcController` |

Manage promotional offers:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Offers` | Offer list with search |
| **Create** | `GET/POST /Offers/create` | New offer (title, linked plan, type, bonus months/days, price, date range) |
| **Edit** | `GET/POST /Offers/edit/{id}` | Update offer |
| **Details** | `GET /Offers/details/{id}` | Offer details |
| **Delete** | `GET/POST /Offers/delete/{id}` | Delete confirmation |

### 5.10 Devices (`/Devices`)

| Aspect | Details |
|--------|---------|
| Route | `/Devices` |
| Access | Owner only |
| Controller | `DevicesMvcController` |

Manage ZKTeco fingerprint devices:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Devices` | Device list with search |
| **Create** | `GET/POST /Devices/create` | New device (name, IP address, port, model, serial number) |
| **Edit** | `GET/POST /Devices/edit/{id}` | Update device config |
| **Details** | `GET /Devices/details/{id}` | Device details |
| **Delete** | `GET/POST /Devices/delete/{id}` | Delete confirmation |

### 5.11 Notifications (`/Notifications`)

| Aspect | Details |
|--------|---------|
| Route | `/Notifications` |
| Access | Owner, Receptionist |
| Controller | `NotificationsMvcController` |

Manage system notifications:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Notifications` | Paginated notification list |
| **Create** | `GET/POST /Notifications/create` | New notification |
| **Details** | `GET /Notifications/details/{id}` | Notification details |

### 5.12 Users (`/Users`)

| Aspect | Details |
|--------|---------|
| Route | `/Users` |
| Access | Owner only |
| Controller | `UsersMvcController` |

Manage system users (for staff login):

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Users` | User list with search |
| **Create** | `GET/POST /Users/create` | New user (select role from dropdown) |
| **Edit** | `GET/POST /Users/edit/{id}` | Update user (name, email, phone, role, active status) |
| **Details** | `GET /Users/details/{id}` | User details |
| **Delete** | `GET/POST /Users/delete/{id}` | Delete confirmation |

### 5.13 Settings (`/Settings`)

| Aspect | Details |
|--------|---------|
| Route | `/Settings` |
| Access | Owner only |
| Controller | `SettingsMvcController` |

Manage system configuration:

| Action | Route | Description |
|--------|-------|-------------|
| **List** | `GET /Settings` | All settings grouped by category |
| **Create** | `GET/POST /Settings/create` | New setting (key, value, group, description) |
| **Edit** | `GET/POST /Settings/edit/{id}` | Update setting value (key/group are read-only) |

Default settings:

| Key | Default Value | Group |
|-----|---------------|-------|
| GymName | My Gym | General |
| DeviceIP | 192.168.1.201 | Device |
| DevicePort | 4370 | Device |
| BackupPath | C:\Backups\GymManagement | Backup |
| WorkingHoursStart | 08:00 | General |
| WorkingHoursEnd | 22:00 | General |
| ReminderDays | 7,3,1 | Notifications |
| WhatsAppEnabled | false | Notifications |
| SMSEnabled | false | Notifications |
| DefaultCurrency | EGP | General |

---

## 6. REST API

The system also provides a REST API at `/api/*` for all entities, used by the MVC pages internally and available for integration.

### Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/login` | Login with username/password, returns JWT | No |
| POST | `/api/auth/refresh` | Refresh expired token | Yes |
| POST | `/api/auth/logout` | Invalidate refresh token | Yes |
| GET | `/api/auth/me` | Get current user profile + permissions | Yes |

### API Endpoints by Module

| Module | Method | Endpoints |
|--------|--------|-----------|
| **Members** | GET | `/api/members`, `/api/members/{id}`, `/api/members/search`, `/api/members/expiring`, `/api/members/outstanding-balance` |
| | POST/PUT/DELETE | `POST /api/members`, `PUT /api/members/{id}`, `DELETE /api/members/{id}`, `PATCH /api/members/{id}/restore` |
| **Plans** | GET | `/api/plans`, `/api/plans/{id}`, `/api/plans/active` |
| | POST/PUT | `POST /api/plans`, `PUT /api/plans/{id}`, `DELETE /api/plans/{id}`, `PATCH /api/plans/{id}/status` |
| **Memberships** | GET | `/api/memberships`, `/api/memberships/{id}`, `/api/memberships/by-member/{memberId}` |
| | POST | `POST /api/memberships`, `POST /api/memberships/{id}/renew`, `POST /api/memberships/{id}/freeze`, `POST /api/memberships/{id}/unfreeze`, `POST /api/memberships/{id}/cancel` |
| **Attendance** | GET | `/api/attendance`, `/api/attendance/{id}`, `/api/attendance/by-member/{memberId}`, `/api/attendance/today` |
| | POST | `POST /api/attendance/check-in`, `POST /api/attendance/check-out`, `POST /api/attendance/manual` |
| **Payments** | GET | `/api/payments`, `/api/payments/{id}`, `/api/payments/by-member/{memberId}` |
| | POST/DELETE | `POST /api/payments`, `DELETE /api/payments/{id}` |
| **Offers** | GET | `/api/offers`, `/api/offers/{id}`, `/api/offers/active` |
| | POST/PUT | `POST /api/offers`, `PUT /api/offers/{id}`, `DELETE /api/offers/{id}`, `PATCH /api/offers/{id}/status` |
| **Devices** | GET | `/api/devices`, `/api/devices/{id}`, `/api/devices/active` |
| | POST/PUT | `POST /api/devices`, `PUT /api/devices/{id}`, `DELETE /api/devices/{id}`, `PATCH /api/devices/{id}/status`, `POST /api/devices/{id}/sync`, `GET /api/devices/{id}/logs`, `POST /api/devices/{id}/test-connection` |
| **Notifications** | GET | `/api/notifications`, `/api/notifications/{id}`, `/api/notifications/pending`, `/api/notifications/by-member/{memberId}` |
| | POST | `POST /api/notifications`, `POST /api/notifications/{id}/mark-sent`, `POST /api/notifications/{id}/mark-failed` |
| **Settings** | GET | `/api/settings`, `/api/settings/{id}`, `/api/settings/by-key/{key}`, `/api/settings/by-group/{group}` |
| | POST/PUT/DELETE | `POST /api/settings`, `PUT /api/settings/{id}`, `DELETE /api/settings/{id}` |
| **Dashboard** | GET | `/api/dashboard` |
| **Users** | GET | `/api/users`, `/api/users/{id}`, `/api/users/roles` |
| | POST/PUT/DELETE | `POST /api/users`, `PUT /api/users/{id}`, `DELETE /api/users/{id}` |
| **Packages** | GET | `/api/packages`, `/api/packages/{id}`, `/api/packages/active`, `/api/packages/search` |
| | POST/PUT | `POST /api/packages`, `PUT /api/packages/{id}`, `DELETE /api/packages/{id}`, `PATCH /api/packages/{id}/activate`, `PATCH /api/packages/{id}/deactivate` |

### Response Format

```json
// Success
{ "success": true, "data": { ... }, "message": null }

// Error
{ "success": false, "data": null, "message": "Error description", "errors": ["..."] }

// Paginated
{
  "items": [...],
  "totalCount": 100,
  "page": 1,
  "pageSize": 20,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## 7. Users & Roles

### Role Definitions

| Role | Description | Access |
|------|-------------|--------|
| **Owner** | Full system access — can do everything | All pages + API full CRUD |
| **Receptionist** | Front desk operations | Members (CRUD), Memberships, Attendance, Payments, Notifications, Dashboard. Read-only on some features |

### Access by Role

| Module | Owner | Receptionist |
|--------|-------|-------------|
| Home Page | ✅ | ✅ |
| Dashboard | ✅ | ✅ |
| Members (list/create/edit/details) | ✅ | ✅ |
| Members (delete) | ✅ | ❌ |
| Members (import) | ✅ | ❌ |
| Packages | ✅ | ❌ |
| Plans | ✅ | ❌ |
| Memberships | ✅ | ✅ |
| Attendance | ✅ | ✅ |
| Payments | ✅ | ✅ |
| Offers | ✅ | ❌ |
| Devices | ✅ | ❌ |
| Notifications | ✅ | ✅ |
| Users | ✅ | ❌ |
| Settings | ✅ | ❌ |

### Permissions System

The system has 16 granular permissions:

| Permission | Owner | Receptionist |
|------------|-------|-------------|
| `members.read` | ✅ | ✅ |
| `members.write` | ✅ | ✅ |
| `members.delete` | ✅ | ❌ |
| `plans.read` | ✅ | ✅ |
| `plans.write` | ✅ | ❌ |
| `attendance.read` | ✅ | ✅ |
| `attendance.write` | ✅ | ✅ |
| `payments.read` | ✅ | ✅ |
| `payments.write` | ✅ | ✅ |
| `reports.read` | ✅ | ✅ |
| `settings.read` | ✅ | ❌ |
| `settings.write` | ✅ | ❌ |
| `devices.manage` | ✅ | ❌ |
| `backup.manage` | ✅ | ❌ |
| `offers.manage` | ✅ | ❌ |
| `users.manage` | ✅ | ❌ |

---

## 8. Navigation Structure

The sidebar (260px wide, fixed position) contains the following links:

```
┌─────────────────────┐
│  HG  Hack Gym       │  ← Logo + brand name
├─────────────────────┤
│  📊  Home           │  → /Home (Owner, Receptionist)
│  📈  Dashboard      │  → /Dashboard (Owner, Receptionist)
│  👥  Members        │  → /Members (Owner, Receptionist)
│  📦  Packages       │  → /Packages (Owner only)
│  📋  Plans          │  → /Plans (Owner only)
│  🎫  Memberships    │  → /Memberships (Owner, Receptionist)
│  ✅  Attendance     │  → /Attendance (Owner, Receptionist)
│  💰  Payments       │  → /Payments (Owner, Receptionist)
│  🏷️  Offers         │  → /Offers (Owner only)
│  🖥️  Devices        │  → /Devices (Owner only)
│  🔔  Notifications  │  → /Notifications (Owner, Receptionist)
│  🔒  Users          │  → /Users (Owner only — hidden from Receptionist)
│  ⚙️  Settings       │  → /Settings (Owner only — hidden from Receptionist)
├─────────────────────┤
│  Hack Gym © 2026    │  ← Footer
└─────────────────────┘
```

**Topbar:** Page title on left, Logout button (POST /logout) on right.

---

## 9. Database Schema

### Tables (18 total)

| # | Table | Description | Key Columns |
|---|-------|-------------|-------------|
| 1 | `Roles` | User roles | Id, Name (unique), Description, IsSystem |
| 2 | `Permissions` | Granular permissions | Id, Name (unique), Description |
| 3 | `RolePermissions` | Role↔Permission M:N | RoleId, PermissionId |
| 4 | `Users` | System users | Id, Username, PasswordHash, FullName, Email, Phone, RoleId, IsActive, RefreshToken, LastLoginAt |
| 5 | `Members` | Gym members | Id, Code, ReceiptNumber, FullName, PhoneNumber, NationalId, Nationality, Subscription*, PaymentMethod, PackageId, FingerscanId, IsDeleted |
| 6 | `MembershipPlans` | Membership plan types | Id, Name, Price, DurationDays, MaxVisits, FreezeDays, Description, IsActive |
| 7 | `Memberships` | Member subscriptions | Id, MemberId, PlanId, StartDate, EndDate, Status, Freeze* |
| 8 | `Attendance` | Check-in/out records | Id, MemberId, DeviceId, Date, Time, CheckIn, CheckOut, IsManual, SyncStatus |
| 9 | `Payments` | Payment transactions | Id, MemberId, EmployeeId, Amount, PaymentMethod, Date, ReceiptNumber |
| 10 | `Offers` | Promotional offers | Id, Title, DiscountType, DiscountValue, StartDate, EndDate, IsActive |
| 11 | `Notifications` | Notification queue | Id, MemberId, Type, Channel, Subject, Message, Status, ScheduledDate |
| 12 | `Devices` | Fingerprint devices | Id, Name, IPAddress, Port, Model, SerialNumber, IsActive, Status |
| 13 | `DeviceLogs` | Device communication logs | Id, DeviceId, Level, Message, Details |
| 14 | `Settings` | Key-value config | Id, Key (unique), Value, Group, Description, IsEncrypted |
| 15 | `AuditLogs` | User action audit trail | Id, UserId, Action, EntityType, EntityId, OldValues, NewValues, IpAddress |
| 16 | `BackupLogs` | Database backup history | Id, FileName, FilePath, Size, Status, ErrorMessage |
| 17 | `Packages` | Subscription packages | Id, PackageName, DurationMonths, Price, FreePeriodMonths, MaxFreezeDays, IsActive |
| 18 | `__EFMigrationsHistory` | EF Core migration tracking | MigrationId, ProductVersion |

### Key Relationships

```
Role 1──N User
Role N──N Permission (via RolePermissions)
Member N──1 MembershipPlan (Package)
Member 1──N Membership
Member 1──N Attendance
Member 1──N Payment
Member 1──N Notification
Membership N──1 MembershipPlan
Attendance N──1 Device
Device 1──N DeviceLog
User 1──N AuditLog
User 1──N Payment (as Employee)
```

---

## 10. Project Structure

```
D:\Hack gym system\
├── README.md                     ← This file
├── GymManagement.slnx            ← .NET solution file
├── run.bat                       ← One-click launcher (build + run)
├── start-system.bat              ← Full system setup script
├── start-api.bat                 ← API launcher with logging
├── database/
│   └── init.sql                  ← Full DB schema + seed data
├── backups/                      ← .bak database backup files
├── docs/                         ← Additional documentation
│
└── src/
    ├── Gym.Shared/               ← Shared kernel
    │   ├── Common/BaseEntity.cs   (Id, CreatedAt, UpdatedAt)
    │   ├── Common/Result.cs       (IsSuccess, Data, Message, Errors)
    │   ├── Common/Guard.cs        (validation helpers)
    │   └── Enums/Enums.cs         (Gender, PaymentMethod, etc.)
    │
    ├── Gym.Domain/               ← Domain layer (no dependencies)
    │   ├── Entities/              (17 entity classes)
    │   └── Interfaces/            (IRepository<T>, IMemberRepository, etc.)
    │
    ├── Gym.Application/          ← Application layer (CQRS)
    │   ├── Auth/
    │   ├── Members/
    │   ├── MembershipPlans/
    │   ├── Memberships/
    │   ├── Attendance/
    │   ├── Payments/
    │   ├── Offers/
    │   ├── Devices/
    │   ├── Notifications/
    │   ├── Settings/
    │   ├── Dashboard/
    │   ├── Users/
    │   ├── Packages/
    │   └── Common/                (interfaces, mappings, DTOs)
    │
    ├── Gym.Infrastructure/       ← Infrastructure layer
    │   ├── Data/
    │   │   ├── GymDbContext.cs     (17 DbSets)
    │   │   ├── Configurations/     (17 EF configs)
    │   │   ├── Migrations/         (EF Core migrations)
    │   │   └── Seed/
    │   ├── Repositories/          (Repository, UnitOfWork)
    │   ├── Security/              (TokenService, CurrentUserService)
    │   └── Services/              (AuthService, MemberService, etc.)
    │
    └── Gym.API/                  ← Presentation layer (runs on port 5000)
        ├── Program.cs             (startup pipeline)
        ├── appsettings.json       (configuration)
        ├── Controllers/           (14 REST + 14 MVC controllers)
        ├── Middleware/
        │   └── ExceptionMiddleware.cs  (global error handler)
        ├── Views/
        │   ├── Account/Login.cshtml
        │   ├── HomeMvc/Index.cshtml
        │   ├── DashboardMvc/Index.cshtml
        │   ├── MembersMvc/       (Index, Create, Edit, Details, Delete, Search, Import)
        │   ├── PackagesMvc/      (Index, Create, Edit, Details, Delete)
        │   ├── PlansMvc/         (Index, Create, Edit, Details, Delete)
        │   ├── MembershipsMvc/   (Index, Details)
        │   ├── AttendancesMvc/   (Index, Details)
        │   ├── PaymentsMvc/      (Index, Create, Details, Delete)
        │   ├── OffersMvc/        (Index, Create, Edit, Details, Delete)
        │   ├── DevicesMvc/       (Index, Create, Edit, Details, Delete)
        │   ├── NotificationsMvc/ (Index, Create, Details)
        │   ├── UsersMvc/         (Index, Create, Edit, Details, Delete)
        │   ├── SettingsMvc/      (Index, Create, Edit)
        │   └── Shared/
        │       ├── _Layout.cshtml     (main layout with sidebar + topbar)
        │       └── _Notifications.cshtml  (temp data alert partial)
        ├── ViewModels/
        └── wwwroot/               (static files)
```

---

## 11. Middlewares & Pipeline

### ExceptionMiddleware

Registered early in the pipeline at `app.UseMiddleware<ExceptionMiddleware>()`. Handles:

| Exception Type | HTTP Status | Behaviour |
|---------------|-------------|-----------|
| `ValidationException` (FluentValidation) | 400 Bad Request | JSON response for API; redirect for form POSTs |
| `UnauthorizedAccessException` | 401 Unauthorized | JSON/redirect |
| `KeyNotFoundException` | 404 Not Found | JSON/redirect |
| `ArgumentException` | 400 Bad Request | JSON/redirect |
| Any unhandled exception | 500/302 redirect | API: JSON error. MVC: redirects to referer page |

### JWT Authentication

- Bearer token authentication via `options.Events.OnMessageReceived`:

  1. First checks query string `access_token` parameter (for SignalR connections)
  2. Falls back to `accessToken` cookie
- Token validation: issuer, audience, signing key (`HmacSha256`), lifetime
- `ClockSkew = TimeSpan.Zero` (no tolerance)
- Access token: 60 minutes expiry

### Authorization

- `[Authorize]` attribute on all controllers (except `AccountController`)
- Role-based via `[Authorize(Roles = "Owner,Receptionist")]` or `[Authorize(Roles = "Owner")]`
- Default route: `{controller=Account}/{action=Login}/{id?}` — unauthenticated users land on the login page

### CORS

Policy `AllowFrontend` allows origins `http://localhost:5173` and `http://localhost:3000` with credentials (for the React dev servers).

---

## 12. Troubleshooting

### Port 5000 Already in Use
```batch
netstat -ano | findstr :5000
taskkill /F /PID <PID>
```

### SQL Server Connection Error
- Ensure SQL Server Express is running: `net start MSSQL$SQLEXPRESS`
- Check connection string in `appsettings.json` uses `localhost\SQLEXPRESS`

### Build Errors
```batch
dotnet clean src/Gym.API
dotnet build src/Gym.API -c Release
```

### Database Reset
```batch
sqlcmd -S localhost\SQLEXPRESS -d master -Q "DROP DATABASE GymManagementDb"
```
Then restart the API — it will recreate the DB automatically.

### Login Fails
- Default credentials: `admin` / `Admin@123`
- Check the `accessToken` cookie is set (browser dev tools → Application → Cookies)
- Verify the user exists in the `Users` table

### Page Returns 401 / Redirect to Login
- Cookie may have expired or been cleared
- Simply log in again

### Page Returns 500
- Check `logs/gym-api-YYYYMMDD.log` for the full stack trace
- Common causes: database schema mismatch (run `init.sql` to reset)

---

*Documentation generated from the Hack Gym Management System codebase (commit dfba239). For development guides, see `docs/` folder.*
