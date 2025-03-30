# HCMApp – Human Capital Management Web App

## Overview
HCMApp is a full-stack employee management system built with:
- ASP.NET Core Web API (backend)
- PostgreSQL (database)
- HTML + Bootstrap (UI)
- Docker Compose (deployment & orchestration)
- JWT-based authentication and role-based authorization

---

## Features
- 🔐 User authentication via JWT
- ➕ Create, ✏️ Update, 🗑️ Delete employees
- 👤 Role-based access control (HRAdmin, Manager, Employee)
- 📋 Swagger API documentation
- 🖥️ Responsive web UI served via `wwwroot`
- 🐳 Dockerized environment

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- pgAdmin or DBeaver (optional for DB access)

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/your-org/HCMApp.git
cd HCMApp
```

### 2. Start the application
```bash
docker-compose up --build
```

- API available at: `http://localhost:8080`
- Swagger docs: `http://localhost:8080/swagger`
- Web UI: `http://localhost:8080/EmployeeUi.html`
- PostgreSQL exposed at `localhost:5432`

### 3. Login Credentials
Use the default admin credentials:
```
Email: admin@hcmapp.com
Password: Admin@123
```

---

## Roles
| Role      | Permissions                     |
|-----------|----------------------------------|
| HRAdmin   | Full access to employee actions |
| Manager   | Read and update only            |
| Employee  | Read-only                       |

---

## Project Structure
```
HCMApp/
├── API/                  # ASP.NET Core Web API
│   ├── Controllers/
│   ├── Services/
│   └── Program.cs
├── Persistence/          # EF Core DbContext + Repositories
├── wwwroot/              # Static HTML UI (EmployeeUi.html)
├── docker-compose.yml    # Docker services
├── README.md             # This file
```

---

## Migrations
Apply migrations using:
```bash
dotnet ef database update   --project HCMApp.Persistence   --startup-project HCMApp.API
```

Or auto-migrate at app startup using:
```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HCMDbContext>();
    db.Database.Migrate();
}
```

---

## Testing
Run all tests:
```bash
dotnet test
```
Includes unit tests and integration tests for:
- AuthService
- EmployeeService
- API endpoints

---

## Troubleshooting
| Issue                                | Fix                                                         |
|-------------------------------------|--------------------------------------------------------------|
| `password authentication failed`    | Ensure DB password in Docker and appsettings match          |
| `relation "AspNetRoles" does not exist` | Run EF Core migrations inside the correct container          |
| `getaddrinfo failed`                | Use `Host=db` inside Docker, `localhost` from your machine  |
| Swagger shows 404 on auth routes    | Use the Authorize 🔐 button with a valid JWT token           |

---

## License
MIT or your custom license.

---

## Contributors
Built with ❤️ using open source tools and frameworks.
