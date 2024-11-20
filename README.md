# PokeLike API

![Pikachu welcomes you!](https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/dream-world/25 "Pikachu")

Welcome to the **PokeLike API**! This project helps you track Pokémon you find cute 🥰 using an ASP.NET Core Web API backed by a PostgreSQL database.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
  - [Using Docker](#using-docker)
  - [Running Locally](#running-locally)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Entity Framework Migrations](#entity-framework-migrations)
- [API Endpoints](#api-endpoints)

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (Included with Docker Desktop)
- (Optional for local development):
  - [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download)
  - [PostgreSQL](https://www.postgresql.org/download/)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/fac30/PRO05_BACK_Gaj_Khalos_Max.git
   cd PRO05_BACK_Gaj_Khalos_Max
   ```

---

## Database Setup

### Using Docker

When using Docker, the PostgreSQL database is automatically set up in a container. No additional manual configuration is required.

1. Pull the Docker image from Docker Hub:

   ```bash
   docker-compose pull
   ```

1. Build and start the Docker containers:

   ```bash
   docker-compose up -d
   ```

1. The API will be available at `http://localhost/`.

1. To stop the containers, run:

   ```bash
   docker-compose down
   ```

1. The API will be available at `http://localhost/` - no port number is needed.

### Running Locally

To start the application locally, follow these steps:

1. Modify ConnectionString in appsettings.json to be hosted on localhost rather than db Docker container:

   ```json
   "ConnectionStrings": {
    "PokeLikeDbContext": "Host=localhost;Port=5432;Database=PokeLikeDb;Username=postgres;Password=password"
   }
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

---

## Project Structure

- `Program.cs`: Configures services and middleware for the application.
- `Data/PokeLikeDbContext.cs`: EF Core `DbContext` configuration for managing the database context.
- `Models/`: Contains entity models such as `Pokemon` and `Collection`.
- `Migrations/`: Contains EF Core migration files.

---

## Configuration

- **Naming Conventions**:
  The project uses the `EFCore.NamingConventions` plugin for consistent naming. By default, `snake_case` is applied to all table and column names.

  ```csharp
  builder.Services.AddDbContextPool<PokeLikeDbContext>(opt =>
      opt.UseNpgsql(builder.Configuration.GetConnectionString("PokeLikeDbContext"))
         .UseSnakeCaseNamingConvention());
  ```

---

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

---

## API Endpoints

### Base URL

- **Docker**: `http://localhost/`
- **Local Development**: `http://localhost:5050`

### Endpoints

- **GET /pokemon**: Retrieves a list of Pokémon.
- **POST /pokemon**: Adds a new Pokémon.
  - **Body**:
    ```json
    {
      "name": "Pikachu",
      "imageUrl": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/dream-world/25",
      "apiUrl": "https://pokeapi.co/api/v2/pokemon/pikachu",
      "likes": 100
    }
    ```

---

## Troubleshooting

### Docker Issues

- **Port conflicts**:
  Ensure no other services are running on the ports specified in `docker-compose.yml`.

- **Database connection issues**:
  If the application cannot connect to the database, verify that the database container is running:

  ```bash
  docker ps
  ```

- **Resetting Docker containers**:
  To rebuild and reset all containers:

  ```bash
  docker-compose down -v
  docker system prune -a
  docker-compose up --build
  ```

- **Resetting migration**
  If there have been updates to the codebase, remove migrations and re-initialise them before re-running container commands:

  ```bash
  dotnet ef migrations remove
  dotnet ef migrations add InitialCreate
  ```

- **Pulling Docker image from Docker Hub**
  If you have trouble authenticating and pulling the image from Docker Hub, you may need to login with Docker:

  ```bash
  docker login
  ```
