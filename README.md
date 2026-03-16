# VibeCoder 1.0 — Product Data Cleaner

Cleans dirty marketplace product exports and presents them in a dashboard with Excel/CSV export.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js v18+](https://nodejs.org/)

## How to Run

### Backend

```bash
cd backend/VibeCoder.Api
dotnet restore
dotnet run
```

Runs at **http://localhost:5038**

### Frontend (new terminal)

```bash
cd frontend/vibecoder-ui
npm install
npm start
```

Opens at **http://localhost:3000**

## Usage

1. Open **http://localhost:3000**
2. Click **Upload JSON File** → select `partner_export_dirty.json`
3. View cleaned data in the table
4. Click **Export XLSX** or **Export CSV** to download
