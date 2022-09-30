# Northwind Applications

### Build and Run
```sh
git clone https://github.com/yurygolub/northwind-app.git
cd northwind-app\NorthwindApiApp
dotnet run
```

### Create database
before using local database services you have to create a database
* create database using SQL script [instnwnd.sql](https://github.com/microsoft/sql-server-samples/blob/master/samples/databases/northwind-pubs/instnwnd.sql)
* create stored procedures using this file: \northwind-app\Northwind.DataAccess.SqlServer\Sql scripts\dbo.CreaterProcedures.sql
