import React, { useCallback, useRef } from 'react';
import { CleanedProduct } from '../../types/product';
import './ProductTable.css';

interface ProductTableProps {
  products: CleanedProduct[];
}

const COLUMNS = [
  { key: 'sku', label: 'SKU', width: 100 },
  { key: 'allegroTitle', label: 'Allegro Title', width: 280 },
  { key: 'cleanDescription', label: 'Description', width: 260 },
  { key: 'dimensions', label: 'Dimensions', width: 110 },
  { key: 'color', label: 'Color', width: 100 },
  { key: 'price', label: 'Price', width: 100 },
  { key: 'stock', label: 'Stock', width: 70 },
  { key: 'ean', label: 'EAN', width: 130 },
] as const;

const ProductTable: React.FC<ProductTableProps> = ({ products }) => {
  const tableRef = useRef<HTMLTableElement>(null);
  const dragState = useRef<{ colIndex: number; startX: number; startWidth: number } | null>(null);

  const onMouseDown = useCallback((e: React.MouseEvent, colIndex: number) => {
    e.preventDefault();
    const table = tableRef.current;
    if (!table) return;
    const th = table.querySelectorAll('thead th')[colIndex] as HTMLElement;
    dragState.current = { colIndex, startX: e.clientX, startWidth: th.offsetWidth };

    const onMouseMove = (ev: MouseEvent) => {
      if (!dragState.current) return;
      const diff = ev.clientX - dragState.current.startX;
      const newWidth = Math.max(50, dragState.current.startWidth + diff);
      const col = table.querySelector(`colgroup col:nth-child(${dragState.current.colIndex + 1})`) as HTMLElement;
      if (col) col.style.width = `${newWidth}px`;
    };

    const onMouseUp = () => {
      dragState.current = null;
      document.removeEventListener('mousemove', onMouseMove);
      document.removeEventListener('mouseup', onMouseUp);
      document.body.style.cursor = '';
      document.body.style.userSelect = '';
    };

    document.body.style.cursor = 'col-resize';
    document.body.style.userSelect = 'none';
    document.addEventListener('mousemove', onMouseMove);
    document.addEventListener('mouseup', onMouseUp);
  }, []);

  if (products.length === 0) {
    return (
      <div className="empty-state">
        <p className="empty-state-text">No products loaded yet</p>
        <p className="empty-state-hint">Upload a JSON export file to begin processing</p>
      </div>
    );
  }

  return (
    <div className="table-wrapper">
      <table className="product-table" ref={tableRef}>
        <colgroup>
          {COLUMNS.map((col) => (
            <col key={col.key} style={{ width: col.width }} />
          ))}
        </colgroup>
        <thead>
          <tr>
            {COLUMNS.map((col, i) => (
              <th key={col.key}>
                {col.label}
                {i < COLUMNS.length - 1 && (
                  <span
                    className="resize-handle"
                    onMouseDown={(e) => onMouseDown(e, i)}
                  />
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.sku}>
              <td className="cell-sku">{p.sku}</td>
              <td className="cell-title" title={p.allegroTitle}>{p.allegroTitle}</td>
              <td className="cell-desc" title={p.cleanDescription}>{p.cleanDescription}</td>
              <td>{p.dimensions}</td>
              <td><span className="color-badge">{p.color}</span></td>
              <td className="cell-price">
                {p.price != null ? `${p.price.toFixed(2)} PLN` : '—'}
              </td>
              <td className={`cell-stock${p.stock === 0 ? ' cell-stock-zero' : ''}`}>
                {p.stock != null ? p.stock : '—'}
              </td>
              <td className="cell-ean">{p.ean ?? '—'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProductTable;
