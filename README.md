# Task Manager: C# & .NET 9 CLI + Web API

A .NET 9 task manager with two interfaces — a **CLI** console app and a **RESTful Web API** — sharing the same business logic and data layer. Demonstrates **Clean Architecture**, **SOLID principles**, **Repository Pattern**, **Dependency Injection**, and **EF Core / SQLite**.

---

## Project Structure

```
TaskManager.CLI/     — Console app (keyboard-driven menu)
TaskManager.Api/     — Web API (HTTP endpoints)
TaskManager.Tests/   — xUnit unit tests
```

Both the CLI and API use the same `TaskService` and `IRepository<T>` — the interface layer doesn't care whether a person or another program is calling it.

---

## Tech Stack

- **Language:** C# 13 (.NET 9)
- **Data Layer:** Entity Framework Core, SQLite, JSON Serialization
- **Testing:** xUnit, Moq
- **Patterns:** Repository Pattern, Dependency Injection, Clean Architecture

---

## Repositories (Data Storage)

Three repository implementations — swap them via DI:

| Repository | Storage | When to use |
|---|---|---|
| `SqlRepository<T>` | SQLite (EF Core) | Default |
| `JsonRepository<T>` | JSON file | Simple file-based storage |
| `InMemoryRepository<T>` | In-memory list | Testing |

---

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | /api/tasks | List all tasks |
| POST | /api/tasks | Create a task |
| PUT | /api/tasks/{id} | Mark task as completed |
| DELETE | /api/tasks/{id} | Delete a task |

---

## How to Run

Requires .NET 9 SDK.

### CLI
```bash
dotnet run --project TaskManager.CLI
```

### API
```bash
dotnet run --project TaskManager.Api
```
Then open `http://localhost:5000/api/tasks`.

### Tests
```bash
dotnet test
```

---

## Roadmap

- [x] Module 1: OOP, Repository Pattern & Defensive Programming
- [x] Module 2: Unit Testing with xUnit & Moq
- [x] Module 3: Dependency Injection & Service Layer
- [x] Module 4: SQL Persistence with EF Core
- [x] Module 5: ASP.NET Core Web API (Full CRUD)
