# PokeLike API

Welcome to the **PokeLike API**! This project helps you track Pokémon you find cute 🥰 using an ASP.NET Core Web API backed by a PostgreSQL database.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Entity Framework Migrations](#entity-framework-migrations)
- [API Endpoints](#api-endpoints)

## Getting Started

### Prerequisites

- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [psql (PostgreSQL Command Line Tool)](https://www.postgresql.org/docs/current/app-psql.html) (Optional but recommended)
- [pgAdmin](https://www.pgadmin.org/) or any PostgreSQL GUI (Optional)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/fac30/PRO05_BACK_Gaj_Khalos_Max.git
   cd PRO05_BACK_Gaj_Khalos_Max
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Database Setup

1. **Configure Connection String**:

   - Update the `appsettings.json` file with your PostgreSQL connection string:
     ```json
     "ConnectionStrings": {
       "PokeLikeDbContext": "Host=localhost;Port=5432;Database=PokeLikeDb;Username=postgres;Password=your_password"
     }
     ```

2. **Create the Database** (if not already created):

   - Using `psql`:
     ```bash
     psql -U postgres -c "CREATE DATABASE PokeLikeDb;"
     ```
   - Or using pgAdmin or another GUI.

3. **Apply Migrations**:
   - Run the following command to apply migrations and set up the database schema:
     ```bash
     dotnet ef database update
     ```

## Running the Application

To start the application, use the following command:

```bash
dotnet run
```

The API will be available at `http://localhost:5050` by default (or another port specified in your launch configuration).

## Project Structure

- `Program.cs`: Configures services and middleware for the application.
- `Data/PokeLikeDbContext.cs`: EF Core `DbContext` configuration for managing the database context.
- `Models/`: Contains entity models such as `Pokemon` and `Collection`.
- `Migrations/`: Contains EF Core migration files.

## Configuration

- **Naming Conventions**:
  The project uses the `EFCore.NamingConventions` plugin for consistent naming. By default, `snake_case` is applied to all table and column names.

  ```csharp
  builder.Services.AddDbContextPool<PokeLikeDbContext>(opt =>
      opt.UseNpgsql(builder.Configuration.GetConnectionString("PokeLikeDbContext"))
         .UseSnakeCaseNamingConvention());
  ```

## Entity Framework Migrations

### Adding a New Migration

To add a new migration after modifying models, run:

```bash
dotnet ef migrations add <MigrationName>
```

### Removing the Last Migration

If you need to undo the last migration:

```bash
dotnet ef migrations remove
```

### Applying Migrations

Ensure the latest migrations are applied to the database:

```bash
dotnet ef database update
```

## API Endpoints

### Base URL

The base URL for the API is `http://localhost:5050`.

### Endpoints

- **GET /pokemon**: Retrieves a list of Pokémon.
- **POST /pokemon**: Adds a new Pokémon.
  - **Body**:
    ```json
    {
      "name": "Pikachu",
      "apiUrl": "https://pokeapi.co/api/v2/pokemon/pikachu",
      "likes": 100
    }
    ```

## Troubleshooting

- **`relation "Collections" already exists` error**:
  - Ensure your database schema is in sync with your migrations. If needed, drop the existing table or reset migrations.
  - Use `psql` or pgAdmin to inspect and manage your database.
