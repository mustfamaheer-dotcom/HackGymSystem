import apiClient from './axios';
import type { ApiResponse, PaginatedResult, PaymentDto, PaginationParams } from '../types';

export const paymentsApi = {
  getAll: (params?: PaginationParams & { dateFrom?: string; dateTo?: string; memberId?: string }) =>
    apiClient.get<ApiResponse<PaginatedResult<PaymentDto>>>('/payments', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<PaymentDto>>(`/payments/${id}`),

  create: (data: any) =>
    apiClient.post<ApiResponse<string>>('/payments', data),

  getByMember: (memberId: string) =>
    apiClient.get<ApiResponse<PaymentDto[]>>(`/payments/by-member/${memberId}`),
};
