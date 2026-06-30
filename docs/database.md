# Database Schema

## Entity Relationship Overview

```
User ──1:N──> AuditLog
Role ──1:N──> User
Role ──1:N──> RolePermission ──N:1──> Permission

Member ──1:N──> Membership ──N:1──> MembershipPlan
Member ──1:N──> Attendance
Member ──1:N──> Payment ──N:1──> Offer
Member ──1:N──> Notification

Device ──1:N──> DeviceLog

Setting (standalone)
BackupLog (standalone)
```

## Tables

### Users
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Username | nvarchar(50) | NOT NULL, unique |
| PasswordHash | nvarchar(500) | NOT NULL |
| Email | nvarchar(100) | NULL |
| FullName | nvarchar(100) | NOT NULL |
| RoleId | int | FK → Roles, NOT NULL |
| IsActive | bit | NOT NULL, default 1 |
| RefreshToken | nvarchar(500) | NULL |
| RefreshTokenExpiry | datetime2 | NULL |
| LastLoginAt | datetime2 | NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### Roles
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Name | nvarchar(50) | NOT NULL, unique |
| Description | nvarchar(200) | NULL |

### Permissions
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Name | nvarchar(100) | NOT NULL, unique |
| Description | nvarchar(200) | NULL |
| Group | nvarchar(50) | NULL |

### RolePermissions
| Column | Type | Constraints |
|---|---|---|
| RoleId | int | PK, FK → Roles |
| PermissionId | int | PK, FK → Permissions |

### Members
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| FullName | nvarchar(100) | NOT NULL |
| Phone | nvarchar(20) | NOT NULL |
| Email | nvarchar(100) | NULL |
| NationalId | nvarchar(20) | NULL, unique |
| Address | nvarchar(500) | NULL |
| DateOfBirth | date | NULL |
| Gender | nvarchar(10) | NULL |
| PhotoUrl | nvarchar(500) | NULL |
| FingerprintId | nvarchar(50) | NULL, unique |
| MembershipNumber | nvarchar(20) | NOT NULL, unique |
| IsActive | bit | NOT NULL, default 1 |
| Notes | nvarchar(max) | NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | FK → Users, NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### MembershipPlans
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Name | nvarchar(100) | NOT NULL |
| Description | nvarchar(500) | NULL |
| DurationDays | int | NOT NULL |
| Price | decimal(18,2) | NOT NULL |
| MaxVisits | int | NULL |
| FreezeDays | int | NULL, default 0 |
| IsActive | bit | NOT NULL, default 1 |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### Memberships
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| MemberId | int | FK → Members, NOT NULL |
| PlanId | int | FK → MembershipPlans, NOT NULL |
| StartDate | date | NOT NULL |
| EndDate | date | NOT NULL |
| Status | nvarchar(20) | NOT NULL (Active/Frozen/Expired/Cancelled) |
| RemainingVisits | int | NULL |
| FrozenDays | int | NULL, default 0 |
| Notes | nvarchar(max) | NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### Attendance
| Column | Type | Constraints |
|---|---|---|
| Id | bigint | PK, identity |
| MemberId | int | FK → Members, NOT NULL |
| Time | datetime2 | NOT NULL |
| CheckIn | datetime2 | NOT NULL |
| CheckOut | datetime2 | NULL |
| Method | nvarchar(20) | NOT NULL (Fingerprint/Manual/Card) |
| DeviceId | int | FK → Devices, NULL |
| DeviceName | nvarchar(100) | NULL |
| Notes | nvarchar(500) | NULL |

### Payments
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| MemberId | int | FK → Members, NOT NULL |
| Amount | decimal(18,2) | NOT NULL |
| AmountPaid | decimal(18,2) | NOT NULL |
| Remaining | decimal(18,2) | NULL |
| PaymentDate | datetime2 | NOT NULL |
| Method | nvarchar(50) | NOT NULL |
| Status | nvarchar(20) | NOT NULL (Paid/Pending/Overdue/Refunded) |
| Reference | nvarchar(100) | NULL |
| Notes | nvarchar(max) | NULL |
| MembershipId | int | FK → Memberships, NULL |
| OfferId | int | FK → Offers, NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |

### Offers
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Code | nvarchar(50) | NOT NULL, unique |
| Name | nvarchar(100) | NOT NULL |
| Description | nvarchar(500) | NULL |
| DiscountType | nvarchar(20) | NOT NULL (Percentage/Fixed) |
| DiscountValue | decimal(18,2) | NOT NULL |
| StartDate | datetime2 | NOT NULL |
| EndDate | datetime2 | NULL |
| IsActive | bit | NOT NULL, default 1 |
| UsageCount | int | NOT NULL, default 0 |
| MaxUsage | int | NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### Notifications
| Column | Type | Constraints |
|---|---|---|
| Id | bigint | PK, identity |
| MemberId | int | FK → Members, NULL |
| Type | nvarchar(50) | NOT NULL (Reminder/Expiration/Payment/Promotion/General) |
| Channel | nvarchar(20) | NOT NULL (WhatsApp/SMS/Email/Push) |
| Subject | nvarchar(200) | NULL |
| Message | nvarchar(max) | NOT NULL |
| Status | nvarchar(20) | NOT NULL (Pending/Sent/Failed) |
| SentAt | datetime2 | NULL |
| Error | nvarchar(max) | NULL |
| CreatedAt | datetime2 | NOT NULL |

### Devices
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Name | nvarchar(100) | NOT NULL |
| SerialNumber | nvarchar(100) | NOT NULL, unique |
| DeviceType | nvarchar(50) | NOT NULL |
| IpAddress | nvarchar(50) | NULL |
| Port | int | NULL |
| Location | nvarchar(200) | NULL |
| IsActive | bit | NOT NULL, default 1 |
| LastSync | datetime2 | NULL |
| Notes | nvarchar(max) | NULL |
| CreatedAt | datetime2 | NOT NULL |
| CreatedBy | int | NULL |
| ModifiedAt | datetime2 | NULL |
| ModifiedBy | int | NULL |

### DeviceLogs
| Column | Type | Constraints |
|---|---|---|
| Id | bigint | PK, identity |
| DeviceId | int | FK → Devices, NOT NULL |
| LogType | nvarchar(50) | NOT NULL |
| Message | nvarchar(max) | NOT NULL |
| Timestamp | datetime2 | NOT NULL |

### Settings
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Key | nvarchar(100) | NOT NULL, unique |
| Value | nvarchar(max) | NOT NULL |
| Group | nvarchar(50) | NOT NULL |
| Description | nvarchar(500) | NULL |

### AuditLogs
| Column | Type | Constraints |
|---|---|---|
| Id | bigint | PK, identity |
| EntityName | nvarchar(100) | NOT NULL |
| EntityId | nvarchar(50) | NOT NULL |
| Action | nvarchar(20) | NOT NULL (Create/Update/Delete) |
| OldValues | nvarchar(max) | NULL |
| NewValues | nvarchar(max) | NULL |
| ChangedBy | int | FK → Users, NULL |
| ChangedAt | datetime2 | NOT NULL |

### BackupLogs
| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| FileName | nvarchar(500) | NOT NULL |
| FileSize | bigint | NULL |
| Status | nvarchar(20) | NOT NULL |
| StartedAt | datetime2 | NOT NULL |
| CompletedAt | datetime2 | NULL |
| Error | nvarchar(max) | NULL |
| CreatedAt | datetime2 | NOT NULL |

## Indexes

| Table | Index | Columns | Type |
|---|---|---|---|
| Members | IX_Members_MembershipNumber | MembershipNumber | Unique |
| Members | IX_Members_NationalId | NationalId | Unique (where not null) |
| Members | IX_Members_FingerprintId | FingerprintId | Unique (where not null) |
| Members | IX_Members_FullName | FullName | Non-clustered |
| Memberships | IX_Memberships_MemberId | MemberId | Non-clustered |
| Memberships | IX_Memberships_Status | Status | Non-clustered |
| Attendance | IX_Attendance_MemberId | MemberId | Non-clustered |
| Attendance | IX_Attendance_CheckIn | CheckIn | Non-clustered |
| Payments | IX_Payments_MemberId | MemberId | Non-clustered |
| Payments | IX_Payments_PaymentDate | PaymentDate | Non-clustered |
| Notifications | IX_Notifications_Status | Status | Non-clustered |
| Notifications | IX_Notifications_MemberId | MemberId | Non-clustered |
| DeviceLogs | IX_DeviceLogs_DeviceId | DeviceId | Non-clustered |
| AuditLogs | IX_AuditLogs_EntityName_EntityId | EntityName, EntityId | Non-clustered |

## Seed Data

### Roles
| Name | Description |
|---|---|
| Admin | Full system access |
| Manager | Operational management |
| Receptionist | Front desk operations |

### Default Users
| Username | Full Name | Role | Password |
|---|---|---|---|
| admin | System Administrator | Admin | Admin@123 |
| manager | Gym Manager | Manager | Manager@123 |
| reception | Front Desk | Receptionist | Reception@123 |

### Default Settings
| Key | Value | Group |
|---|---|---|
| GymName | Gym Manager | General |
| GymAddress | N/A | General |
| GymPhone | N/A | General |
| GymEmail | N/A | General |
| Currency | EGP | Financial |
| TaxRate | 14 | Financial |
| CheckInWindowMinutes | 15 | Attendance |
| AutoCheckout | true | Attendance |
| AutoCheckoutHour | 23 | Attendance |
| BackupTime | 02:00 | Backup |
| BackupRetentionDays | 30 | Backup |
| SmsProvider | Twilio | Notifications |
| WhatsAppEnabled | false | Notifications |
| SessionTimeoutMinutes | 60 | Security |
| MaxLoginAttempts | 5 | Security |
