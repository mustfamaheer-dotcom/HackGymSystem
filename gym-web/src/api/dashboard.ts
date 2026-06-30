import apiClient from './axios';
import type { ApiResponse, DashboardStats } from '../types';

export const dashboardApi = {
  getStats: () =>
    apiClient.get<ApiResponse<DashboardStats>>('/dashboard'),
};
