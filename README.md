# Healthcare Task

A small healthcare check-in application built with **ASP.NET Core**, **Angular**, **Clean Architecture**, **CQRS using MediatR**, **Entity Framework Core**, **JWT Authentication**, and **SQL Server**.

The application allows users to sign up, log in, check in patients, view patient records, search/filter patients, and view patient details. Patient-related APIs and pages are protected using JWT authentication.

---

## Technologies Used

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Clean Architecture
* CQRS with MediatR
* Repository Pattern
* FluentValidation
* JWT Authentication
* Swagger/OpenAPI
* Global Exception Handling Middleware
* Audit Logging
* Webhook Integration

### Frontend

* Angular
* Reactive Forms
* Angular Routing
* Route Guards
* HTTP Interceptors
* CSS
* JWT stored in browser localStorage

---

## Project Structure

```text
HealthcareAssignment/
  database/
    migration.sql

  src/
    Healthcare.Api/
    Healthcare.Application/
    Healthcare.Domain/
    Healthcare.Infrastructure/

  healthcare-angular/
```

---

## Backend Architecture Explanation

The backend follows **Clean Architecture** principles.

### 1. Domain Layer

Project:

```text
Healthcare.Domain
```

This layer contains the core business entities, such as:

* User
* Patient
* AuditLog
* WebhookEndpoint
* WebhookDeliveryLog

---

### 2. Application Layer

Project:

```text
Healthcare.Application
```

This layer contains the application business use cases.

It includes:

* Commands
* Queries
* DTOs
* Validators
* Repository interfaces
* Service abstractions
* Common exceptions
* MediatR pipeline behaviors

---

### 3. Infrastructure Layer

Project:

```text
Healthcare.Infrastructure
```

This layer contains external implementation details.

It includes:

* Entity Framework Core DbContext
* Repository implementations
* JWT token service
* Audit service
* Webhook service
* EF Core migrations

---

### 4. API Layer

Project:

```text
Healthcare.Api
```

This is the presentation layer for the backend.

It includes:

* Controllers
* Middleware
* Dependency Injection setup
* Authentication/Authorization configuration
* Swagger configuration
* CORS configuration

---

## Backend Features

### Authentication

Implemented endpoints:

```http
POST /api/auth/signup
POST /api/auth/login
```

After login/signup, the API returns a JWT token. The frontend stores the token and sends it with protected requests.

---

### Patients

Implemented protected endpoints:

```http
POST /api/patients/checkin
GET  /api/patients
GET  /api/patients/{id}
```

The patients list supports pagination and filtering:

```http
GET /api/patients?pageNumber=1&pageSize=10&search=test
```

---

### Validation

FluentValidation is used for request validation. Validation runs through a MediatR pipeline behavior before the request reaches the handler.

---

### Error Handling

A global exception handling middleware returns consistent error responses using ProblemDetails.

---

### Audit Logging

Important user actions are stored in the database through the audit logging service.

Examples:

* User signup
* User login
* Patient check-in
* Patient list viewed
* Patient details viewed

---

### Webhook Integration

When a patient check-in is created, the system can send a webhook event to configured active webhook endpoints.

Webhook delivery attempts are logged in the database.

If webhook delivery fails, the patient check-in still succeeds.

---

## Frontend Explanation

The Angular frontend is built as a single-page application.

### Main frontend features

* Signup page
* Login page
* Patient check-in page
* Patients list page
* Patient details page
* Pagination/search support
* Loading and error states
* Protected routes using Angular route guards
* JWT token automatically attached using an HTTP interceptor

---

## Authentication Flow

1. User signs up or logs in.
2. Backend returns JWT token.
3. Angular stores token in `localStorage`.
4. Angular HTTP interceptor adds the token to API requests:

```http
Authorization: Bearer <token>
```

5. Protected backend endpoints validate the JWT token.
6. If the backend returns `401 Unauthorized`, Angular removes the token and redirects the user to the login page.

---

## Backend Setup Instructions

### Prerequisites

Install:

* .NET SDK
* SQL Server or SQL Server LocalDB
* Visual Studio or VS Code

---

### 1. Clone the repository

```bash
git clone <repository-url>
cd HealthcareAssignment
```

---

### 2. Configure the database connection

Open:

```text
src/Healthcare.Api/appsettings.json
```

Update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HealthcareAssignmentDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

### 3. Apply database migrations

Using .NET CLI:

```bash
dotnet ef database update --project src/Healthcare.Infrastructure --startup-project src/Healthcare.Api
```

Or using Visual Studio Package Manager Console:

```powershell
Update-Database -Project Healthcare.Infrastructure -StartupProject Healthcare.Api
```

---

### 4. Run the backend API

```bash
dotnet run --project src/Healthcare.Api
```

Swagger should be available at:

```text
https://localhost:7163/swagger
```

If the port is different, use the port shown in the terminal.

---

## Swagger JWT Testing

1. Run the backend API.
2. Open Swagger.
3. Call:

```http
POST /api/auth/login
```

or:

```http
POST /api/auth/signup
```

4. Copy the returned JWT token.
5. Click the Swagger **Authorize** button.
6. Paste only the token value.
7. Test protected patient endpoints.

---

## Frontend Setup Instructions

### Prerequisites

Install:

* Node.js
* Angular CLI

If Angular CLI is not installed:

```bash
npm install -g @angular/cli
```

---

### 1. Go to the Angular project

```bash
cd healthcare-angular
```

---

### 2. Install dependencies

```bash
npm install
```

---

### 3. Check API URL

Open:

```text
src/environments/environment.ts
```

Make sure the API URL matches the backend port:

```ts
export const environment = {
  apiUrl: 'https://localhost:7163/api'
};
```

---

### 4. Run Angular

```bash
ng serve
```

Open:

```text
http://localhost:4200
```

---

## CORS

The backend is configured to allow the Angular development server:

```text
http://localhost:4200
```

This is required because the Angular frontend and ASP.NET Core backend run on different ports during development.

---

## Database Migration Script

A generated SQL migration script is included at:

```text
database/migration.sql
```

It can be generated using:

```bash
dotnet ef migrations script --project src/Healthcare.Infrastructure --startup-project src/Healthcare.Api --output database/migration.sql --idempotent
```

---

## Completed Requirements

### Backend

* Signup
* Login
* JWT Authentication
* Protected patient APIs
* Patient check-in
* Patient list
* Patient details
* Clean Architecture
* CQRS using MediatR
* Repository Pattern
* Dependency Injection
* FluentValidation
* Entity Framework Core
* Swagger/OpenAPI
* Global Exception Handling Middleware
* Audit logging
* Webhook integration
* Pagination and filtering

### Frontend

* Angular frontend
* Login page
* Signup page
* JWT token storage
* HTTP interceptor
* Route guard
* Protected patient pages
* Patient check-in form
* Patients list with pagination/search
* Patient details page
* Loading and error states


