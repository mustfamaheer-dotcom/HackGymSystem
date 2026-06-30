# API Reference

Base URL: `http://localhost:5000/api`

All endpoints (except Auth Login/Refresh) require:

```
Authorization: Bearer <accessToken>
```

Responses follow the standard envelope:

```json
{
  "succeeded": true,
  "message": null,
  "errors": null,
  "data": { }
}
```

Paginated responses:

```json
{
  "succeeded": true,
  "data": {
    "items": [],
    "page": 1,
    "pageSize": 10,
    "totalCount": 50,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

---

## Auth

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/login` | ❌ | Login with username/password |
| POST | `/api/auth/refresh` | ❌ | Refresh expired token |
| POST | `/api/auth/logout` | ✅ | Invalidate refresh token |
| GET | `/api/auth/me` | ✅ | Get current user profile |

### POST /api/auth/login

```json
// Request
{ "username": "admin", "password": "Admin@123" }

// Response
{
  "succeeded": true,
  "data": {
    "accessToken": "eyJ...",
    "refreshToken": "dGhpcyBpcyBh...",
    "expiresAt": "2026-06-22T17:44:54Z"
  }
}
```

### POST /api/auth/refresh

```json
// Request
{ "refreshToken": "dGhpcyBpcyBh..." }

// Response — same shape as login
```

### GET /api/auth/me

```json
// Response
{
  "succeeded": true,
  "data": {
    "id": 1,
    "username": "admin",
    "email": "admin@gym.com",
    "fullName": "System Administrator",
    "role": "Admin",
    "isActive": true
  }
}
```

---

## Members

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/members?page=1&pageSize=10&search=&sortBy=&sortDirection=` | Paginated member list |
| GET | `/api/members/{id}` | Get member by ID |
| POST | `/api/members` | Create member |
| PUT | `/api/members/{id}` | Update member |
| DELETE | `/api/members/{id}` | Delete member |

### POST /api/members

```json
// Request
{
  "fullName": "John Doe",
  "phone": "+201234567890",
  "email": "john@example.com",
  "nationalId": "12345678901234",
  "address": "Cairo, Egypt",
  "dateOfBirth": "1990-01-15",
  "gender": "Male",
  "fingerprintId": "FP001",
  "notes": "VIP member"
}

// Response
{
  "succeeded": true,
  "data": {
    "id": 1,
    "fullName": "John Doe",
    "phone": "+201234567890",
    "email": "john@example.com",
    "membershipNumber": "M-2026-00001",
    "isActive": true,
    "createdAt": "2026-06-21T17:44:54Z",
    ...
  }
}
```

---

## Membership Plans

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/plans` | List all plans (no pagination) |
| GET | `/api/plans/{id}` | Get plan by ID |
| POST | `/api/plans` | Create plan |
| PUT | `/api/plans/{id}` | Update plan |
| DELETE | `/api/plans/{id}` | Delete plan |
| PUT | `/api/plans/{id}/toggle-active` | Toggle plan active status |

### POST /api/plans

```json
// Request
{
  "name": "Monthly Premium",
  "description": "Full access to all facilities",
  "durationDays": 30,
  "price": 500.00,
  "maxVisits": 30,
  "freezeDays": 5
}
```

---

## Memberships

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/memberships?page=1&pageSize=10&status=Active&memberId=` | Paginated membership list |
| GET | `/api/memberships/{id}` | Get membership by ID |
| GET | `/api/memberships/by-member/{memberId}` | Get member's memberships |
| POST | `/api/memberships` | Create membership |
| POST | `/api/memberships/{id}/renew` | Renew membership |
| PUT | `/api/memberships/{id}/cancel` | Cancel membership |
| PUT | `/api/memberships/{id}/freeze` | Freeze membership |
| PUT | `/api/memberships/{id}/unfreeze` | Unfreeze membership |

### POST /api/memberships

```json
// Request
{
  "memberId": 1,
  "planId": 1,
  "startDate": "2026-06-21"
}

// Response
{
  "succeeded": true,
  "data": {
    "id": 1,
    "memberId": 1,
    "memberName": "John Doe",
    "planId": 1,
    "planName": "Monthly Premium",
    "startDate": "2026-06-21",
    "endDate": "2026-07-21",
    "status": "Active",
    "remainingVisits": 30
  }
}
```

### PUT /api/memberships/{id}/freeze

```json
// Request
{ "days": 10 }
```

---

## Attendance

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/attendance?page=1&pageSize=15&dateFrom=&dateTo=&memberId=` | Paginated attendance list |
| GET | `/api/attendance/today` | Today's attendance records |
| POST | `/api/attendance/check-in` | Manual check-in |
| PUT | `/api/attendance/{id}/check-out` | Check out |
| GET | `/api/attendance/member/{memberId}` | Member's attendance history |

### POST /api/attendance/check-in

```json
// Request
{
  "memberId": 1,
  "method": "Manual",
  "deviceId": null
}
```

### PUT /api/attendance/{id}/check-out

No request body required.

---

## Payments

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/payments?page=1&pageSize=10&dateFrom=&dateTo=&memberId=` | Paginated payment list |
| GET | `/api/payments/{id}` | Get payment by ID |
| POST | `/api/payments` | Create payment |
| GET | `/api/payments/by-member/{memberId}` | Member's payment history |

### POST /api/payments

```json
// Request
{
  "memberId": 1,
  "amount": 500.00,
  "amountPaid": 500.00,
  "paymentDate": "2026-06-21",
  "method": "Cash",
  "reference": "INV-001",
  "notes": "Paid in full",
  "membershipId": 1,
  "offerId": null
}
```

---

## Offers

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/offers` | List all offers |
| GET | `/api/offers/{id}` | Get offer by ID |
| POST | `/api/offers` | Create offer |
| PUT | `/api/offers/{id}` | Update offer |
| DELETE | `/api/offers/{id}` | Delete offer |

### POST /api/offers

```json
// Request
{
  "code": "SUMMER2026",
  "name": "Summer Sale",
  "description": "20% off all annual plans",
  "discountType": "Percentage",
  "discountValue": 20,
  "startDate": "2026-06-01",
  "endDate": "2026-08-31",
  "maxUsage": 100
}
```

---

## Devices

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/devices` | List all devices |
| GET | `/api/devices/{id}` | Get device by ID |
| POST | `/api/devices` | Create device |
| PUT | `/api/devices/{id}` | Update device |
| DELETE | `/api/devices/{id}` | Delete device |
| GET | `/api/devices/{id}/logs` | Get device logs |
| POST | `/api/devices/{id}/sync` | Sync device data |

### POST /api/devices

```json
// Request
{
  "name": "Main Entrance",
  "serialNumber": "MB2000-001",
  "deviceType": "Fingerprint",
  "ipAddress": "192.168.1.100",
  "port": 4370,
  "location": "Main Gate"
}
```

---

## Notifications

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/notifications?page=1&pageSize=10&memberId=&status=` | Paginated notification list |
| POST | `/api/notifications` | Send notification |
| POST | `/api/notifications/{id}/resend` | Resend failed notification |

### POST /api/notifications

```json
// Request
{
  "memberId": 1,
  "type": "Reminder",
  "channel": "WhatsApp",
  "subject": "Membership Expiring",
  "message": "Your membership will expire on 2026-07-21. Please renew."
}
```

---

## Settings

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/settings?group=General` | List settings (filtered by optional group) |
| PUT | `/api/settings/{id}` | Update setting value |

### PUT /api/settings/{id}

```json
// Request
{ "value": "Gym Manager" }
```

---

## Dashboard

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/dashboard` | Aggregate statistics |

### GET /api/dashboard

```json
{
  "succeeded": true,
  "data": {
    "totalMembers": 150,
    "activeMembers": 120,
    "activeMemberships": 115,
    "todayCheckIns": 42,
    "todayCheckOuts": 38,
    "monthlyRevenue": 45000.00,
    "pendingPayments": 8,
    "expiringMemberships": 5,
    "recentActivities": [
      { "id": 1, "type": "CheckIn", "description": "John Doe checked in", "timestamp": "2026-06-21T08:30:00Z" }
    ],
    "revenueChart": [
      { "label": "Jan", "value": 35000 },
      { "label": "Feb", "value": 38000 }
    ],
    "attendanceChart": [
      { "label": "Mon", "value": 85 },
      { "label": "Tue", "value": 92 }
    ]
  }
}
```

---

## Error Responses

### Validation Error (400)

```json
{
  "succeeded": false,
  "message": "Validation failed",
  "errors": ["'Full Name' must not be empty.", "'Phone' is not a valid phone number."],
  "data": null
}
```

### Unauthorized (401)

```json
{
  "succeeded": false,
  "message": "Unauthorized",
  "errors": null,
  "data": null
}
```

### Not Found (404)

```json
{
  "succeeded": false,
  "message": "Member with ID 99 not found.",
  "errors": null,
  "data": null
}
```

### Server Error (500)

```json
{
  "succeeded": false,
  "message": "An error occurred while processing your request.",
  "errors": null,
  "data": null
}
```

---

## HTTP Status Codes

| Code | Meaning |
|---|---|
| `200` | Success |
| `201` | Created |
| `400` | Bad request / validation error |
| `401` | Unauthorized (missing or invalid token) |
| `403` | Forbidden (insufficient permissions) |
| `404` | Resource not found |
| `500` | Internal server error |
