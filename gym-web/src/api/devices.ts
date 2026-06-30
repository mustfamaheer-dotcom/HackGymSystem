import apiClient from './axios';
import type { ApiResponse, DeviceDto, DeviceLogDto } from '../types';

export const devicesApi = {
  getAll: (params?: any) =>
    apiClient.get<ApiResponse<any>>('/devices', { params }),

  getActive: () =>
    apiClient.get<ApiResponse<DeviceDto[]>>('/devices/active'),

  getById: (id: string) =>
    apiClient.get<ApiResponse<DeviceDto>>(`/devices/${id}`),

  testConnection: (id: string) =>
    apiClient.post<ApiResponse<{ success: boolean; message: string }>>(`/devices/${id}/test-connection`),

  create: (data: Partial<DeviceDto>) =>
    apiClient.post<ApiResponse<string>>('/devices', data),

  update: (id: string, data: any) =>
    apiClient.put<ApiResponse<any>>(`/devices/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/devices/${id}`),

  getLogs: (deviceId: string) =>
    apiClient.get<ApiResponse<DeviceLogDto[]>>(`/devices/${deviceId}/logs`),

  sync: (id: string) =>
    apiClient.post<ApiResponse<null>>(`/devices/${id}/sync`),
};
