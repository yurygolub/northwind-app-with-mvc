# Northwind Applications

## NorthwindMvcClient

### Build and Run

```sh
git clone https://github.com/yurygolub/northwind-app-with-mvc.git
```

```sh
cd northwind-app-with-mvc\NorthwindMvcClient
```

```sh
dotnet run
```

### Release

```powershell
.\publish.ps1 -Project .\NorthwindMvcClient\
```

## NorthwindApiApp

### Build and Run

```sh
git clone https://github.com/yurygolub/northwind-app-with-mvc.git
```

```sh
cd northwind-app-with-mvc\NorthwindApiApp
```

```sh
dotnet run
```

### Release

```powershell
.\publish.ps1 -Project .\NorthwindApiApp\
```

### API

#### Products

| Operation        | HTTP Verb | URI                |
| ---------------- | --------- | ------------------ |
| Create           | POST      | /api/products      |
| Read (all items) | GET       | /api/products      |
| Read (item)      | GET       | /api/products/{id} |
| Update           | PUT       | /api/products/{id} |
| Delete           | DELETE    | /api/products/{id} |

#### ProductCategories

| Operation        | HTTP Verb | URI                  |
| ---------------- | --------- | -------------------- |
| Create           | POST      | /api/categories      |
| Read (all items) | GET       | /api/categories      |
| Read (item)      | GET       | /api/categories/{id} |
| Update           | PUT       | /api/categories/{id} |
| Delete           | DELETE    | /api/categories/{id} |

| Operation        | HTTP Verb | URI                                  | Request body    | Response body  |
| ---------------- | --------- | ------------------------------------ | --------------- | -------------- |
| Upload picture   | PUT       | /api/categories/{categoryId}/picture | Picture stream  | None           |
| Get picture      | GET       | /api/categories/{categoryId}/picture | None            | Picture stream |
| Delete picture   | DELETE    | /api/categories/{categoryId}/picture | None            | None           |

#### Employees

| Operation        | HTTP Verb | URI                 |
| ---------------- | --------- | ------------------- |
| Create           | POST      | /api/employees      |
| Read (all items) | GET       | /api/employees      |
| Read (item)      | GET       | /api/employees/{id} |
| Update           | PUT       | /api/employees/{id} |
| Delete           | DELETE    | /api/employees/{id} |

| Operation        | HTTP Verb | URI                               | Request body    | Response body  |
| ---------------- | --------- | --------------------------------- | --------------- | -------------- |
| Upload photo     | PUT       | /api/employees/{employeeId}/photo | Photo stream    | None           |
| Get photo        | GET       | /api/employees/{employeeId}/photo | None            | Photo stream   |
| Delete photo     | DELETE    | /api/employees/{employeeId}/photo | None            | None           |

#### Blogging

| Operation        | HTTP Verb | URI                |
| ---------------- | --------- | ------------------ |
| Create           | POST      | /api/articles      |
| Read (all items) | GET       | /api/articles      |
| Read (item)      | GET       | /api/articles/{id} |
| Update           | PUT       | /api/articles/{id} |
| Delete           | DELETE    | /api/articles/{id} |

Create JSON payload structure:

```json
{
    "title": "Gourmet Quality: Mascarpone Fabioli",
    "text": "A text that describes Mascarpone Fabioli...",
    "authorId": 1
}
```

Read (all items) JSON response structure:

```json
[
    {
        "id": 1,
        "title": "Gourmet Quality: Mascarpone Fabioli",
        "posted": "2012-04-23T18:25:43.511Z",
        "authorId": 1,
        "authorName": "Nancy Davolio, Sales Representative",
    }
]
```

Read (item) JSON response structure:

```json
{
    "id": 1,
    "title": "Gourmet Quality: Mascarpone Fabioli",
    "posted": "2012-04-23T18:25:43.511Z",
    "authorId": 1,
    "authorName": "Nancy Davolio, Sales Representative",
    "text": "A text that describes Mascarpone Fabioli..."
}
```

Update JSON payload structure:

```json
{
    "title": "Gourmet Quality: Mascarpone Fabioli",
    "text": "A text that describes Mascarpone Fabioli..."
}
```

| Operation                                           | HTTP Verb | URI                                      |
| --------------------------------------------------- | --------- | ---------------------------------------- |
| Return all related products                         | GET       | /api/articles/{article-id}/products      |
| Create a link to a product for an article           | POST      | /api/articles/{article-id}/products/{id} |
| Remove an existed link to a product from an article | DELETE    | /api/articles/{article-id}/products/{id} |

#### Comments

| Operation        | HTTP Verb | URI                                      |
| ---------------- | --------- | ---------------------------------------- |
| Create           | POST      | /api/articles/{article-id}/comments      |
| Read (all items) | GET       | /api/articles/{article-id}/comments      |
| Update           | PUT       | /api/articles/{article-id}/comments/{id} |
| Delete           | DELETE    | /api/articles/{article-id}/comments/{id} |

### Change services

use following files to configure services
* in production mode: \northwind-app-with-mvc\NorthwindApiApp\appsettings.json
* in development mode: \northwind-app-with-mvc\NorthwindApiApp\appsettings.Development.json

set "Mode" to use one of the following service types
* "Ef" - use local database using Entity Framework Core
* "Sql" use local database using ADO.NET

### Create databases

#### Northwind

before using local database services you have to create a database
* create database using SQL script [instnwnd.sql](https://github.com/microsoft/sql-server-samples/blob/65c27abe8a87c4990998a7007f1d9ef912c0f366/samples/databases/northwind-pubs/instnwnd.sql)
* create stored procedures using this file: \northwind-app-with-mvc\Northwind.DataAccess.SqlServer\sql-scripts\create-procedures.sql

Download script file

```powershell
curl.exe -LO https://raw.githubusercontent.com/microsoft/sql-server-samples/65c27abe8a87c4990998a7007f1d9ef912c0f366/samples/databases/northwind-pubs/instnwnd.sql
```

```powershell
SQLCMD.EXE -E -S "(LocalDB)\MSSQLLocalDB" -i .\instnwnd.sql
SQLCMD.EXE -E -S "(LocalDB)\MSSQLLocalDB" -d Northwind -i .\Northwind.DataAccess.SqlServer\sql-scripts\create-procedures.sql
```

Connect to mssqllocaldb

```powershell
SQLCMD.EXE -E -S "(LocalDB)\MSSQLLocalDB"
```

In case of problems with connecting

```powershell
SqlLocalDB.exe info MSSQLLocalDB
SqlLocalDB.exe stop MSSQLLocalDB
SqlLocalDB.exe delete MSSQLLocalDB
SqlLocalDB.exe start MSSQLLocalDB
```

> This will delete all your databases located in MSSQLLocalDB. Proceed with caution

Get all databases

```sql
SELECT name FROM sys.databases
GO
```

#### NorthwindBlogging

Run the following command to make the Entity Framework tool available:

```sh
dotnet tool restore
```

Apply migrations:

```sh
dotnet ef database update --project Northwind.Services.EntityFrameworkCore.Blogging
```

##### PowerShell

set environment variable:

```powershell
$Env:SQLCONNSTR_NORTHWIND_BLOGGING = 'Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; Initial Catalog=NorthwindBlogging'
```

show environment variables:

```powershell
ls Env:\
```

##### Command prompt

set environment variable:

```sh
set SQLCONNSTR_NORTHWIND_BLOGGING=Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; Initial Catalog=NorthwindBlogging
```

show environment variables:

```sh
set
```
