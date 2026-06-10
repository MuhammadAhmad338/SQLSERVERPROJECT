# SQLSERVERPROJECT — ASP.NET Core REST API

A RESTful Web API built with **ASP.NET Core (.NET 10)** and **Entity Framework Core**, backed by a **PostgreSQL** database. The project follows a clean, layered architecture with auto-generated API documentation via Swagger.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 10) |
| ORM | Entity Framework Core |
| Database | PostgreSQL (via Npgsql) |
| API Docs | Swagger / Swashbuckle |
| Language | C# |

---

## Project Structure

```
SQLSERVERPROJECT/
├── Controllers/        # API controllers (route handlers)
├── DBContext/          # EF Core DbContext (AppDbContext)
├── Migrations/         # EF Core database migrations
├── Models/             # Entity / data models
├── Properties/         # Launch settings
├── Program.cs          # App entry point & service registration
├── appsettings.json    # App configuration (connection strings, etc.)
└── myapi.csproj        # Project file
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL running locally or remotely
- `dotnet-ef` CLI tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 1. Clone the repo

```bash
git clone https://github.com/MuhammadAhmad338/SQLSERVERPROJECT.git
cd SQLSERVERPROJECT
```

### 2. Configure the database

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=yourpassword"
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the project

```bash
dotnet run
```

The API will start on `https://localhost:5001` (or the port shown in your terminal).

---

## API Documentation

Swagger UI is available in development mode at:

```
https://localhost:<port>/swagger
```

It lists all available endpoints with request/response schemas and lets you test them directly from the browser.

---

## Key Configuration (Program.cs)

- **EF Core** is registered with the `AppDbContext` using the `DefaultConnection` string from `appsettings.json`.
- **JSON circular reference handling** is configured via `ReferenceHandler.IgnoreCycles` to safely serialize related entities.
- **Default route** pattern: `api/{controller}/{action}/{id?}`

---

## Running Migrations (reference)

```bash
# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update

# Revert last migration
dotnet ef migrations remove
```

---

## License

This project is open source. Feel free to fork and build on it.
