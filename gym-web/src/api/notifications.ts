import apiClient from './axios';
import type { ApiResponse, PaginatedResult, NotificationDto, PaginationParams } from '../types';

export const notificationsApi = {
  getAll: (params?: PaginationParams & { memberId?: string; status?: string }) =>
    apiClient.get<ApiResponse<PaginatedResult<NotificationDto>>>('/notifications', { params }),

  send: (data: { memberId: string; type: string; channel: string; message: string; subject?: string }) =>
    apiClient.post<ApiResponse<string>>('/notifications', data),

  resend: (id: string) =>
    apiClient.post<ApiResponse<NotificationDto>>(`/notifications/${id}/resend`),
};
