# Expense Tracker API

A modern, secure RESTful API built with **.NET 10** and **Entity Framework Core**, implementing authentication with **ASP.NET Core Identity & JWT**, and automated testing with **xUnit**.

---

## Features

- **Expenses Management:** Full CRUD operations for personal expenses with amounts, descriptions, and categories.
- **Category Organization:** Categorize expenses with unique name constraints and 1-to-many database relationships.
- **Financial Analytics:** Aggregate calculations (e.g. monthly total expenses) powered by optimized LINQ queries.
- **Authentication & Security:** 
  - User registration and password hashing via **ASP.NET Core Identity**.
  - Stateless authentication using **JSON Web Tokens (JWT)** with custom claims (`UserId`, `Email`).
  - Route authorization protection (`RequireAuthorization`).
  - User-isolated data: users can only view, edit, and aggregate their own expenses.
- **Database Architecture:** SQLite database powered by EF Core with Code-First migrations and Fluent API configurations.
- **Clean Architecture:** Modular Minimal APIs organized into feature-based endpoint definitions (`Endpoints/`, `DTOs/`, `Models/`, `Data/`).
- **Automated Testing:** Unit and parameterized tests written with **xUnit** using the **AAA (Arrange-Act-Assert)** pattern.
- **Interactive Documentation:** Integrated OpenAPI & Scalar API Reference.

---

## Project Structure

```text
ExpenseTrackerApi/
├── Data/                    # DbContext & Entity Framework configurations
│   └── ExpenseTrackerDb.cs
├── Models/                  # Relational Database Entities (Expense, Category)
│   ├── Category.cs
│   └── Expense.cs
├── DTOs/                    # Data Transfer Objects (Records & Classes)
│   ├── AuthDTOs.cs
│   ├── CategoryDTO.cs
│   └── ExpenseDTO.cs
├── Endpoints/               # Modular Minimal API route definitions
│   ├── AuthEndpoints.cs
│   ├── CategoryEndpoints.cs
│   └── ExpenseEndpoints.cs
├── Migrations/              # EF Core Code-First schema evolution
├── ExpenseTrackerApi.Tests/ # xUnit automated test suite
├── appsettings.json         # Database connection & JWT signing configuration
└── Program.cs               # Clean entry point & dependency injection setup
```

---

## API Endpoints

### Authentication (`/auth`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `POST` | `/auth/register` | Register a new user with email and password | No |
| `POST` | `/auth/login` | Authenticate credentials and receive a signed JWT | No |

### Categories (`/categories`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `GET` | `/categories` | List all available categories | No |
| `POST` | `/categories` | Create a new unique category | No |

### Expenses (`/expenses`)
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `GET` | `/expenses` | Get all expenses belonging to the authenticated user | Bearer JWT |
| `POST` | `/expenses` | Create an expense linked to the authenticated user | Bearer JWT |
| `GET` | `/expenses/{id}` | Get specific expense details | Bearer JWT |
| `PUT` | `/expenses/{id}` | Update an existing expense | Bearer JWT |
| `PATCH` | `/expenses/{id}` | Partially update an expense | Bearer JWT |
| `DELETE` | `/expenses/{id}` | Delete an expense | Bearer JWT |
| `GET` | `/expenses/total` | Calculate total sum of expenses for the user | Bearer JWT |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- SQLite3

### 1. Clone the repository
```bash
git clone https://github.com/gabrieloide/ExpenseTrackerApi.git
cd ExpenseTrackerApi
```

### 2. Restore and Build
```bash
dotnet restore
dotnet build
```

### 3. Run Database Migrations
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```
Navigate to `https://localhost:<port>/scalar/v1` in your browser to explore the interactive API reference.

---

## Running Automated Tests

Run the xUnit test suite with:
```bash
dotnet test
```

---

## License
This project is open-source and available under the MIT License.
