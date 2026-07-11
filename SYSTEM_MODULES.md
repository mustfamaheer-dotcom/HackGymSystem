# Hack Gym - System Modules Documentation

> **Version:** Phase 1  
> **Architecture:** Clean Architecture (DDD + CQRS)  
> **Stack:** ASP.NET Core 8, EF Core, SQL Server, SignalR, gRPC, ZKTeco Biometric Integration

---

## Table of Contents

1. [Authentication](#1-authentication)
2. [Members](#2-members)
3. [Membership Plans](#3-membership-plans)
4. [Subscriptions](#4-subscriptions)
5. [Attendance](#5-attendance)
6. [Daily Sessions](#6-daily-sessions)
7. [Leads (CRM)](#7-leads-crm)
8. [Offers](#8-offers)
9. [Devices (ZKTeco)](#9-devices-zkteco)
10. [ZKTeco Integration](#10-zkteco-integration)
11. [Users](#11-users)
12. [Roles & Permissions](#12-roles--permissions)
13. [Settings](#13-settings)
14. [Dashboard](#14-dashboard)
15. [Notifications (WhatsApp)](#15-notifications-whatsapp)
16. [Audit Logs](#16-audit-logs)
17. [Backup Logs](#17-backup-logs)
18. [Background Jobs](#18-background-jobs)

---

## 1. Authentication

**Purpose:** User login, JWT token management, session control.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `api/auth/login` | Authenticates user with username/password + CAPTCHA. Returns JWT access/refresh tokens as HttpOnly cookies. |
| POST | `api/auth/refresh` | Refreshes expired JWT access token using refresh token cookie. |
| POST | `api/auth/change-password` | Changes authenticated user's password (verifies current password first). |
| POST | `api/auth/logout` | Logs out user. Deletes cookies and invalidates refresh token server-side. |
| GET | `api/auth/me` | Returns current authenticated user's profile. |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `LoginCommand` | Username, Password | Authenticates user, returns tokens |
| `LogoutCommand` | UserId | Invalidates refresh token |
| `RefreshTokenCommand` | RefreshToken | Issues new token pair |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetCurrentUserQuery` | (none) | Returns current user info |

### Validators

| Validator | Rules |
|-----------|-------|
| `LoginCommandValidator` | Username not empty, Password not empty |
| `LogoutCommandValidator` | UserId not empty |
| `RefreshTokenCommandValidator` | RefreshToken not empty |

---

## 2. Members

**Purpose:** Gym member/customer management - CRUD, search, import, expiring members, outstanding balances.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/members` | Paginated list with search, sort, pagination |
| GET | `api/members/{id}` | Single member by GUID |
| GET | `api/members/search?term=` | Search members by term |
| GET | `api/members/expiring?withinDays=` | Members with expiring subscriptions (default 7 days) |
| GET | `api/members/outstanding-balance` | Members with unpaid balances |
| POST | `api/members` | Create new member |
| PUT | `api/members/{id}` | Update member |
| DELETE | `api/members/{id}` | Soft-delete member |
| PATCH | `api/members/{id}/restore` | Restore soft-deleted member |

### Domain Entity: `Member`

| Property | Type | Description |
|----------|------|-------------|
| Code | int | Auto-increment member code |
| ReceiptNumber | string | Registration receipt number |
| FullName | string | Member full name |
| PhoneNumber | string | Contact phone |
| Email | string? | Email address |
| Nationality | string | Nationality |
| NationalId | string | National ID number |
| Gender | Gender? | Male/Female |
| DateOfBirth | DateTime? | Birth date |
| Notes | string? | Additional notes |
| Company | string? | Employer |
| Address | string? | Home address |
| Weight | decimal? | Weight in kg |
| HasDisease | bool | Has medical condition |
| DiseaseType | string? | Condition description |
| ReferralSource | ReferralSource? | How they found the gym |
| PackageId | Guid? | Linked membership plan |
| FingerprintDeviceId | Guid? | Biometric device ID |
| MemberSignature | byte[]? | Digital signature |
| AdminSignature | byte[]? | Admin signature |
| ImagePath | string? | Profile photo |
| RegistrationDate | DateTime | Registration timestamp |
| IsDeleted | bool | Soft-delete flag |

### Commands

| Command | Key Parameters | Description |
|---------|----------------|-------------|
| `CreateMemberCommand` | FullName, PhoneNumber, Nationality, NationalId, + optional fields | Creates member + optional subscription |
| `UpdateMemberCommand` | Id, FullName, PhoneNumber, Nationality, NationalId, + optional fields | Updates member + optional subscription |
| `DeleteMemberCommand` | Id | Soft-deletes member |
| `ToggleMemberStatusCommand` | Id, IsDeleted | Toggles soft-delete status |
| `RestoreMemberCommand` | Id | Restores soft-deleted member |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllMembersQuery` | Page, PageSize, SearchTerm, SortBy, SortDescending | Paginated member list |
| `GetMemberByIdQuery` | Id | Single member detail |
| `SearchMembersQuery` | SearchTerm | Search by name/phone/etc |
| `GetExpiringMembersQuery` | WithinDays (default 7) | Expiring subscriptions |
| `GetMembersWithOutstandingBalanceQuery` | (none) | Unpaid balances |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `MemberDto` | Id, Code, ReceiptNumber, FullName, Nationality, NationalId, PhoneNumber, Email, DateOfBirth, Gender, Notes, Company, Address, Weight, HasDisease, DiseaseType, ReferralSource, PackageId, PackageName, ImagePath, FingerprintDeviceId, MemberSignature, AdminSignature, RegistrationDate, IsDeleted |
| `MemberExportDto` | Code, ReceiptNumber, FullName, Nationality, NationalId, PhoneNumber, Email, DateOfBirth, Gender, Company, Address, ReferralSource, RegistrationDate, Weight, HasDisease, DiseaseType, Notes, FingerprintDeviceId, MemberSignature, AdminSignature, PlanName, SubReceiptNumber, TotalSubscriptionValue, AmountPaid, RemainingBalance, PaymentMethod, StartDate, ExpirationDate, SubStatus, FreezeStart, FreezeEnd, TotalFreezeDays, SubNotes |
| `MemberPaymentDto` | PaymentId, PaymentDate, SubscriptionReceipt, PlanName, Amount, PaymentMethod, RunningBalance, RecordedBy |
| `MemberPaymentHistoryViewModel` | MemberId, MemberCode, MemberName, MemberPhone, TotalPaid, PaymentCount, List\<MemberPaymentDto\> |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateMemberCommandValidator` | FullName (200 max), PhoneNumber (digits, 7+), Nationality (100 max), NationalId (5+), ReferralSource enum, Email format, Gender enum, DiseaseType required when HasDisease |
| `UpdateMemberCommandValidator` | Id required, FullName (200 max), PhoneNumber (11 digits), Nationality (100 max), NationalId (14 digits), ReferralSource enum, DiseaseType required when HasDisease |
| `CreateMemberDtoValidator` | FullName (200 max), PhoneNumber (digits, 7+), Nationality (100 max), NationalId (5+), ReferralSource enum, Email format, Gender enum, DiseaseType when HasDisease, PaymentMethod enum, PaidAmount <= SubscriptionPrice |

### Import

| Class | Fields |
|-------|--------|
| `MemberImportResult` | Imported, Failed, TotalRows, SuccessCount, FailedCount |
| `MemberImportRow` | RowNumber, FullName, PhoneNumber, NationalId, ReceiptNumber, FailureReason, IsSuccess |

---

## 3. Membership Plans

**Purpose:** Subscription plan/package management (monthly, yearly plans with pricing and rules).

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/plans` | Paginated list with search |
| GET | `api/plans/{id}` | Single plan by GUID |
| GET | `api/plans/active` | All active plans |
| POST | `api/plans` | Create new plan |
| PUT | `api/plans/{id}` | Update plan |
| DELETE | `api/plans/{id}` | Delete plan |
| PATCH | `api/plans/{id}/status` | Toggle active/inactive |

### Domain Entity: `MembershipPlan`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Plan name (e.g., "Monthly Premium") |
| Price | decimal | Subscription price |
| DurationDays | int | Duration in days |
| MaxVisits | int? | Maximum visits allowed |
| FreezeDays | int? | Allowed freeze days |
| Description | string? | Plan description |
| IsActive | bool | Active status |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreatePlanCommand` | Name, Price, DurationDays, MaxVisits?, FreezeDays?, Description? | Creates plan |
| `UpdatePlanCommand` | Id, Name, Price, DurationDays, MaxVisits?, FreezeDays?, Description?, IsActive | Updates plan |
| `DeletePlanCommand` | Id | Deletes plan |
| `TogglePlanStatusCommand` | Id, IsActive | Toggles active status |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllPlansQuery` | PaginationRequest | Paginated plan list |
| `GetPlanByIdQuery` | Id | Single plan |
| `GetActivePlansQuery` | (none) | Active plans only |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `PlanDto` | Id, Name, Price, DurationDays, MaxVisits, FreezeDays, Description, IsActive, CreatedAt, UpdatedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreatePlanCommandValidator` | Name not empty + 100 max, Price > 0, DurationDays > 0, MaxVisits >= 0, FreezeDays >= 0 |
| `UpdatePlanCommandValidator` | Id not empty, Name not empty + 100 max, Price > 0, DurationDays > 0, MaxVisits >= 0, FreezeDays >= 0 |
| `DeletePlanCommandValidator` | Id not empty |
| `TogglePlanStatusCommandValidator` | Id not empty |

---

## 4. Subscriptions

**Purpose:** Subscription lifecycle management - create, renew, freeze/unfreeze, payment recording.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/subscriptions` | Paginated list with filters (status, date, expiry, balance) |
| GET | `api/subscriptions/{id}` | Full subscription detail with payment history |
| POST | `api/subscriptions` | Create new subscription |
| POST | `api/subscriptions/{id}/freeze` | Freeze subscription for N days |
| POST | `api/subscriptions/{id}/unfreeze` | Unfreeze subscription |
| POST | `api/subscriptions/{id}/renew` | Renew with optional new plan/offer |
| POST | `api/subscriptions/{id}/payments` | Record a payment |

### Domain Entity: `Subscription`

| Property | Type | Description |
|----------|------|-------------|
| ReceiptNumber | string | Subscription receipt number |
| MemberId | Guid | FK to Member |
| PlanId | Guid | FK to MembershipPlan |
| OfferId | Guid? | FK to Offer (optional) |
| TotalSubscriptionValue | decimal | Total value |
| AmountPaid | decimal | Amount paid so far |
| RemainingBalance | decimal | Outstanding balance |
| PaymentMethod | PaymentMethod | Cash/Card/Transfer/etc |
| StartDate | DateTime | Subscription start |
| ExpirationDate | DateTime | Calculated expiration |
| Status | MembershipStatus | Active/Frozen/Expired/Renewed/Cancelled |
| FreezeStart | DateTime? | Freeze start date |
| FreezeEnd | DateTime? | Freeze end date |
| TotalFreezeDays | int | Total days frozen |
| AdminSignature | byte[]? | Admin signature |
| Notes | string? | Notes |

**Domain Events:**
- `SubscriptionActivatedEvent`
- `SubscriptionSuspendedEvent`
- `SubscriptionRenewedEvent`
- `SubscriptionExpiredEvent`
- `SubscriptionUpgradedEvent`

### Commands

| Command | Key Parameters | Description |
|---------|----------------|-------------|
| `CreateSubscriptionCommand` | MemberId, PlanId?, OfferId?, AmountPaid, PaymentMethod, StartDate, DurationMonths?, AdminSignature?, Notes? | Creates subscription |
| `RenewSubscriptionCommand` | PreviousSubscriptionId, NewPlanId?, OfferId?, AmountPaid, PaymentMethod, StartDate, AdminSignature?, Notes? | Renews subscription |
| `RecordSubscriptionPaymentCommand` | SubscriptionId, Amount, PaymentMethod | Records payment |
| `FreezeSubscriptionCommand` | Id, FreezeDays, Reason? | Freezes subscription |
| `UnfreezeSubscriptionCommand` | Id | Unfreezes subscription |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllSubscriptionsQuery` | PaginationRequest, SearchTerm, StatusFilter, DateFrom, DateTo, ExpiresWithinDays, HasOutstandingBalance, SortBy, SortDescending | Paginated filtered list |
| `GetSubscriptionByIdQuery` | Id | Full subscription detail |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `SubscriptionDto` | Id, ReceiptNumber, MemberId, MemberName, MemberPhone, MemberCode, MemberRegistrationDate, PlanId, PlanName, OfferId, OfferTitle, TotalSubscriptionValue, AmountPaid, RemainingBalance, PaymentMethod, StartDate, ExpirationDate, Status, LastPaymentAmount, LastPaymentDate |
| `SubscriptionDetailDto` | All SubscriptionDto fields + FreezeStart, FreezeEnd, TotalFreezeDays, AdminSignature, Notes + nested MemberDto, PlanDto, OfferDto, List\<SubscriptionPaymentDto\>, List\<SubscriptionFreezeHistoryDto\>, List\<SubscriptionTransactionLogDto\> |
| `SubscriptionPaymentDto` | Id, SubscriptionId, Amount, PaymentMethod, ReferenceNumber, EmployeeName, Notes, RunningBalance, CreatedAt |
| `SubscriptionFreezeHistoryDto` | Id, FreezeStart, FreezeEnd, FreezeDays, Reason, UnfreezeDate, CreatedAt |
| `SubscriptionTransactionLogDto` | Id, Action, Description, PerformedByName, CreatedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateSubscriptionCommandValidator` | MemberId required, PlanId or OfferId required, StartDate required, AmountPaid >= 0, PaymentMethod enum, DurationMonths > 0 if provided |
| `RenewSubscriptionCommandValidator` | PreviousSubscriptionId required, AmountPaid >= 0, StartDate required, PaymentMethod enum |
| `RecordSubscriptionPaymentCommandValidator` | SubscriptionId required, Amount > 0, PaymentMethod enum |
| `FreezeSubscriptionCommandValidator` | Id required, FreezeDays > 0 |

---

## 5. Attendance

**Purpose:** Check-in/check-out tracking, manual attendance, daily/monthly reports, real-time updates via SignalR.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/attendances` | Paginated attendance records |
| GET | `api/attendances/{id}` | Single attendance record |
| GET | `api/attendances/by-member/{memberId}` | All records for a member |
| GET | `api/attendances/today` | Today's attendance records |
| POST | `api/attendances/check-in` | Check-in a member (pushes SignalR event) |
| POST | `api/attendances/check-out` | Check-out a member |
| POST | `api/attendances/manual` | Manually create attendance (admin) |
| GET | `api/attendances/monthly-report` | Monthly summary by day |
| GET | `api/attendances/summary/{memberId}` | Member attendance summary for date |
| GET | `api/attendances/daily-summaries` | All members summary for date |
| GET | `api/attendances/dashboard-stats` | High-level attendance stats |
| GET | `api/attendances/device-health` | Health/status of attendance devices |

### Domain Entities

#### `Attendance`

| Property | Type | Description |
|----------|------|-------------|
| MemberId | Guid | FK to Member |
| DeviceId | Guid? | FK to Device |
| CheckIn | DateTime | Check-in timestamp |
| CheckOut | DateTime? | Check-out timestamp |
| IsManual | bool | Manually created flag |
| SyncStatus | string | Sync status with device |

#### `AttendanceSummary`

| Property | Type | Description |
|----------|------|-------------|
| MemberId | Guid | FK to Member |
| Date | DateTime | Summary date |
| CheckInTime | DateTime? | First check-in |
| CheckOutTime | DateTime? | Last check-out |
| WorkDurationMinutes | int? | Duration in minutes |
| Status | AttendanceStatus | Present/Late/EarlyLeave/Absent/OnLeave/HalfDay |
| PresentDays | int | Running present count |
| LateDays | int | Running late count |
| AbsentDays | int | Running absent count |
| EarlyLeaveDays | int | Running early leave count |
| HalfDays | int | Running half-day count |
| OnLeaveDays | int | Running leave count |
| TotalWorkHours | decimal | Total hours worked |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CheckInCommand` | MemberId, IsManual, DeviceId?, DeviceTimestamp? | Records check-in, updates summary, publishes event |
| `CheckOutCommand` | AttendanceId, DeviceId?, DeviceTimestamp? | Records check-out, updates summary (HalfDay if <240 min) |
| `CreateManualAttendanceCommand` | MemberId, Date, Time, CheckIn?, CheckOut?, DeviceId? | Manual admin attendance |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllAttendancesQuery` | PaginationRequest | Paginated records |
| `GetAttendanceByIdQuery` | Id | Single record |
| `GetMemberAttendancesQuery` | MemberId | Member's attendance |
| `GetTodayAttendancesQuery` | (none) | Today's records |
| `GetMonthlyReportQuery` | Year, Month | Monthly summary |
| `GetAttendanceSummaryQuery` | MemberId, Date | Member summary |
| `GetDailySummariesQuery` | Date | All members summary |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `AttendanceDto` | Id, MemberId, MemberName, Date, Time, CheckIn, CheckOut, IsManual, SyncStatus, DeviceId, DeviceName |
| `MonthlyReportDto` | MemberId, MemberName, MemberCode, Year, Month, PresentDays, LateDays, AbsentDays, EarlyLeaveDays, HalfDays, OnLeaveDays, TotalWorkHours, WorkingDaysInMonth, AttendancePercentage |
| `AttendanceSummaryDto` | Id, MemberId, MemberName, Date, CheckInTime, CheckOutTime, WorkDurationMinutes, Status |
| `DashboardStatsDto` | TotalActiveMembers, CheckedInToday, AbsentToday, LateToday, OnLeaveToday, TotalRecordsToday, DevicesOnline, LastUpdated |
| `DeviceHealthDto` | DeviceId, DeviceName, IPAddress, Port, Status, LastConnectedAt, IsActive |

### SignalR Hub: `AttendanceHub`

| Client Method | Server Method | Description |
|---------------|---------------|-------------|
| `JoinGroup(groupName)` | `JoinGroup` | Join a named group |
| `LeaveGroup(groupName)` | `LeaveGroup` | Leave a named group |
| (server push) | `AttendancePushed` | Pushes attendance events with member details, image, package, phone, timestamp |

---

## 6. Daily Sessions

**Purpose:** Walk-in/daily visit sessions with payment tracking (non-member visitors).

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/DailySessions` | List with search and date filtering |
| GET | `/DailySessions/create` | Show create form |
| POST | `/DailySessions/create` | Submit new daily session |
| GET | `/DailySessions/get-plan-price` | AJAX: returns plan price by ID |

### Domain Entity: `DailySession`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Visitor name |
| Phone | string | Contact phone |
| VisitDate | DateTime | Visit date |
| PlanId | Guid? | FK to MembershipPlan (optional) |
| Amount | decimal | Session amount |
| PaidAmount | decimal | Amount paid |
| RemainingBalance | decimal | Calculated remaining |
| PaymentMethod | PaymentMethod | Payment method |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateDailySessionCommand` | Name, Phone, VisitDate, PlanId?, Amount, PaidAmount, PaymentMethod | Creates session + auto-creates Lead (WalkIn source) |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllDailySessionsQuery` | SearchTerm?, DateFrom?, DateTo?, Page, PageSize | Paginated list with filters |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `DailySessionDto` | Id, Name, Phone, VisitDate, PlanId, PlanName, Amount, PaidAmount, RemainingBalance, PaymentMethod, CreatedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateDailySessionCommandValidator` | Name (200 max), Phone (20 max), VisitDate required, Amount >= 0, PaidAmount >= 0, PaymentMethod enum |

---

## 7. Leads (CRM)

**Purpose:** Potential customer/lead management with follow-ups, conversion to member, import, and statistics.

### API Endpoints

(MVC-based - no dedicated REST API)

### Domain Entities

#### `Lead`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Lead name |
| Phone | string | Contact phone |
| Email | string? | Email address |
| Gender | Gender? | Male/Female |
| Source | LeadSource | How they found the gym |
| InterestedPackageId | Guid? | FK to MembershipPlan |
| Status | LeadStatus | New/Contacted/Interested/Converted/Lost |
| NextFollowUpDate | DateTime? | Scheduled follow-up |
| Notes | string? | Additional notes |

#### `LeadFollowUp`

| Property | Type | Description |
|----------|------|-------------|
| LeadId | Guid | FK to Lead |
| Notes | string | Follow-up notes |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateLeadCommand` | Name, Phone, Source, InterestedPackageId?, Notes?, Email?, Gender? | Creates lead |
| `UpdateLeadCommand` | Id, Name, Phone, Source, InterestedPackageId?, Status, NextFollowUpDate?, Notes?, Email?, Gender? | Updates lead |
| `DeleteLeadCommand` | Id | Hard deletes lead |
| `ConvertToMemberCommand` | LeadId, PlanId, AmountPaid, PaymentMethod | Creates Member + Subscription, marks Lead as Converted |
| `AddFollowUpCommand` | LeadId, Notes | Adds follow-up record |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllLeadsQuery` | SearchTerm?, StatusFilter?, GenderFilter?, SourceFilter?, PackageFilter?, DateFrom?, DateTo?, NextFollowUpFrom?, NextFollowUpTo?, HasFollowUp?, SortBy?, SortDescending, Page, PageSize | Paginated filtered list |
| `GetLeadByIdQuery` | Id | Single lead |
| `GetFollowUpsQuery` | LeadId | Lead's follow-ups |
| `GetLeadStatsQuery` | (none) | Lead statistics |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `LeadDto` | Id, Name, Phone, Email, Gender, Source, InterestedPackageId, InterestedPackageName, Status, NextFollowUpDate, Notes, CreatedAt, FollowUpCount |
| `LeadFollowUpDto` | Id, LeadId, Notes, CreatedAt |
| `LeadStatsDto` | TotalLeads, NewThisWeek, FollowUpsDueToday, ConvertedCount, ConversionRate |
| `CreateLeadDto` | Name, Phone, Email, Gender, Source, InterestedPackageId, Notes |
| `UpdateLeadDto` | Id + CreateLeadDto fields + Status, NextFollowUpDate |
| `ConvertToMemberDto` | LeadId, PackageId, SubscriptionPrice, PaidAmount |

### Import

| Class | Fields |
|-------|--------|
| `LeadImportResult` | Imported, Failed, TotalRows, SuccessCount, FailedCount |
| `LeadImportRow` | RowNumber, Name, Phone, FailureReason, IsSuccess |

---

## 8. Offers

**Purpose:** Discounts, promotions, and special offers (BonusDuration, FixedPrice, ExtraFreeze, FreeRegistration).

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/offers` | Paginated list |
| GET | `api/offers/{id}` | Single offer |
| GET | `api/offers/active` | Active offers only |
| GET | `api/offers/expired` | Expired offers |
| GET | `api/offers/package/{packageId}` | Offers linked to a package |
| POST | `api/offers` | Create offer |
| PUT | `api/offers/{id}` | Update offer |
| DELETE | `api/offers/{id}` | Delete offer |
| POST | `api/offers/apply` | Calculate discounted price for a package |

### Domain Entity: `Offer`

| Property | Type | Description |
|----------|------|-------------|
| OfferTitle | string | Offer name |
| LinkedPackageId | Guid? | FK to MembershipPlan |
| OfferType | OfferType | BonusDuration/FixedPrice/ExtraFreeze/FreeRegistration |
| BonusMonths | int? | Bonus months (BonusDuration type) |
| BonusDays | int? | Bonus days (BonusDuration type) |
| OfferPrice | decimal? | Discounted price (FixedPrice type) |
| ExtraFreezeDays | int? | Extra freeze days (ExtraFreeze type) |
| Description | string? | Offer description |
| StartDate | DateTime | Offer start |
| EndDate | DateTime | Offer end |
| IsActive | bool | Active status |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateOfferCommand` | OfferTitle, OfferType, StartDate, EndDate, LinkedPackageId?, BonusMonths?, BonusDays?, OfferPrice?, ExtraFreezeDays?, Description? | Creates offer |
| `UpdateOfferCommand` | Id, OfferTitle, OfferType, StartDate, EndDate, LinkedPackageId?, BonusMonths?, BonusDays?, OfferPrice?, ExtraFreezeDays?, Description? | Updates offer |
| `DeleteOfferCommand` | Id | Deletes offer |
| `ToggleOfferStatusCommand` | Id, IsActive | Toggles active status |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllOffersQuery` | PaginationRequest | Paginated list |
| `GetOfferByIdQuery` | Id | Single offer |
| `GetActiveOffersQuery` | (none) | Active offers |
| `GetExpiredOffersQuery` | (none) | Expired offers |
| `GetOffersByPackageQuery` | PackageId | Offers for a package |
| `ApplyOfferQuery` | OfferId, PackageId?, PackagePrice?, PackageDurationMonths? | Calculates discount |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `OfferDto` | Id, OfferTitle, LinkedPackageId, LinkedPackageName, OfferType, BonusMonths, BonusDays, OfferPrice, ExtraFreezeDays, Description, StartDate, EndDate, IsActive, OfferTypeDisplay (computed) |
| `AppliedOfferDto` | OfferId, OfferTitle, OfferType, OriginalDurationMonths, OriginalPrice, FinalDurationMonths, FinalPrice, BonusDays, ExtraFreezeDays, Description |
| `CreateOfferDto` | OfferTitle, LinkedPackageId, OfferType, OfferPrice, BonusMonths, BonusDays, Description, StartDate, EndDate |
| `UpdateOfferDto` | Id + CreateOfferDto fields |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateOfferCommandValidator` | OfferTitle (200 max), OfferType enum, EndDate > StartDate, conditional rules per OfferType |
| `UpdateOfferCommandValidator` | Same as Create + Id required |
| `DeleteOfferCommandValidator` | Id not empty |
| `ToggleOfferStatusCommandValidator` | Id not empty |

### Offer Type Rules

| OfferType | Required Fields |
|-----------|-----------------|
| BonusDuration | BonusMonths > 0 OR BonusDays > 0 |
| FixedPrice | OfferPrice > 0 |
| ExtraFreeze | ExtraFreezeDays > 0 |
| FreeRegistration | OfferPrice null |

---

## 9. Devices (ZKTeco)

**Purpose:** Biometric device management (ZKTeco fingerprint/face readers).

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/devices` | Paginated list |
| GET | `api/devices/{id}` | Single device |
| GET | `api/devices/active` | Active devices |
| POST | `api/devices` | Register device |
| PUT | `api/devices/{id}` | Update device |
| DELETE | `api/devices/{id}` | Delete device |
| POST | `api/devices/{id}/sync` | Trigger sync with ZKTeco bridge |
| GET | `api/devices/{id}/logs` | Last 100 log entries |
| POST | `api/devices/{id}/test-connection` | Test connection to bridge |
| PATCH | `api/devices/{id}/status` | Toggle device status |

### Domain Entity: `Device`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Device name |
| IPAddress | string | IP address |
| Port | int | Port number |
| Model | string? | Device model |
| SerialNumber | string? | Serial number |
| FirmwareVersion | string? | Firmware version |
| IsActive | bool | Active status |
| Status | DeviceStatus | Online/Offline/Error |
| LastConnectedAt | DateTime? | Last connection time |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateDeviceCommand` | Name, IPAddress, Port, Model?, SerialNumber? | Registers device |
| `UpdateDeviceCommand` | Id, Name, IPAddress, Port, Model?, SerialNumber? | Updates device |
| `DeleteDeviceCommand` | Id | Deletes device |
| `ToggleDeviceStatusCommand` | Id, Status | Sets device status |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllDevicesQuery` | PaginationRequest | Paginated list |
| `GetDeviceByIdQuery` | Id | Single device |
| `GetActiveDevicesQuery` | (none) | Active devices |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `DeviceDto` | Id, Name, IPAddress, Port, Model, SerialNumber, FirmwareVersion, IsActive, Status, LastConnectedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateDeviceCommandValidator` | Name (100 max), IPAddress regex `^(\d{1,3}\.){3}\d{1,3}$`, Port > 0 |
| `UpdateDeviceCommandValidator` | Same as Create + Id required |
| `DeleteDeviceCommandValidator` | Id not empty |
| `ToggleDeviceStatusCommandValidator` | Id not empty, Status enum |

---

## 10. ZKTeco Integration

**Purpose:** Biometric device communication - fingerprint/face enrollment, attendance polling, user reconciliation.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/zkteco/status` | Device connection status |
| GET | `api/zkteco/sync-logs` | Paginated sync log history |
| POST | `api/zkteco/reconcile` | Reconcile users between DB and device |
| POST | `api/zkteco/enroll` | Enroll biometric template on device |
| POST | `api/zkteco/testconnection` | Test bridge connection |

### Device-to-Server Endpoints (Anonymous, API Key Auth)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `api/zktecoattendance/push` | Single attendance push from device |
| GET | `api/zktecoattendance/health` | Service health check |
| POST | `api/zktecoattendance/push-batch` | Batch attendance push |
| POST | `api/zktecoattendance/device-info` | Device info push |
| POST | `api/zktecoattendance/sync-users` | Enrolled users sync |

### Domain Entities

#### `DeviceMemberMapping`

| Property | Type | Description |
|----------|------|-------------|
| MemberId | Guid | FK to Member |
| DeviceEnrollmentId | int | Enrollment ID on device |
| BiometricType | BiometricType | Fingerprint/Face |
| FingerIndex | int? | Finger index (0-9) for fingerprints |
| EnrolledAt | DateTime | Enrollment timestamp |
| IsDeleted | bool | Soft-delete flag |

#### `SyncAuditLog`

| Property | Type | Description |
|----------|------|-------------|
| EventType | string | Event type |
| Direction | string | Inbound/Outbound |
| EntityId | Guid? | Related entity |
| Payload | string? | JSON payload |
| Status | string | Success/Failed |
| ErrorMessage | string? | Error details |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `EnrollBiometricCommand` | MemberId, Type, FingerIndex? | Enrolls member via bridge, saves mapping, sets privilege |
| `ReconcileUsersCommand` | (none) | Iterates all active mappings, sets privilege based on subscription status |
| `TestConnectionCommand` | (none) | Tests bridge connection |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetDeviceStatusQuery` | (none) | Bridge health status |
| `GetSyncLogsQuery` | Page, PageSize, EventType?, Status?, From?, To? | Paginated sync logs |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `DeviceStatusDto` | IsConnected, EnrolledUserCount, FreeMemory, FirmwareVersion, ConsecutiveFailures, LastConnectedAt, UptimeMs, MaxFingerprints, MaxFaces |
| `SyncLogDto` | Id, EventType, Direction, EntityId, Payload, Status, ErrorMessage, CreatedAt |
| `ReconcileResult` | UsersChecked, DiscrepanciesFixed, Details |
| `TestConnectionResult` | (defined in Common) |

### Event Handlers

| Handler | Handles Events | Description |
|---------|----------------|-------------|
| `ZKTecoSyncHandler` | SubscriptionActivatedEvent, SubscriptionExpiredEvent, SubscriptionUpgradedEvent, SubscriptionSuspendedEvent, SubscriptionRenewedEvent | Syncs device privileges via IZKTecoBridgeClient |

### ZKTeco Bridge Service

| Component | Description |
|-----------|-------------|
| `ZKDeviceManager` | Device connection and operations |
| `DeviceHealthMonitor` | Health monitoring (hosted service) |
| `AttendancePollingWorker` | Polls device for attendance events |
| `ZKTecoBridgeGrpcClient` | gRPC client for bridge communication |

---

## 11. Users

**Purpose:** System user management (admins, staff) with authentication and role assignment.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/users` | Paginated list |
| GET | `api/users/{id}` | Single user |
| GET | `api/users/roles` | Available roles |
| POST | `api/users` | Create user |
| PUT | `api/users/{id}` | Update user |
| DELETE | `api/users/{id}` | Delete user |

### Domain Entity: `User`

| Property | Type | Description |
|----------|------|-------------|
| Username | string | Login username |
| PasswordHash | string | BCrypt hashed password |
| FullName | string | Display name |
| Email | string | Email address |
| Phone | string? | Phone number |
| RoleId | Guid | FK to Role |
| IsActive | bool | Active status |
| IsPasswordChangeRequired | bool | Force password change on next login |
| RefreshToken | string? | Current refresh token |
| PreviousRefreshTokenHash | string? | Previous token hash (family tracking) |
| RefreshTokenExpiry | DateTime? | Token expiration |
| LastLoginAt | DateTime? | Last login timestamp |
| FailedLoginAttempts | int | Failed login counter |
| LockoutEnd | DateTime? | Account lockout end |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateUserCommand` | Username, Password, FullName, Email, Phone?, RoleId | Creates user (BCrypt hash, checks duplicate username/email) |
| `UpdateUserCommand` | Id, FullName, Email, Phone?, RoleId, IsActive | Updates user (checks duplicate email) |
| `DeleteUserCommand` | Id | Deletes user |
| `ChangePasswordCommand` | UserId, CurrentPassword, NewPassword | Changes password (BCrypt verify + rehash) |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllUsersQuery` | PaginationRequest | Paginated list |
| `GetUserByIdQuery` | Id | Single user |
| `GetRolesQuery` | (none) | Available roles (excludes "Trainer" role) |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `UserListItemDto` | Id, Username, FullName, Email, Phone, RoleName, RoleId, IsActive, CreatedAt, LastLoginAt |
| `RoleDto` | Id, Name |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateUserCommandValidator` | Username (50 max), Password (6+ min), FullName (200 max), Email (200 max), RoleId required |
| `UpdateUserCommandValidator` | Id required, FullName (200 max), Email (200 max), RoleId required |
| `DeleteUserCommandValidator` | Id not empty |
| `ChangePasswordCommandValidator` | CurrentPassword required, NewPassword (6+ min) |

---

## 12. Roles & Permissions

**Purpose:** Role-based access control (RBAC) with granular permissions.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/roles` | All roles |
| GET | `api/roles/{id}` | Single role with permissions |
| GET | `api/roles/permissions` | All permissions grouped by category |
| POST | `api/roles` | Create role |
| PUT | `api/roles/{id}` | Update role |
| PUT | `api/roles/{id}/permissions` | Replace role permissions |
| DELETE | `api/roles/{id}` | Delete role |

### Domain Entities

#### `Role`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Role name |
| Description | string? | Description |
| IsSystem | bool | System role (cannot delete) |
| IsActive | bool | Active status |

#### `Permission`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Permission name |
| Description | string? | Description |
| Module | string | Module category |

#### `RolePermission`

| Property | Type | Description |
|----------|------|-------------|
| RoleId | Guid | FK to Role |
| PermissionId | Guid | FK to Permission |

### Permission Constants (from `Permissions.cs`)

Permissions are organized by module:

| Module | Permissions |
|--------|-------------|
| Members | Members.View, Members.Create, Members.Edit, Members.Delete |
| Subscriptions | Subscriptions.View, Subscriptions.Create, Subscriptions.Edit, Subscriptions.Delete |
| Attendance | Attendance.View, Attendance.Manage |
| Plans | Plans.View, Plans.Create, Plans.Edit, Plans.Delete |
| Offers | Offers.View, Offers.Create, Offers.Edit, Offers.Delete |
| Devices | Devices.View, Devices.Create, Devices.Edit, Devices.Delete |
| Users | Users.View, Users.Create, Users.Edit, Users.Delete |
| Roles | Roles.View, Roles.Create, Roles.Edit, Roles.Delete |
| Settings | Settings.View, Settings.Edit |
| Dashboard | Dashboard.View |
| Leads | Leads.View, Leads.Create, Leads.Edit, Leads.Delete |
| DailySessions | DailySessions.View, DailySessions.Create |

---

## 13. Settings

**Purpose:** System-wide configuration (gym info, currency, attendance rules, etc.) stored as key-value pairs.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/settings` | All settings |
| GET | `api/settings/{id}` | Single setting by ID |
| GET | `api/settings/by-key/{key}` | Setting by key name |
| GET | `api/settings/by-group/{group}` | Settings by group |
| POST | `api/settings` | Create setting |
| PUT | `api/settings/{id}` | Update setting value |
| DELETE | `api/settings/{id}` | Delete setting |

### Domain Entity: `Setting`

| Property | Type | Description |
|----------|------|-------------|
| Key | string | Setting key (unique) |
| Value | string | Setting value |
| Group | string? | Group category |
| Description | string? | Description |
| IsEncrypted | bool | Encrypted value flag |

### Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateSettingCommand` | Key, Value, Group?, Description?, IsEncrypted | Creates setting |
| `UpdateSettingCommand` | Id, Value | Updates value |
| `DeleteSettingCommand` | Id | Deletes setting |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllSettingsQuery` | (none) | All settings |
| `GetSettingByIdQuery` | Id | Single setting |
| `GetSettingByKeyQuery` | Key | Lookup by key |
| `GetSettingsByGroupQuery` | Group | Settings in group |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `SettingDto` | Id, Key, Value, Group, Description, IsEncrypted, CreatedAt, UpdatedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateSettingCommandValidator` | Key (100 max), Value required |
| `UpdateSettingCommandValidator` | Id required, Value required |
| `DeleteSettingCommandValidator` | Id required |

---

## 14. Dashboard

**Purpose:** Aggregate statistics, analytics, charts, and KPIs for gym management.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `api/dashboard` | Full dashboard data |

### Queries

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetDashboardQuery` | (none) | Basic dashboard stats |
| `GetDetailedDashboardQuery` | Year?, Month?, From?, To? | Full analytics with date range |

### DTOs

#### Basic Dashboard

| DTO | Key Fields |
|-----|------------|
| `DashboardDto` | TotalMembers, ActiveMembers, ActiveMemberships, TodayCheckIns, ExpiringMemberships, List\<MembershipStatsDto\> |
| `MembershipStatsDto` | PlanName, Count |

#### Detailed Dashboard

| DTO | Key Fields |
|-----|------------|
| `DetailedDashboardDto` | Members, Memberships, Attendance, Subscriptions, TopRevenuePackage, PlanDistribution, SubscriptionRevenueByPlan, RecentActivity, DailyStats, MonthlyStats, SubscriptionDailyRevenue, SubscriptionMonthlyRevenue, AovTrend, RenewalRateByPlan, TopSpenders, OverduePayments, PaymentDelayStats, FreezeImpact, OverallRenewalRate |
| `MembersStatsDto` | TotalMembers, ActiveMembers, NewThisMonth, MaleCount, FemaleCount, ExpiredSubscriptions, ExpiringThisWeek |
| `MembershipsStatsDto` | Total, Active, Frozen, Expired, Cancelled |
| `AttendanceStatsDto` | TodayTotal, ThisWeekTotal, ThisMonthTotal, CurrentlyCheckedIn, AvgDailyThisMonth |
| `SubscriptionStatsDto` | TotalSubscriptions, ActiveSubscriptions, FrozenSubscriptions, ExpiredSubscriptions, RenewedSubscriptions, TotalRevenue, RevenueThisMonth, RevenueThisWeek, TodayRevenue, TotalOutstanding, AverageSubscriptionValue, ExpiringNext7Days, ExpiringNext30Days, SubscriptionsWithOffers, FreezesThisMonth |
| `PlanDistributionDto` | PlanName, Count, Percent |
| `SubscriptionRevenueByPlanDto` | PlanName, Count, TotalPaid, TotalOutstanding, Percent |
| `SubscriptionDailyRevenueDto` | Label, SubscriptionCount, Revenue |
| `SubscriptionMonthlyRevenueDto` | Label, Revenue, NewSubscriptions |
| `RecentActivityDto` | Type, Description, Timestamp |
| `TopPackageDto` | PlanName, TotalPaid, SubCount, PercentOfRevenue |
| `AovTrendDto` | Label, Aov, SubCount, Revenue |
| `RenewalRateByPlanDto` | PlanName, TotalSubscriptions, RenewedCount, RenewalRate, ActiveCount |
| `TopSpenderDto` | MemberName, Phone, TotalPaid, TopPlan, SubscriptionCount |
| `OverduePaymentDto` | MemberName, Phone, ReceiptNumber, RemainingBalance, TotalValue, ExpirationDate, PlanName |
| `PaymentDelayStatsDto` | AverageDelayDays, List\<PaymentDelayTrendDto\> |
| `PaymentDelayTrendDto` | Label, AvgDays, PaymentCount |
| `FreezeImpactDto` | Label, FreezeCount, Revenue, ExpectedSubscriptions |

---

## 15. Notifications (WhatsApp)

**Purpose:** WhatsApp/SMS/Email messaging with template management.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `api/whatsapp/send` | Send free-text WhatsApp message |
| POST | `api/whatsapp/send-member` | Send template-based message to member |

### Domain Entity: `WhatsAppTemplate`

| Property | Type | Description |
|----------|------|-------------|
| Name | string | Template name |
| MessageBody | string | Message body (supports placeholders) |
| IsActive | bool | Active status |
| TriggerType | NotificationType? | Auto-trigger type |

#### `Notification`

| Property | Type | Description |
|----------|------|-------------|
| MemberId | Guid | FK to Member |
| Title | string | Notification title |
| Body | string | Message body |
| IsRead | bool | Read status |
| ReadAt | DateTime? | Read timestamp |

### Commands (WhatsApp Templates)

| Command | Parameters | Description |
|---------|------------|-------------|
| `CreateTemplateCommand` | Name, MessageBody, NotificationType? TriggerType | Creates template (checks duplicate name) |
| `UpdateTemplateCommand` | Id, Name, MessageBody, IsActive, NotificationType? TriggerType | Updates template (checks duplicate name) |

### Queries (WhatsApp Templates)

| Query | Parameters | Description |
|-------|------------|-------------|
| `GetAllTemplatesQuery` | (none) | All templates |

### DTOs

| DTO | Key Fields |
|-----|------------|
| `WhatsAppTemplateDto` | Id, Name, MessageBody, IsActive, TriggerType, CreatedAt, UpdatedAt |

### Validators

| Validator | Rules |
|-----------|-------|
| `CreateTemplateCommandValidator` | Name (200 max), MessageBody (2000 max) |
| `UpdateTemplateCommandValidator` | Id required, Name (200 max), MessageBody (2000 max) |

---

## 16. Audit Logs

**Purpose:** Change tracking for compliance and debugging.

### Domain Entities

#### `AuditLog`

| Property | Type | Description |
|----------|------|-------------|
| UserId | Guid? | FK to User |
| Action | string | Action performed |
| EntityType | string | Entity type affected |
| EntityId | Guid? | Entity ID |
| OldValues | string? | JSON of old values |
| NewValues | string? | JSON of new values |
| IpAddress | string? | Client IP |

#### `PermissionAuditLog`

| Property | Type | Description |
|----------|------|-------------|
| UserId | Guid? | FK to User |
| Action | string | Action performed |
| RoleName | string | Role affected |
| OldPermissions | string? | JSON of old permissions |
| NewPermissions | string? | JSON of new permissions |
| IpAddress | string? | Client IP |

---

## 17. Backup Logs

**Purpose:** Database backup management and tracking.

### Domain Entity: `BackupLog`

| Property | Type | Description |
|----------|------|-------------|
| FileName | string | Backup file name |
| FilePath | string | File path |
| Size | long? | File size in bytes |
| Status | string | Success/Failed |
| ErrorMessage | string? | Error details |
| BackupDate | DateTime | Backup timestamp |

---

## 18. Background Jobs

**Purpose:** Automated scheduled tasks for maintenance and notifications.

### Jobs

| Job | Trigger | Description |
|-----|---------|-------------|
| `SubscriptionExpiryJob` | Periodic | Finds Active/Frozen subscriptions past ExpirationDate, calls `MarkExpired()`, creates Notification per member |
| `SubscriptionRenewalReminderJob` | Periodic | Finds Active subscriptions expiring within 7 days, creates Notification if not already present |
| `LeadFollowUpJob` | Periodic | Finds leads with NextFollowUpDate <= now (not Converted/Lost), logs them |
| `OfferExpiryHostedService` | Periodic | Deactivates expired offers |
| `PeriodicReconciliationWorker` | Periodic | Syncs device attendance data with database |

---

## Cross-Cutting Concerns

### Shared Components (`Gym.Shared`)

| Component | Description |
|-----------|-------------|
| `BaseEntity` | Base entity with Id, CreatedAt, ModifiedAt |
| `Guard` | Guard clauses for validation |
| `Result<T>` | Result pattern for error handling |
| `DomainEvents` | IDomainEvent marker interface |
| `Permissions` | Compile-time safe permission constants |
| `Enums` | Gender, MemberStatus, MembershipStatus, PaymentMethod, etc. |
| `BiometricEnums` | BiometricType, SyncEventType, FingerIndex |

### Infrastructure Services

| Service | Description |
|---------|-------------|
| `AuthService` | JWT token generation, password hashing (BCrypt) |
| `CacheService` | IMemoryCache wrapper |
| `CaptchaService` | CAPTCHA generation/validation |
| `ExcelImportService` | Excel file import for members/leads |
| `MemberService` | Member business logic |
| `OfferService` | Offer calculation logic |
| `RolePermissionService` | RBAC permission management |
| `WhatsAppService` | WhatsApp API integration |

### Localization

- **Languages:** Arabic (ar), English (en)
- **RTL Support:** Full right-to-left layout support
- **Cookie-based:** Language preference stored in cookie

### Deployment

- **Docker:** SQL Server + API containerized via `docker-compose.yml`
- **Scripts:** ZKTeco SDK registration and integration tests
- **Batch Files:** System startup/stop scripts

---

## Module Statistics

| Module | Commands | Queries | DTOs | Validators | API Endpoints |
|--------|----------|---------|------|------------|---------------|
| Authentication | 3 | 1 | 2 | 3 | 5 |
| Members | 5 | 5 | 5 | 3 | 9 |
| Membership Plans | 4 | 3 | 1 | 4 | 7 |
| Subscriptions | 5 | 2 | 5 | 4 | 7 |
| Attendance | 3 | 7 | 5 | 3 | 12 |
| Daily Sessions | 1 | 1 | 1 | 1 | 4 |
| Leads (CRM) | 5 | 4 | 5 | 0 | - |
| Offers | 4 | 6 | 4 | 4 | 9 |
| Devices | 4 | 3 | 1 | 4 | 10 |
| ZKTeco Integration | 3 | 2 | 3 | 1 | 10 |
| Users | 4 | 3 | 2 | 4 | 6 |
| Roles & Permissions | - | - | - | - | 7 |
| Settings | 3 | 4 | 1 | 3 | 7 |
| Dashboard | 0 | 2 | 15+ | 0 | 1 |
| WhatsApp | 2 | 1 | 1 | 2 | 2 |
| **TOTAL** | **46** | **44** | **~55** | **36** | **96** |
