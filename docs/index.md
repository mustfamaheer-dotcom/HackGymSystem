# Gym Management System — Documentation

A production-ready, fully offline Gym Management System with ZKTeco MB2000 fingerprint device integration.

## Contents

| Document | Description |
|---|---|
| [Architecture](architecture.md) | Clean Architecture layers, project structure, design decisions |
| [Setup Guide](setup.md) | Local development environment setup |
| [API Reference](api-reference.md) | Complete REST API endpoint documentation |
| [Database](database.md) | Entity relationship diagram, schema overview, seed data |
| [Deployment](deployment.md) | Production deployment guidance for all components |

## Quick Overview

```
┌──────────────────────────────────────────────────────────┐
│                    React SPA (gym-web)                    │
│              Vite + React 19 + Tailwind CSS               │
└──────────────────────┬───────────────────────────────────┘
                       │ HTTP (REST + SignalR)
┌──────────────────────┴───────────────────────────────────┐
│               API Gateway (Kestrel)                       │
│            ASP.NET Core Minimal / Controllers              │
├──────────────────────────────────────────────────────────┤
│                Application Layer (CQRS)                    │
│         MediatR + FluentValidation + AutoMapper            │
├──────────────────────────────────────────────────────────┤
│               Domain Layer (DDD)                           │
│   15 Entities + Value Objects + Domain Interfaces         │
├──────────────────────────────────────────────────────────┤
│            Infrastructure Layer                            │
│     EF Core → SQL Server / Repository / UoW / Auth        │
└──────────────────────────────────────────────────────────┘
         │                          │
         ▼                          ▼
   SQL Server (local)     ZKTeco MB2000 (COM Interop)
```

### Projects

| Project | Path | Purpose |
|---|---|---|
| `Gym.Domain` | `src/Gym.Domain/` | Entities, enums, domain interfaces |
| `Gym.Application` | `src/Gym.Application/` | CQRS commands/queries, validators, DTOs |
| `Gym.Infrastructure` | `src/Gym.Infrastructure/` | EF Core, Repository, Auth, Token service |
| `Gym.API` | `src/Gym.API/` | REST controllers, middleware, Program.cs |
| `Gym.DeviceService` | `src/Gym.DeviceService/` | Windows Service for ZKTeco device |
| `Gym.NotificationService` | `src/Gym.NotificationService/` | Windows Service for notifications |
| `Gym.Shared` | `src/Gym.Shared/` | Shared kernel (BaseEntity, Result<T>, Guard) |
| `Gym.Web` | `gym-web/` | React SPA frontend |

### Tech Stack

| Component | Technology |
|---|---|
| **Runtime** | .NET 10 / ASP.NET Core |
| **Database** | SQL Server (LocalDB or Express) |
| **ORM** | EF Core 10 |
| **CQRS** | MediatR + FluentValidation |
| **Mapping** | AutoMapper 12 |
| **Auth** | JWT Bearer tokens |
| **Real-time** | SignalR |
| **Background Jobs** | Hangfire |
| **Logging** | Serilog |
| **Frontend** | Vite 8 + React 19 + TypeScript 6 |
| **Styling** | Tailwind CSS v4 |
| **HTTP Client** | Axios + TanStack React Query |
| **Fingerprint** | ZKTeco MB2000 (COM Interop, zkemkeeper.dll) |
| **Notifications** | WhatsApp Business API + Twilio SMS |
