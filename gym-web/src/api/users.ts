import apiClient from './axios';
import type { ApiResponse, PaginatedResult, UserListItemDto, UserFormData, RoleDto } from '../types';

export const usersApi = {
  getAll: (params?: { page?: number; pageSize?: number; searchTerm?: string }) =>
    apiClient.get<ApiResponse<PaginatedResult<UserListItemDto>>>('/users', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<UserListItemDto>>(`/users/${id}`),

  getRoles: () =>
    apiClient.get<ApiResponse<RoleDto[]>>('/users/roles'),

  create: (data: UserFormData) =>
    apiClient.post<ApiResponse<string>>('/users', data),

  update: (id: string, data: { fullName: string; email: string; phone?: string; roleId: string; isActive: boolean }) =>
    apiClient.put<ApiResponse<string>>(`/users/${id}`, { id, ...data }),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/users/${id}`),
};
