import React from 'react';
import './App.css';
import FileUpload from './components/FileUpload/FileUpload';
import ProductTable from './components/ProductTable/ProductTable';
import ExportButton from './components/ExportButton/ExportButton';
import { useProducts } from './hooks/useProducts';

function App() {
  const { products, loading, error, upload, exportData } = useProducts();

  return (
    <div className="app">
      <nav className="sidebar">
        <div className="sidebar-brand">
          <span className="brand-mark">V</span>
          <div className="brand-text">
            <span className="brand-name">VibeCoder</span>
            <span className="brand-version">v1.0</span>
          </div>
        </div>
        <div className="sidebar-section">
          <span className="sidebar-label">Tools</span>
          <div className="sidebar-item active">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none"><path d="M2 3h12M2 8h12M2 13h8" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/></svg>
            Data Cleaner
          </div>
        </div>
      </nav>

      <div className="main-area">
        <header className="topbar">
          <div className="topbar-left">
            <h1 className="page-title">Product Data Cleaner</h1>
            <span className="page-desc">Process and clean marketplace exports</span>
          </div>
          <div className="topbar-actions">
            <FileUpload onUpload={upload} loading={loading} />
            <ExportButton onExport={exportData} disabled={products.length === 0} />
          </div>
        </header>

        {error && (
          <div className="error-banner">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none"><circle cx="8" cy="8" r="7" stroke="#c53030" strokeWidth="1.5"/><path d="M8 4.5v4M8 10.5v.5" stroke="#c53030" strokeWidth="1.5" strokeLinecap="round"/></svg>
            {error}
          </div>
        )}

        <section className="content-card">
          <ProductTable products={products} />
        </section>
      </div>
    </div>
  );
}

export default App;
