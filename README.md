# QuestLog API

> **The Backend Engine for Your Epic RPG Life.**

QuestLog API is the powerful backend service for the QuestLog platform, a gamified task management system. It provides the logic for turning daily tasks into quests, calculating XP, managing user levels, and handling real-time progress.

Built with **ASP.NET Core** and following **Clean Architecture** principles, this API is designed for scalability, maintainability, and performance.

## 🚀 Tech Stack

-   **Framework:** [ASP.NET Core 8/9](https://dotnet.microsoft.com/) (Web API)
-   **Database:** [PostgreSQL](https://www.postgresql.org/)
-   **ORM:** [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (Code-First)
-   **Architecture:** Clean Architecture + CQRS (Command Query Responsibility Segregation)
-   **Messaging:** [MediatR](https://github.com/jbogard/MediatR)
-   **Validation:** [FluentValidation](https://fluentvalidation.net/)
-   **Mapping:** [AutoMapper](https://automapper.org/)
-   **Containerization:** [Docker](https://www.docker.com/) & Docker Compose
-   **Documentation:** [Swagger / OpenAPI](https://swagger.io/)

## 🏛️ Architecture

The solution is organized into four distinct layers to enforce separation of concerns:

-   **`QuestLog.Domain`**: The core of the application. Contains entities (User, Quest, Reward), value objects, and domain logic. No external dependencies.
-   **`QuestLog.Application`**: Contains the business logic, CQRS handlers (Commands & Queries), interfaces, and DTOs. Depends only on Domain.
-   **`QuestLog.Infrastructure`**: Implements interfaces from Application. Handles database access (EF Core), external services, and file storage.
-   **`QuestLog.Api`**: The entry point. Contains Controllers, Middleware, and DI configuration.

## Features

* **Authentication & Authorization:** Secure JWT-based authentication.
* **Quest System:** CRUD operations for daily, weekly, and epic quests.
* **Gamification Engine:** Logic to calculate XP gain, level ups, and unlocking achievements.
* **User Profiles:** Manage stats (Strength, Intellect, Charisma) based on completed task types.
* **Scalable Design:** Ready for cloud deployment (Azure/AWS).

## Getting Started

### Prerequisites

-   [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or later)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop)
-   [PostgreSQL](https://www.postgresql.org/download/) (if running locally without Docker)

### Running with Docker (Recommended)

The easiest way to start the entire system (API + Database) is using Docker Compose.

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/QuantGit43/questlog-api.git](https://github.com/QuantGit43/questlog-api.git)
    cd questlog-api
    ```

2.  **Start the services:**
    ```bash
    docker-compose up --build
    ```

3.  **Access the API:**
    The API will be available at `http://localhost:5000` (or the port defined in your `docker-compose.yml`).
    Swagger UI: `http://localhost:5000/swagger`

### Running Locally

1.  **Update Connection String:**
    Ensure your `appsettings.Development.json` in `QuestLog.Api` points to your local PostgreSQL instance.

2.  **Apply Migrations:**
    ```bash
    dotnet ef database update --project QuestLog.Infrastructure --startup-project QuestLog.Api
    ```

3.  **Run the API:**
    ```bash
    dotnet run --project QuestLog.Api
    ```
