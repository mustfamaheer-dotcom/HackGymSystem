import apiClient from './axios';
import type { ApiResponse, SettingDto } from '../types';

export const settingsApi = {
  getAll: (group?: string) =>
    apiClient.get<ApiResponse<SettingDto[]>>('/settings', { params: { group } }),

  update: (id: string, value: string) =>
    apiClient.put<ApiResponse<string>>(`/settings/${id}`, { value }),
};
