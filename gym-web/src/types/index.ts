export interface ApiResponse<T> {
  success: boolean;
  message: string | null;
  errors: string[] | null;
  data: T;
}

export interface PaginatedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface UserDto {
  id: string;
  username: string;
  email: string;
  fullName: string;
  role: string;
  isActive: boolean;
}

export interface MemberDto {
  id: string;
  code: string;
  fullName: string;
  phone: string;
  email?: string;
  gender?: string;
  dateOfBirth?: string;
  address?: string;
  photoPath?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  joinDate: string;
  status: string;
  notes?: string;
  fingerprintId?: number;
  faceId?: number;
  deviceUserId?: number;
  isActive: boolean;
  createdAt: string;
}

export interface MembershipPlanDto {
  id: string;
  name: string;
  description?: string;
  durationDays: number;
  price: number;
  maxVisits?: number;
  freezeDays?: number;
  isActive: boolean;
}

export interface MembershipDto {
  id: string;
  memberId: string;
  memberName: string;
  planId: string;
  planName: string;
  startDate: string;
  endDate: string;
  status: string;
  remainingVisits?: number;
  frozenDays?: number;
  notes?: string;
}

export interface AttendanceDto {
  id: string;
  memberId: string;
  memberName: string;
  memberNumber: string;
  checkIn: string;
  checkOut?: string;
  isManual: boolean;
  deviceName?: string;
  notes?: string;
}

export interface PaymentDto {
  id: string;
  memberId: string;
  memberName: string;
  amount: number;
  discountAmount: number;
  totalAmount: number;
  paymentDate: string;
  paymentMethod: string;
  referenceNumber?: string;
  employeeName?: string;
  notes?: string;
  receiptNumber: string;
  createdAt: string;
}

export interface OfferDto {
  id: string;
  title: string;
  description?: string;
  discountType: string;
  discountValue: number;
  startDate: string;
  endDate: string;
  isActive: boolean;
  createdAt: string;
}

export interface DeviceDto {
  id: string;
  name: string;
  serialNumber: string;
  deviceType: string;
  ipAddress?: string;
  port?: number;
  location?: string;
  isActive: boolean;
  lastSync?: string;
  notes?: string;
}

export interface DeviceLogDto {
  id: string;
  deviceId: string;
  deviceName: string;
  logType: string;
  message: string;
  timestamp: string;
}

export interface NotificationDto {
  id: string;
  memberId?: string;
  memberName?: string;
  type: string;
  channel: string;
  subject?: string;
  message: string;
  status: string;
  sentAt?: string;
  error?: string;
}

export interface SettingDto {
  id: string;
  key: string;
  value: string;
  group: string;
  description?: string;
}

export interface DashboardStats {
  totalMembers: number;
  activeMembers: number;
  activeMemberships: number;
  todayCheckIns: number;
  todayRevenue: number;
  monthlyRevenue: number;
  expiringMemberships: number;
  overduePayments: number;
  recentActivities: ActivityDto[];
  membershipByPlan: ChartDataPoint[];
}

export interface ActivityDto {
  id: string;
  type: string;
  description: string;
  timestamp: string;
  userId?: string;
  userName?: string;
}

export interface ChartDataPoint {
  label: string;
  value: number;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
  searchTerm?: string;
  sortBy?: string;
  sortDescending?: boolean;
}

export interface MemberFormData {
  fullName: string;
  phone: string;
  email?: string;
  gender?: string;
  dateOfBirth?: string;
  address?: string;
  notes?: string;
}

export interface PlanFormData {
  id?: string;
  name: string;
  description?: string;
  durationDays: number;
  price: number;
  maxVisits?: number;
  freezeDays?: number;
}

export interface PaymentFormData {
  memberId: string;
  amount: number;
  amountPaid: number;
  paymentDate: string;
  method: string;
  reference?: string;
  notes?: string;
}

export interface OfferFormData {
  title: string;
  description?: string;
  discountType: string;
  discountValue: number;
  startDate: string;
  endDate: string;
}

export interface UserListItemDto {
  id: string;
  username: string;
  fullName: string;
  email: string;
  phone?: string;
  roleName: string;
  roleId: string;
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
}

export interface UserFormData {
  username: string;
  password: string;
  fullName: string;
  email: string;
  phone?: string;
  roleId: string;
}

export interface RoleDto {
  id: string;
  name: string;
}
