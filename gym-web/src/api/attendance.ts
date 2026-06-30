import apiClient from './axios';
import type { ApiResponse, PaginatedResult, AttendanceDto, PaginationParams } from '../types';

export const attendanceApi = {
  getAll: (params?: PaginationParams & { dateFrom?: string; dateTo?: string; memberId?: string }) =>
    apiClient.get<ApiResponse<PaginatedResult<AttendanceDto>>>('/attendances', { params }),

  getToday: () =>
    apiClient.get<ApiResponse<AttendanceDto[]>>('/attendances/today'),

  checkIn: (data: { memberId: string; deviceId?: string; isManual?: boolean }) =>
    apiClient.post<ApiResponse<string>>('/attendances/check-in', data),

  checkOut: (data: { id: string }) =>
    apiClient.post<ApiResponse<null>>('/attendances/check-out', data),

  getByMember: (memberId: string, params?: PaginationParams) =>
    apiClient.get<ApiResponse<PaginatedResult<AttendanceDto>>>(`/attendances/by-member/${memberId}`, { params }),
};
