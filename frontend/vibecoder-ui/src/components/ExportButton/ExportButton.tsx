import React from 'react';
import { ExportFormat } from '../../types/product';
import './ExportButton.css';

interface ExportButtonProps {
  onExport: (format: ExportFormat) => void;
  disabled: boolean;
}

const ExportButton: React.FC<ExportButtonProps> = ({ onExport, disabled }) => {
  return (
    <div className="export-group">
      <button
        className="export-btn export-xlsx"
        onClick={() => onExport('xlsx')}
        disabled={disabled}
      >
        Export XLSX
      </button>
      <button
        className="export-btn export-csv"
        onClick={() => onExport('csv')}
        disabled={disabled}
      >
        Export CSV
      </button>
    </div>
  );
};

export default ExportButton;
