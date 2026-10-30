# WorkTrack

WorkTrack is a C#/.NET 10 application for managing work items through a REST API.

The project demonstrates domain-driven business rules, ASP.NET Core controllers, dependency injection, Entity Framework Core, SQL Server, API validation, OpenAPI documentation, and automated testing.

## Technology Stack

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- xUnit
- OpenAPI

## Project Structure

| Project | Responsibility |
|---|---|
| `WorkTrack.Api` | HTTP endpoints, request validation and response contracts |
| `WorkTrack.Domain` | Entities, enums, interfaces and business rules |
| `WorkTrack.Infrastructure` | EF Core database context and persistence |
| `WorkTrack.Domain.Tests` | Domain unit tests |
| `WorkTrack.Infrastructure.Tests` | Database integration tests |
| `WorkTrack.Playground` | C# learning and experimentation |

## Running the API

**Prerequisites**

- .NET 10 SDK
- SQL Server
- A configured `WorkTrackDb` connection string
- An initialized WorkTrack database

From the solution root:

```powershell
dotnet build WorkTrack.slnx
dotnet run --project .\WorkTrack.Api
```

The application prints its listening address when started.

In the Development environment, the OpenAPI document is available at:

`/openapi/v1.json`

Example using the local development port:

`http://localhost:5071/openapi/v1.json`

The actual port may differ.

## API Endpoints

| Method | Endpoint | Description | Expected status codes |
|---|---|---|---|
| GET | `/api/WorkItems` | Retrieve work items with optional filters and pagination | 200, 400 |
| GET | `/api/WorkItems/{id}` | Retrieve one work item by GUID | 200, 404 |
| GET | `/api/WorkItems/high-priority` | Retrieve High-priority work items | 200 |
| POST | `/api/WorkItems` | Create a work item | 201, 400 |
| PUT | `/api/WorkItems/{id}/priority` | Update priority | 204, 400, 404, 409 |
| POST | `/api/WorkItems/{id}/close` | Close a work item | 204, 404, 409 |
| DELETE | `/api/WorkItems/{id}` | Delete a work item | 204, 404 |

These are the documented application responses, not an exhaustive list of possible infrastructure failures.

## GET Filtering and Pagination

`GET /api/WorkItems` accepts optional query parameters:

| Parameter | Description |
|---|---|
| `priority` | Filter by work item priority |
| `status` | Filter by work item status |
| `sort` | Supported value: `dueDate` |
| `page` | Page number, minimum 1 when supplied |
| `pageSize` | Number of items per page, between 1 and 100 when supplied |

Example:

```http
GET /api/WorkItems?page=1&pageSize=10
```

The successful response contains `page`, `pageSize`, `totalCount`, and `items`.

## Creating a Work Item

**Endpoint:** `POST /api/WorkItems`

Example JSON request:

```json
{
  "title": "Prepare ASP.NET interview",
  "description": "Practice API and DI questions",
  "priority": 2,
  "dueDate": "2027-01-15T10:00:00Z"
}
```

The due date must be in the future when the request is submitted. Update the example date if necessary.

A successful creation returns HTTP `201 Created`, a `Location` response header, and a `WorkItemResponse` JSON body.

## Updating Priority

**Endpoint:** `PUT /api/WorkItems/{id}/priority`

Request:

```json
{
  "priority": 3
}
```

Priority values:

| Value | Priority |
|---|---|
| 0 | Low |
| 1 | Medium |
| 2 | High |
| 3 | Critical |

Successful updates return `204 No Content`.

Updating a closed work item returns `409 Conflict`.

## Closing a Work Item

**Endpoint:** `POST /api/WorkItems/{id}/close`

Closing changes the item's status to Closed and records its `ClosedAt` timestamp.

A successful close returns `204 No Content`.

Attempting to close an already closed item returns `409 Conflict`.

## Deleting a Work Item

**Endpoint:** `DELETE /api/WorkItems/{id}`

A successful deletion returns `204 No Content`.

Requesting or attempting to delete a nonexistent item returns `404 Not Found`.

## Error Responses

The API supports Problem Details responses for several errors, including validation failures.

The current implementation returns plain-text messages for certain `409 Conflict` responses.

The error response formats are not yet completely standardized.

## Automated Tests

The solution contains:

- Domain unit tests validating business rules.
- EF Core integration tests, including a regression test for loading existing overdue work items.

Run the tests:

```powershell
dotnet test WorkTrack.slnx
```

The integration test uses an isolated SQLite in-memory database. SQLite testing does not guarantee identical behaviour across all SQL Server-specific features.

## Current Development Status

Implemented:

- Work item creation and retrieval
- Priority filtering and updates
- Pagination
- Work item closure
- Work item deletion
- Domain validation
- OpenAPI response documentation
- Domain and infrastructure testing

Further improvements include consistent error responses, additional automated API tests, frontend development, and deployment tooling.