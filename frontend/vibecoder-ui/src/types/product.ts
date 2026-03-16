export interface CleanedProduct {
  sku: string;
  originalName: string;
  allegroTitle: string;
  cleanDescription: string;
  dimensions: string;
  color: string;
  price: number | null;
  stock: number | null;
  ean: string | null;
}

export type ExportFormat = 'xlsx' | 'csv';
