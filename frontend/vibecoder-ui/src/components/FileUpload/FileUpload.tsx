import React, { useRef } from 'react';
import './FileUpload.css';

interface FileUploadProps {
  onUpload: (file: File) => void;
  loading: boolean;
}

const FileUpload: React.FC<FileUploadProps> = ({ onUpload, loading }) => {
  const inputRef = useRef<HTMLInputElement>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      onUpload(file);
      e.target.value = '';
    }
  };

  return (
    <div className="file-upload">
      <input
        ref={inputRef}
        type="file"
        accept=".json"
        onChange={handleChange}
        id="file-input"
        hidden
      />
      <button
        className="upload-btn"
        onClick={() => inputRef.current?.click()}
        disabled={loading}
      >
        {loading ? 'Processing...' : 'Upload JSON File'}
      </button>
    </div>
  );
};

export default FileUpload;
