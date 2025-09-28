# Order Management Project — .NET 8 Web API

Status: Completed, runs locally and deployed to Azure App Service.
Live Demo (Swagger UI):
http://ordermanagement-mortadaissa-2025.azurewebsites.net/swagger/index.html

---

## Overview

This repository contains a small Clean Architecture sample Web API built with .NET 8, demonstrating core backend patterns that a product-level BSS/BPM or CRM system might use:

- Layered solution: Domain, Application, Infrastructure, Api and Tests.
- EF Core (Code-First) with SQLite for persistence.
- JWT Bearer authentication protecting order endpoints.
- FluentValidation for DTO validation.
- DTOs and Repositories (Repository pattern).
- Minimal logging placeholders and unit tests (xUnit + Moq).

The goal was to produce a compact, clean, and deployable sample that is easy to read.

---

## Live Azure App Service URL

Swagger UI:
http://ordermanagement-mortadaissa-2025.azurewebsites.net/swagger/index.html

<aside>
💡

Note: For testing authenticated endpoints, register on the deployed API (POST /api/auth/register) to obtain a token issued by the deployed server. Local tokens signed with a different JWT secret will not be valid against the deployed service.

</aside>

---

## Quick start — prerequisites

Install the following on your machine to run locally:

- .NET SDK 8.0
- Git
- Optional: sqlite3, DB Browser for SQLite (to inspect assessment.db)
- (For deployment) Azure CLI — Just in case you want to deploy on your azure portal

---

## Setup / Run locally

1. Clone the repository:
    
    ```bash
    git clone https://github.com/MortadaIssa/order-management.git
    cd order-management
    ```
    
2. Restore packages and build:
    
    ```bash
    dotnet restore
    dotnet build
    ```
    
3. Use the existing assessment.db and existing migrations (faster) and directly run the API
    
    ```bash
    cd src
    dotnet run --project OrderManagement.Api
    ```
    
4. Open Swagger (default): `http://localhost:5275/swagger/index.html` (or port shown in console).
5. In case you want to run it from visual studio in debug mode (F5), you will be redirected to  `https://localhost:7040/swagger/index.html`

---

## API Endpoints (summary)

### Auth

- `POST /api/auth/register`
    
    Body: `{ "name", "email", "password", "confirmPassword" }`
    
    Response: `201 Created` and `{ id, token }`.
    
- `POST /api/auth/login`
    
    Body: `{ "email", "password" }`
    
    Response: `200 OK` and `{ id, token }`
    

### **Orders** (Protected — require `Authorization: Bearer <token>`)

- `POST /api/orders`
    
    Body: `{ "items": [ { "name", "price", "quantity" } ] }`
    
    Response: `201 Created` with order id and total.
    
- `GET /api/orders/{id}`
    
    Response: `200 OK` with order and items, or `404 Not Found`
    

---

## **Postman collection**

A Postman collection is provided to help you quickly test the API endpoints.

1. Download the collection file from:
    - [Test Order Management -- Local.postman_collection.json](https://github.com/MortadaIssa/order-management/blob/1b79e56439d5acb7d6d33d8dc65cc522025d4e85/postman-collections/Test%20Order%20Management%20--%20Local.postman_collection.json)
2. Import it into Postman:
    - In Postman, click **Import** > **File** > select the `.json` file.
    - Optionally, import the environment file: [Env Order Management -- Local.postman_environment.json](https://github.com/MortadaIssa/order-management/blob/4c691432e11ccd46d569f7f854a187ae1273eb61/postman-collections/Env%20Order%20Management%20--%20Local.postman_environment.json)
3. Set the `baseUrl` variable in the environment to point to your API instance (in this case: http://localhost:5275)
4. Set the `token` variable in the environment to the one that you get from register or login — Make sure to add the `Bearer` keyword before the token
You can now run requests directly in Postman.

---

## Unit Test (locally)

There are sample unit tests in `OrderManagementProject.Tests` using **xUnit** and **Moq**. They demonstrate basic white-box tests for the `CreateOrderHandler` (successful create and error path when user does not exist).

Run tests locally (starting from application root):

```bash
cd tests\OrderManagement.Tests
dotnet test
```

---

## Test Online (faster)

The service is also exposed on a public azure portal link. You can access it using the below link

http://ordermanagement-mortadaissa-2025.azurewebsites.net/swagger/index.html

> If you need to test it online, you can also find a postman collection and environments under the repository postman-collection directory that are prepared for online test calls
> - [Test Order Management -- Online.postman_collection.json](https://github.com/MortadaIssa/order-management/blob/1b79e56439d5acb7d6d33d8dc65cc522025d4e85/postman-collections/Test%20Order%20Management%20--%20Online.postman_collection.json)
> - [Env Order Management -- Online.postman_environment.json](https://github.com/MortadaIssa/order-management/blob/3a53d5e9a695376725db7808b6813675c24c95a9/postman-collections/Env%20Order%20Management%20--%20Online.postman_environment.json)

---

### Below are CURL Examples (GIT BASH Not PowerShell - alternative of postman)

**Register**

```bash
curl -X POST "https://ordermanagement-mortadaissa-2025.azurewebsites.net/api/auth/register" \
 -H "Content-Type: application/json" \
 -d '{"name":"Mortada","email":"mortada@example.com","password":"P@ssw0rd123","confirmPassword":"P@ssw0rd123"}'
```

**Login**

```bash
curl -X POST "https://ordermanagement-mortadaissa-2025.azurewebsites.net/api/auth/login" \
 -H "Content-Type: application/json" \
 -d '{"email":"mortada@example.com","password":"P@ssw0rd123"}'
```

**Create Order** (replace `<TOKEN>` with token from register/login)

```bash
curl -X POST "https://ordermanagement-mortadaissa-2025.azurewebsites.net/api/orders" \
 -H "Content-Type: application/json" \
 -H "Authorization: Bearer <TOKEN>" \
 -d '{"items":[{"name":"Sample Item","price":12.50,"quantity":2}]}'
```

**Get Order (**replace `<TOKEN>` and `<order-id>`)

```bash
curl -X GET "https://ordermanagement-mortadaissa-2025.azurewebsites.net/api/orders/<order-id>" \
 -H "Authorization: Bearer <TOKEN>"
```

---

## Architectural Decisions

Below are the main decisions I made and the reasons behind them.

### Layering / Clean Architecture

- **Projects:** `Domain`, `Application`, `Infrastructure`, `Api`, `Tests`.
    
    **Reason:** This separation keeps domain models and business logic isolated from infrastructure (EF, configuration, logging) and the API layer. It makes testing and reasoning about code easier and is a common interview expectation.
    

### DTOs, Validation, and Mapping

- **DTOs** are used for input/output to decouple the external contract from EF entities.
- **FluentValidation** validates DTOs at the API boundary.
    
    **Reason:** To avoid exposing internal domain models and to prevent object graph cycles during serialization.
    

### Repositories and Handlers (CQRS-like)

- **Repository pattern** (generic base + concrete `UserRepository`, `OrderRepository`) used for persistence abstraction.
- **Handlers** (`RegisterUserHandler`, `CreateOrderHandler`) implement command-like logic (validate/use services/repositories).
    
    **Reason:** Provides clear single-responsibility components and makes unit testing straightforward.
    

### Authentication - JWT & Login

- Although the assessment did not require JWT, I implemented JWT Bearer authentication for order endpoints.
- I added both `register` and `login` endpoints. I chose to include `login` so the client can obtain tokens after registration and after token expiry—this is more robust for a live demo.
    
    **Reason:** Tokens are stateless and survive app restarts so long as the secret does not change; however, tokens expire and it is good practice to enable login to re-issue tokens.
    

### Persistence - SQLite (Code-First)

- **SQLite** chosen for simplicity and quick deployability to App Service (no external DB required for demo).
    
    **Reason:** Fast to set up and portable for a demo. Not intended as production DB for scaled apps.
    

### Logging & Error handling

- Simple console-based `LoggingService` is included as a placeholder; in production I would use a structured logging provider (Serilog/ELK/Azure Monitor).
    
    **Reason:** Keep the sample small, readable and hosting-friendly while demonstrating extension points.
    

### Simplicity vs production complexity

- I intentionally kept the project small and functional rather than adding complexity (refresh tokens, distributed DB, full identity, etc.). This trade-off is appropriate for an assessment where clarity and working deployment are important.

---

## Assumptions

- **Client**: The API is intended to be consumed by a web or mobile client. For example, a React SPA with:
    - a registration page calling `POST /api/auth/register`,
    - a login page calling `POST /api/auth/login`,
    - and protected pages that call `/api/orders` with `Authorization: Bearer <token>`.
- **Database**: SQLite is acceptable for the assessment demo. For production, a managed RDBMS (Azure SQL / PostgreSQL) would be used.
- **Token storage**: Clients are expected to store the JWT securely (HttpOnly cookie or secure storage for mobile). The README assumes a basic demo usage where the token is stored in memory or in local Dev tools for testing.
- **Single instance**: The deployed app runs as a single App Service instance for the demo. Multi-instance scaling with file-backed SQLite is not supported in this configuration.
- **Secrets**: The JWT secret is stored in Azure App Service Application Settings. The secret must be kept private and should be rotated in production.
