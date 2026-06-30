import apiClient from './axios';
import type { ApiResponse, LoginRequest, LoginResponse, UserDto } from '../types';

export const authApi = {
  login: (data: LoginRequest) =>
    apiClient.post<ApiResponse<LoginResponse>>('/auth/login', data),

  refresh: (refreshToken: string) =>
    apiClient.post<ApiResponse<LoginResponse>>('/auth/refresh', { refreshToken }),

  me: () =>
    apiClient.get<ApiResponse<UserDto>>('/auth/me'),

  logout: () =>
    apiClient.post<ApiResponse<null>>('/auth/logout'),
};
