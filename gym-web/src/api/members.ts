import apiClient from './axios';
import type { ApiResponse, PaginatedResult, MemberDto, MemberFormData, PaginationParams } from '../types';

export const membersApi = {
  getAll: (params?: PaginationParams) =>
    apiClient.get<ApiResponse<PaginatedResult<MemberDto>>>('/members', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<MemberDto>>(`/members/${id}`),

  create: (data: MemberFormData) =>
    apiClient.post<ApiResponse<MemberDto>>('/members', data),

  update: (id: string, data: MemberFormData) =>
    apiClient.put<ApiResponse<MemberDto>>(`/members/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/members/${id}`),
};
