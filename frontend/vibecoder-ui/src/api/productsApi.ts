import axios from 'axios';
import { CleanedProduct, ExportFormat } from '../types/product';

const API_BASE = 'http://localhost:5038/api';

const api = axios.create({
  baseURL: API_BASE,
});

export const uploadProducts = async (file: File): Promise<CleanedProduct[]> => {
  const formData = new FormData();
  formData.append('file', file);
  const response = await api.post<CleanedProduct[]>('/products/upload', formData);
  return response.data;
};

export const getProducts = async (): Promise<CleanedProduct[]> => {
  const response = await api.get<CleanedProduct[]>('/products');
  return response.data;
};

export const exportProducts = async (format: ExportFormat): Promise<void> => {
  const response = await api.get(`/products/export?format=${format}`, {
    responseType: 'blob',
  });

  const ext = format === 'xlsx' ? 'xlsx' : 'csv';
  const mimeType = format === 'xlsx'
    ? 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    : 'text/csv';

  const blob = new Blob([response.data], { type: mimeType });
  const url = window.URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = `products.${ext}`;
  document.body.appendChild(link);
  link.click();
  link.remove();
  window.URL.revokeObjectURL(url);
};
