import apiClient from './axios';
import type { ApiResponse, PaginatedResult, OfferDto, OfferFormData } from '../types';

export const offersApi = {
  getAll: (params?: { page?: number; pageSize?: number }) =>
    apiClient.get<ApiResponse<PaginatedResult<OfferDto>>>('/offers', { params }),

  getById: (id: string) =>
    apiClient.get<ApiResponse<OfferDto>>(`/offers/${id}`),

  create: (data: OfferFormData) =>
    apiClient.post<ApiResponse<string>>('/offers', data),

  update: (id: string, data: OfferFormData) =>
    apiClient.put<ApiResponse<OfferDto>>(`/offers/${id}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/offers/${id}`),
};
