import apiClient from './axios';
import type { ApiResponse, PaginatedResult, MembershipPlanDto, PlanFormData } from '../types';

export const plansApi = {
  getAll: (params?: any) =>
    apiClient.get<ApiResponse<PaginatedResult<MembershipPlanDto>>>('/plans', { params }),

  getActive: () =>
    apiClient.get<ApiResponse<MembershipPlanDto[]>>('/plans/active'),

  getById: (id: string) =>
    apiClient.get<ApiResponse<MembershipPlanDto>>(`/plans/${id}`),

  create: (data: PlanFormData) =>
    apiClient.post<ApiResponse<string>>('/plans', data),

  update: (id: string, data: PlanFormData) =>
    apiClient.put<ApiResponse<string>>(`/plans/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/plans/${id}`),

  toggleActive: (id: string, isActive: boolean) =>
    apiClient.patch<ApiResponse<string>>(`/plans/${id}/status`, { isActive }),
};
