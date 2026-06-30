# Architecture

## Clean Architecture Layering

The system follows Clean Architecture with strict dependency inversion — inner layers know nothing about outer layers.

```
┌─────────────────────────────────────────────────────────────┐
│                     Gym.API / Gym.Web                        │
│           (Presentation — Controllers / React SPA)           │
│                     Depends on: Application                   │
├─────────────────────────────────────────────────────────────┤
│                     Gym.Application                          │
│          (Use Cases — CQRS Commands/Queries/Validators)       │
│                     Depends on: Domain                        │
├─────────────────────────────────────────────────────────────┤
│                       Gym.Domain                             │
│         (Enterprise Business Rules — Entities/Interfaces)     │
│                     Depends on: (nothing)                     │
├─────────────────────────────────────────────────────────────┤
│                     Gym.Infrastructure                       │
│   (Persistence / External Services — EF Core, Auth, SMTP)    │
│                     Depends on: Application, Domain           │
├─────────────────────────────────────────────────────────────┤
│               Gym.DeviceService / Gym.NotificationService     │
│           (Background Windows Services — long-running)        │
│                     Depends on: Infrastructure, Application   │
└─────────────────────────────────────────────────────────────┘
```

## Project Dependencies

```
Gym.Shared          (no dependencies)
    ↑
Gym.Domain          (no dependencies)
    ↑
Gym.Application     → Gym.Domain, Gym.Shared
    ↑
Gym.Infrastructure  → Gym.Application, Gym.Domain, Gym.Shared
    ↑
Gym.API             → Gym.Infrastructure, Gym.Application, Gym.Shared
Gym.DeviceService   → Gym.Infrastructure, Gym.Application
Gym.NotificationService → Gym.Infrastructure, Gym.Application
Gym.Web             (independent React SPA, consumes Gym.API via HTTP)
```

## Domain Layer (`Gym.Domain`)

### Entities (15 total)

| Entity | Description |
|---|---|
| `User` | System users (admins, staff) with hashed passwords |
| `Role` | RBAC roles |
| `Permission` | Granular permissions |
| `RolePermission` | Role-permission assignment (many-to-many) |
| `Member` | Gym members (customers) |
| `MembershipPlan` | Subscription plans (monthly, yearly, etc.) |
| `Membership` | Active subscriptions linking members to plans |
| `Attendance` | Check-in / check-out records |
| `Payment` | Payment transactions |
| `Offer` | Discounts and promotions |
| `Notification` | Outbound notification log |
| `Device` | ZKTeco fingerprint devices |
| `DeviceLog` | Device operation logs |
| `Setting` | System-wide key-value settings |
| `AuditLog` | Change tracking log |
| `BackupLog` | Database backup records |

### Key Interfaces

```csharp
IRepository<T>     // Generic repository (GetAll, GetById, Add, Update, Delete)
IUnitOfWork        // Transaction management (SaveChanges, Begin/Commit/Rollback)
```

## Application Layer (`Gym.Application`)

### CQRS Pattern

Every use case is split into a Command (write) or Query (read), each with:
- **Request** (`IRequest<T>`) — input DTO
- **Handler** (`IRequestHandler<TRequest, TResponse>`) — business logic
- **Validator** (`AbstractValidator<T>`) — FluentValidation rules
- **DTO** — output model with `IMapFrom<T>` for AutoMapper

### Modules (12 modules)

```
Auth           — Login, Refresh, Logout, GetCurrentUser
Members        — CRUD + Search
MembershipPlans— CRUD + ToggleActive
Memberships    — Create, Renew, Freeze, Unfreeze, Cancel
Attendance     — CheckIn, CheckOut, Manual, GetByMember
Payments       — CRUD + GetByMember
Offers         — CRUD
Devices        — CRUD + Sync + Logs
Notifications  — Send, Resend
Settings       — GetAll (by group), Update
Dashboard      — Aggregate stats (members, revenue, attendance, activities)
```

### Pipeline Behaviour

`ValidationBehavior` wraps MediatR pipeline to automatically run FluentValidation validators before each request handler.

## Infrastructure Layer (`Gym.Infrastructure`)

### EF Core (`GymDbContext`)

- 15 `DbSet<T>` properties
- 15 entity type configurations (fluent API)
- Seed data for roles, admin user, permissions, role-permissions, settings
- Works with SQL Server (LocalDB or Express)

### Repository Pattern

`Repository<T>` implements `IRepository<T>`. `UnitOfWork` manages a dictionary of cached repositories and provides transaction scope.

### Authentication

- `AuthService` — validates credentials, generates JWT
- `TokenService` — JWT creation with claims
- `CurrentUserService` — extracts current user from `HttpContext`

### DI Registration (`ServiceRegistration` / `DependencyInjection`)

```
Scoped:   DbContext → GymDbContext
Scoped:   IRepository<T> → Repository<T>
Scoped:   IUnitOfWork → UnitOfWork
Scoped:   IAuthService → AuthService
Scoped:   ITokenService → TokenService
Scoped:   ICurrentUserService → CurrentUserService
```

## API Layer (`Gym.API`)

### Controllers

| Controller | Route | Auth |
|---|---|---|
| `AuthController` | `/api/auth` | ❌ (Login/Refresh), ✅ (Me/Logout) |
| `MembersController` | `/api/members` | ✅ |
| `PlansController` | `/api/plans` | ✅ |
| `MembershipsController` | `/api/memberships` | ✅ |
| `AttendancesController` | `/api/attendance` | ✅ |
| `PaymentsController` | `/api/payments` | ✅ |
| `OffersController` | `/api/offers` | ✅ |
| `DevicesController` | `/api/devices` | ✅ |
| `NotificationsController` | `/api/notifications` | ✅ |
| `SettingsController` | `/api/settings` | ✅ |
| `DashboardController` | `/api/dashboard` | ✅ |

### Middleware Pipeline

```
ExceptionMiddleware → Serilog → CORS → JWT Auth → Authorization → Controllers
```

### OpenAPI

Native .NET `AddOpenApi()` / `MapOpenApi()` at `GET /openapi/v1.json`.

## Frontend (`Gym.Web`)

### Component Tree

```
<BrowserRouter>
  <AuthProvider>
    <QueryClientProvider>
      <Routes>
        /login       → Login
        <ProtectedRoute>
          <AppLayout>
            <Outlet/>
              /dashboard      → Dashboard
              /members        → MemberList
              /plans          → PlanList
              /memberships    → MembershipList
              /attendance     → AttendanceList
              /payments       → PaymentList
              /offers         → OfferList
              /devices        → DeviceList
              /notifications  → NotificationList
              /settings       → SettingsPage
```

### Data Flow

```
Page Component
  → useQuery / useMutation (TanStack React Query)
    → API module function (api/members.ts)
      → axios instance (with JWT interceptor)
        → HTTP → Gym.API Controller
```

## Windows Services

### DeviceService

- Runs as Windows Service (`GymDeviceService`)
- Connects to ZKTeco MB2000 via COM Interop (`zkemkeeper.dll`)
- Polls for fingerprint events and pushes check-in records to the API
- Runs Hangfire server for scheduled sync

### NotificationService

- Runs as Windows Service (`GymNotificationService`)
- Processes pending notifications from the queue
- Sends via WhatsApp Business API or Twilio SMS
- Retries failed deliveries
