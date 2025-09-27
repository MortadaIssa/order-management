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
    
3. Update configuration (optional):
    - By default the API uses SQLite with connection string Data Source=assessment.db.
    - To override, set ConnectionStrings__DefaultConnection (or appsettings.json) to your chosen path.
4. Use Existing assessment.db and existing migrations (faster) and directly run the API
    
    ```bash
    cd src
    dotnet run --project OrderManagement.Api
    ```
    
5. Open Swagger (default): `http://localhost:5275/swagger/index.html` (or port shown in console).
6. In case you want to run it from visual studio in debug mode (F5), you will be redirected to  `https://localhost:7040/swagger/index.html`

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
    - [Test Order Management -- Local.postman_collection.json](https://github.com/MortadaIssa/order-management/blob/4c691432e11ccd46d569f7f854a187ae1273eb61/postman-collections/Test%20Order%20Management%20--%20Local.postman_collection.json)
2. Import it into Postman:
    - In Postman, click **Import** > **File** > select the `.json` file.
    - Optionally, import the environment file: [Env Order Management -- Local.postman_environment.json](https://github.com/MortadaIssa/order-management/blob/4c691432e11ccd46d569f7f854a187ae1273eb61/postman-collections/Env%20Order%20Management%20--%20Local.postman_environment.json)
3. Set the `baseUrl` variable in the environment to point to your API instance.
4. Set the `token` variable in the environment to the one that you get from register or login — Make sure to keep the Bearer keyword and just replace the ##REPLACE_TOKEN_KEEP_Bearer##

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
> 

---

### Below are CURL Examples (alternative of postman)

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
