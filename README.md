# Library API (MongoDB + Docker)

A .NET Core / .NET 9 Web API for managing a library system, backed by MongoDB database.  
This project is structured with clean architecture (Domain, Application, Infrastructure, API) and supports basic CRUD operations on books, authors, categories, publishers, etc.

---

## 📁 Repository Structure
```
LibraryApiSqlServer/
├── Library.Api/                   # API / presentation layer (controllers, endpoints)
├── Library.Application/           # Application logic
│   ├── Services/                  # Business services / use cases
│   └── DTOs/                      # Data Transfer Objects, ViewModels
├── Library.Domain/                # Domain / core (entities, interfaces)
├── Library.Infrastructure/        # Data access, repository implementations, EF Core, DB context
├── LibraryApiSqlServer.sln
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── README.md
```

---

## ⚙️ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download) or compatible .NET version  
- MongoDB (local or remote)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for containerized setup (Optional)
- A tool like **SMongoDB Compass** for DB inspection (Optional)

---

## 🔧 Setup / Getting Started

**1. Clone the repository**

```bash
   git clone https://github.com/rashedulalam46/library-api-mongo.git
   cd library-api-sqlserver
```

**2. Configure connection string**
   
Open appsettings.json or appsettings.Development.json, and set up your ConnectionStrings:DefaultConnection to point to your SQL Server.

```

{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "library",
    "BooksCollectionName": "books",
    "AuthorsCollectionName": "authors",
    "PublishersCollectionName": "publishers",
    "CategoriesCollectionName": "categories",
    "CountriesCollectionName": "countries"
  }
}
```

If you are using Docker, then use

```
{
  "ConnectionStrings": {
    "ConString": "Server=host.docker.internal;Database=LibraryDb;User Id=…;Password=…;"
  }
}
```

**3. Apply migrations / create database**
   
In the Infrastructure project (or main solution root), run:

```
dotnet ef database update
```

This will create the database and necessary tables.

**4. Build & run the API**

```
dotnet build
dotnet run
```

The default launch URL might be https://localhost:5001 (or as configured). Use a tool like Postman, curl, or HTTPie to test the endpoints.

## 🛣️ API Endpoints (Examples)

These are sample endpoints — adjust according to actual implementation.
| Method | URL             | Description             |
| ------ | --------------- | ----------------------- |
| GET    | /api/books      | Get all books           |
| GET    | /api/books/{id} | Get book by ID          |
| POST   | /api/books      | Create a new book       |
| PUT    | /api/books/{id} | Update an existing book |
| DELETE | /api/books/{id} | Delete a book by ID     |

## ✅ Features & Highlights

- Clean / layered architecture (Domain, Application, Infrastructure)
- Dependency Injection
- EF Core as data access layer
- DTOs / ViewModels for input/output
- Configuration-based connection strings
- Exception handling, validation, etc. (if implemented)

 ## 📦 Deployment

When you’re ready to deploy:

- Configure the production connection string in environment variables.
- Publish the project:

```
dotnet publish --configuration Release
```

- Deploy the resulting output to your server / host / container.
- Ensure the database is accessible and migrations are applied.

##  📄 License & Contribution

- Feel free to fork or suggest changes via pull requests.
- Add a LICENSE file if you have specific usage terms.
- Please document style, code conventions, etc., in a CONTRIBUTING.md.



