### Tech Stack

##### Backend

> C#
> ASP.NET Core Web API
> Entity Framework Core
> PostgreSQL
> xUnit
> EF Core InMemory provider for unit tests

##### Frontend

> Angular
> TypeScript
> Bootstrap
> Reactive Forms

### Running the Application Locally

##### Prerequisites

This `README.md` assumes:

- .NET SDK
- Node.js
- npm
- Angular CLI
- PostgreSQL

Are all installed.

##### DB Setup

The Application uses `PostgreSQL`.

Create a `PostgreSQL` database named: `support_desk`

Then configure the connection string in: `backend/appsettings.json`.
Example:

```
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=support_desk;Username=postgres;Password=YOUR_PASSWORD"
    }
}
```

Then from the backend directory, run the `EF Core` migrations:

> dotnet ef database update

The application contains a database seeder( as defined in the Assignment PDF) that automatically creates sample data when the database is empty.

##### Running The Backend

From the backend directory:

> dotnet restore

> dotnet ef database update

> dotnet run

The API will start on the configured ASP.NET Core development URL.

##### Running the Frontend

From the `frontend` directory:

> npm install

> ng serve

Then open the URL shown by Angular in the terminal, normally:

> http://localhost:4200

The frontend communicates with the ASP.NET Core API running locally.

### Business Rules

The main business rules are implemented in: `backend/Controllers/TicketsController.cs`.
The backend is treated as the authoritative layer for enforcing workflow rules.

##### Why the Business Rules Are in the Controller

For this assignment, the workflow rules were kept inside `TicketsController.cs` because the application is relatively small( and to speed up the coding process, since I only had 48 hours) and the controller provides a single authoritative entry point for ticket mutations.

This also keeps the implementation straightforward and makes the workflow rules easy to locate and review.

The status transition logic and due-date calculation are separated into dedicated private methods within the controller.

### Assumptions

My assumptions when I started this project:

1. Authentication and authorization are outside the scope of the assignment.
2. The comment author is provided by the client because there is no authentication/user system.
3. Only active agents can be assigned to tickets.
4. The frontend communicates directly with the REST API through Angular services.

### What I Would Improve

1. UI&UX. As of now, to make up for the lack of time I only used basic bootstrap.
2. Authentication and authorization.
3. Add more detailed validation and error handling on the frontend.
4. Improve frontend state management.
5. Add AuditLogs( audit history)

### Roughly how long the assignment took you

Around 12-14 hours.
