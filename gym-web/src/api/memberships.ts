import apiClient from './axios';
import type { ApiResponse, PaginatedResult, MembershipDto, PaginationParams } from '../types';

export const membershipsApi = {
  getAll: (params?: PaginationParams & { memberId?: string; status?: string }) =>
    apiClient.get<ApiResponse<PaginatedResult<MembershipDto>>>('/memberships', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<MembershipDto>>(`/memberships/${id}`),

  getByMember: (memberId: string) =>
    apiClient.get<ApiResponse<MembershipDto[]>>(`/memberships/by-member/${memberId}`),

  create: (data: { memberId: string; planId: string; startDate: string; notes?: string }) =>
    apiClient.post<ApiResponse<string>>('/memberships', data),

  renew: (id: string, data: { planId: string; startDate: string }) =>
    apiClient.post<ApiResponse<MembershipDto>>(`/memberships/${id}/renew`, data),

  cancel: (id: string) =>
    apiClient.post<ApiResponse<null>>(`/memberships/${id}/cancel`),

  freeze: (id: string, data: { days: number }) =>
    apiClient.post<ApiResponse<null>>(`/memberships/${id}/freeze`, data),

  unfreeze: (id: string) =>
    apiClient.post<ApiResponse<null>>(`/memberships/${id}/unfreeze`),
};
