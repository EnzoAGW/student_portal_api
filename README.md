# Portal Escolar

Full-stack school management platform with two portals: **Student** (grades, attendance, enrollment) and **Employee** (time tracking, scheduling, shifts).

## Stack

| Layer | Technology |
|---|---|
| Frontend | React 19 · TypeScript · Vite · Tailwind CSS 4 |
| State | Zustand · TanStack React Query |
| Backend | ASP.NET Core 10 · C# |
| Database | PostgreSQL 17 |
| Auth | JWT Bearer |
| Tests | xUnit · Moq · EF Core InMemory |

## Features

**Student Portal**
- View enrolled subjects by semester
- Check grades per subject and evaluation type
- Track attendance with presence percentage

**Employee Portal**
- Time recording — clock in/out with lunch break calculation
- Work schedule — view assigned shifts by date
- Shift management — morning, afternoon, night definitions

## Project Structure

```
WebApplication1/
├── back/          # ASP.NET Core API
│   ├── Controllers/
│   ├── Model/         # Entities + repository interfaces
│   ├── infra/         # EF Core repositories + DbContext
│   ├── ViewModel/     # Request DTOs
│   └── Services/      # JWT token generation
├── front/         # React + TypeScript (Vite)
│   └── src/
│       ├── features/  # student/ and employee/ feature modules
│       ├── components/
│       ├── store/     # Zustand auth + portal state
│       └── lib/       # Axios instance + React Query config
└── WebApplication1.Tests/  # xUnit test suite
```

## Running locally

**Prerequisites:** .NET 10 SDK · Node 20+ · PostgreSQL running on `localhost:5432`

**1. Database**

Create a database named `employee_sample` in PostgreSQL. The application uses EF Core and expects the schema to exist — run migrations or let the context auto-create on first run.

```
Host:     localhost:5432
Database: employee_sample
User:     postgres
Password: 1234
```

**2. Backend**

```bash
cd back
dotnet run
# API available at http://localhost:5266
# Swagger at http://localhost:5266/swagger
```

**3. Frontend**

```bash
cd front
npm install
npm run dev
# App available at http://localhost:3000
```

## Authentication

The API uses JWT Bearer tokens. Credentials for local testing:

| Username | Password | Role | Access |
|---|---|---|---|
| admin | 123 | admin | Full |
| rh | rh123 | rh | Employee portal |
| secretaria | sec123 | secretaria | Student portal |
| professor | prof123 | professor | Full |

Login endpoint:
```
POST /api/v1/auth?username={user}&password={password}
```

## API overview

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/v1/auth` | No | Get JWT token |
| GET | `/api/v1/student` | Yes | List students |
| GET | `/api/v1/grade/student/{id}` | Yes | Grades by student |
| GET | `/api/v1/attendance/student/{id}` | Yes | Attendance by student |
| GET | `/api/v1/timerecord/employee/{id}` | Yes | Time records |
| GET | `/api/v1/workschedule/employee/{id}` | Yes | Work schedule |

Full endpoint reference in [`back/README.md`](back/README.md).

## Tests

28 tests across models, repositories, and controllers.

```bash
cd WebApplication1.Tests
dotnet test
```

Coverage includes:
- `TimeRecord` worked hours calculation (with/without break, overtime)
- Grade and Attendance repository operations against in-memory DB
- Controller responses via Moq-injected repositories
