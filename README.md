# DISCLAIMER: Readme file and css styles are Copilot AI enhanced*
# 📦 Product Management System

A CRUD web application built with **ASP.NET Core 8**, **SQL Server Stored Procedures**, and a **public API** integration. The project follows a layered architecture using separate class libraries for Domain, Data, ServicesConsumption, and Web.

---

## 🎯 Features

- ✅ Full **CRUD** for Products (Create, Read, Update, Delete)
- ✅ All data access through **SQL Server Stored Procedures** (ADO.NET, no ORM)
- ✅ **REST API** (`/api/products`) with Swagger/OpenAPI documentation
- ✅ **MVC Web UI** (`/Products`) with Razor views
- ✅ **Public API consumption** — live USD → COP exchange rate from [open.er-api.com](https://open.er-api.com/)
- ✅ Clean **layered architecture** (Domain / Data / Services / Web)
- ✅ **Dependency Injection** throughout
- ✅ `async/await` everywhere for scalability

---

## 🏛️ Architecture

```
┌─────────────────────────────────────────────────┐
│          ProductManagement.Web                  │
│   (ASP.NET Core MVC + Web API + Views)          │
└────────────┬───────────────────────┬────────────┘
             │                       │
             ▼                       ▼
   ┌───────────────────┐   ┌───────────────────┐
   │  .Services        │   │  .Data            │
   │  (HttpClient,     │   │  (Repositories,   │
   │   external API)   │   │   ADO.NET + SPs)  │
   └─────────┬─────────┘   └─────────┬─────────┘
             │                       │
             └──────────┬────────────┘
                        ▼
              ┌───────────────────┐
              │    .Domain        │
              │  (Entities, DTOs) │
              └───────────────────┘
```

**Dependency rule:** outer layers depend on inner layers, never the reverse.
The `Domain` project has **zero dependencies** — it's the core of the system.

| Project                       | Type          | Responsibility                                           |
| ----------------------------- | ------------- | -------------------------------------------------------- |
| `ProductManagement.Domain`    | Class Library | Entities (`Product`) and DTOs (`ExchangeRateDto`)         |
| `ProductManagement.Data`      | Class Library | Repository pattern, ADO.NET, calls SQL Server procedures |
| `ProductManagement.Services`  | Class Library | Business logic, external API consumption via HttpClient  |
| `ProductManagement.Web`       | ASP.NET Core  | Controllers (REST + MVC), Razor Views, entry point       |

---

## 🛠️ Tech Stack

- **.NET 8** (ASP.NET Core MVC + Web API)
- **C# 12**
- **SQL Server** (2019 / 2022 / LocalDB / Express — any edition works)
- **ADO.NET** (`Microsoft.Data.SqlClient`) — no Entity Framework, for StoredProcedures functionality purpose
- **Swashbuckle** for Swagger/OpenAPI UI
- **Razor Views** for the web UI

---

## 📋 Prerequisites

Install these before running the project:

1. **[.NET 8 SDK](https://dotnet.microsoft.com/download)**
   Verify with:
```bash
   dotnet --version
```
   Should return `8.x.x` or higher.

2. **SQL Server** — any of:
   - SQL Server Express (free)
   - SQL Server Developer Edition (free)
   - SQL Server LocalDB (comes with Visual Studio)
   - Azure SQL / Docker SQL Server

3. **SQL client** to run the script — one of:
   - [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)
   - [Azure Data Studio](https://azure.microsoft.com/products/data-studio/)
   - VS Code with the *SQL Server (mssql)* extension

4. **Git** (to clone the repository)

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/ProductManagement.git
cd ProductManagement
```

### 2. Create the database and stored procedures

Open **`Database/DatabaseScript.sql`** in SSMS / Azure Data Studio / VS Code, connect to your SQL Server instance, and run the entire script.

This will:
- Create the `ProductManagementDB` database
- Create the `Products` table
- Create 5 stored procedures:
  `sp_Product_Create`, `sp_Product_GetAll`, `sp_Product_GetById`, `sp_Product_Update`, `sp_Product_Delete`
- Seed three sample products

### 3. Update the connection string

Open **`ProductManagement.Web/appsettings.json`** and edit the connection string to match your SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Common connection string variations:**

| Scenario                          | Connection string                                                                                                                    |
| --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| Local SQL Server (Windows Auth)   | `Server=localhost;Database=ProductManagementDB;Trusted_Connection=True;TrustServerCertificate=True;`                                 |
| SQL Server Express                | `Server=localhost\SQLEXPRESS;Database=ProductManagementDB;Trusted_Connection=True;TrustServerCertificate=True;`                       |
| LocalDB                           | `Server=(localdb)\MSSQLLocalDB;Database=ProductManagementDB;Trusted_Connection=True;TrustServerCertificate=True;`                     |
| SQL Authentication (user/pwd)     | `Server=localhost;Database=ProductManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;`                         |
| Docker (port 1433)                | `Server=localhost,1433;Database=ProductManagementDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;`             |

### 4. Restore, build, and run

From the solution root:

```bash
dotnet restore
dotnet build
dotnet run --project ProductManagement.Web
```

You should see output like:

```
Now listening on: https://localhost:7xxx
Now listening on: http://localhost:5xxx
Application started. Press Ctrl+C to shut down.
```

### 5. Open the app

| URL                                    | What it shows                            |
| -------------------------------------- | ---------------------------------------- |
| `https://localhost:7xxx/`              | Landing page                             |
| `https://localhost:7xxx/Products`      | Product list with live USD → COP rate    |
| `https://localhost:7xxx/swagger`       | Interactive REST API documentation       |

> The exact port number is shown in the terminal when the app starts.

---

## 📡 REST API Endpoints

Base URL: `https://localhost:7xxx/api`

### Products

| Method | Route                 | Description                 | Body                                          |
| ------ | --------------------- | --------------------------- | --------------------------------------------- |
| GET    | `/api/products`       | List all products           | —                                             |
| GET    | `/api/products/{id}`  | Get a product by id         | —                                             |
| POST   | `/api/products`       | Create a new product        | `{ "name": "...", "description": "...", "price": 0.0 }` |
| PUT    | `/api/products/{id}`  | Update an existing product  | `{ "name": "...", "description": "...", "price": 0.0 }` |
| DELETE | `/api/products/{id}`  | Delete a product            | —                                             |

### Exchange Rate

| Method | Route                          | Description                        |
| ------ | ------------------------------ | ---------------------------------- |
| GET    | `/api/exchangerate/{currency}` | Live USD → {currency} rate         |

**Example:** `GET /api/exchangerate/COP` →
```json
{
  "baseCurrency": "USD",
  "targetCurrency": "COP",
  "rate": 4012.35,
  "fetchedAt": "2026-04-17T14:32:11Z"
}
```

### Example cURL requests

```bash
# List all products
curl https://localhost:7xxx/api/products -k

# Create a product
curl -X POST https://localhost:7xxx/api/products -k \
  -H "Content-Type: application/json" \
  -d '{"name":"Headphones","description":"Over-ear, noise cancelling","price":199.99}'

# Update a product
curl -X PUT https://localhost:7xxx/api/products/1 -k \
  -H "Content-Type: application/json" \
  -d '{"name":"Headphones Pro","description":"Updated","price":249.99}'

# Delete a product
curl -X DELETE https://localhost:7xxx/api/products/1 -k
```

> The `-k` flag skips HTTPS certificate validation for the local dev cert. For Windows PowerShell, use `Invoke-RestMethod` instead.

---

## 🗂️ Project Structure

```
ProductManagement/
├── ProductManagement.sln
├── README.md
├── Database/
│   └── DatabaseScript.sql              ← Creates DB, table, and stored procedures
│
├── ProductManagement.Domain/           ← Entities (zero dependencies)
│   ├── Entities/
│   │   └── Product.cs
│   └── DTOs/
│       └── ExchangeRateDto.cs
│
├── ProductManagement.Data/             ← Data access via ADO.NET + stored procedures
│   ├── DependencyInjection.cs
│   └── Repositories/
│       ├── IProductRepository.cs
│       └── ProductRepository.cs
│
├── ProductManagement.Services/         ← External API consumption
│   ├── DependencyInjection.cs
│   ├── IExchangeRateService.cs
│   └── ExchangeRateService.cs
│
└── ProductManagement.Web/              ← ASP.NET Core app (MVC + Web API)
    ├── Program.cs
    ├── appsettings.json
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── ProductsController.cs          ← MVC (HTML pages)
    │   ├── ProductsApiController.cs       ← REST API
    │   └── ExchangeRateController.cs      ← REST API
    ├── Views/
    │   ├── _ViewStart.cshtml
    │   ├── _ViewImports.cshtml
    │   ├── Shared/_Layout.cshtml
    │   ├── Home/Index.cshtml
    │   └── Products/
    │       ├── Index.cshtml
    │       ├── Create.cshtml
    │       └── Edit.cshtml
    └── wwwroot/
        └── css/site.css
```

---

## 🗄️ Stored Procedures

All CRUD operations go through these stored procedures (defined in `Database/DatabaseScript.sql`):

| Procedure              | Purpose                                      |
| ---------------------- | -------------------------------------------- |
| `sp_Product_Create`    | Inserts a product; returns the new row       |
| `sp_Product_GetAll`    | Returns every product, newest first          |
| `sp_Product_GetById`   | Returns a single product by id               |
| `sp_Product_Update`    | Updates Name / Description / Price by id     |
| `sp_Product_Delete`    | Deletes a product by id                      |

The `ProductRepository` class (in `ProductManagement.Data/Repositories/`) invokes each of these via `SqlCommand` with `CommandType.StoredProcedure` — **no raw SQL is sent from the application**.

---

## 🌐 Public API Used

**ExchangeRate-API (open access tier)** — [https://open.er-api.com/v6/latest/USD](https://open.er-api.com/v6/latest/USD)

- No API key required
- Returns the latest USD rate against ~160 currencies
- Real-world use case: on the `/Products` page, every product's USD price is displayed alongside its equivalent in Colombian Pesos (COP) using the live rate

---

## 🧪 Testing the App Manually

1. Open `/Products` — you should see the 3 seeded products and a banner showing the live USD → COP rate.
2. Click **+ New Product** — fill the form and submit.
3. Click **Edit** on any row — change a value and save.
4. Click **Delete** on any row — confirm the dialog.
5. Open `/swagger` — test every API endpoint interactively.

---

## 🆘 Troubleshooting

| Problem                                                                 | Solution                                                                                                                          |
| ----------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| `A network-related or instance-specific error occurred`                 | SQL Server isn't running or the connection string server name is wrong. Verify the server is up and the instance name is correct. |
| `Login failed for user`                                                 | SQL authentication credentials are wrong, or the user isn't mapped to the database. Use Windows auth (`Trusted_Connection=True`) if possible. |
| `The certificate chain was issued by an untrusted authority`            | Add `TrustServerCertificate=True;` to the connection string.                                                                      |
| `Could not find stored procedure 'sp_Product_GetAll'`                   | You didn't run the SQL script, or ran it against the wrong database. Run `Database/DatabaseScript.sql` in `ProductManagementDB`.   |
| `Could not load live exchange rate` banner appears                      | No internet access or the public API is temporarily down. CRUD still works; only the COP column will show `—`.                    |
| `HTTPS certificate is not trusted` in browser                           | Run `dotnet dev-certs https --trust` once.                                                                                        |
| Port already in use                                                     | Another app is using ports 5000/5001/7xxx. Edit `Properties/launchSettings.json` in the Web project or stop the other app.        |


---

## 👤 Author

Built as a technical test deliverable.
