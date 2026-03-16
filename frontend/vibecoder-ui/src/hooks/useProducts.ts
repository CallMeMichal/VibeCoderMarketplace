import { useState, useCallback } from 'react';
import { CleanedProduct, ExportFormat } from '../types/product';
import * as productsApi from '../api/productsApi';

interface UseProductsReturn {
  products: CleanedProduct[];
  loading: boolean;
  error: string | null;
  upload: (file: File) => Promise<void>;
  exportData: (format: ExportFormat) => Promise<void>;
}

export const useProducts = (): UseProductsReturn => {
  const [products, setProducts] = useState<CleanedProduct[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const upload = useCallback(async (file: File) => {
    setLoading(true);
    setError(null);
    try {
      const result = await productsApi.uploadProducts(file);
      setProducts(result);
    } catch (err: any) {
      setError(err.response?.data ?? 'Upload failed. Make sure the backend is running.');
    } finally {
      setLoading(false);
    }
  }, []);

  const exportData = useCallback(async (format: ExportFormat) => {
    setError(null);
    try {
      await productsApi.exportProducts(format);
    } catch (err: any) {
      setError(err.response?.data ?? 'Export failed.');
    }
  }, []);

  return { products, loading, error, upload, exportData };
};
