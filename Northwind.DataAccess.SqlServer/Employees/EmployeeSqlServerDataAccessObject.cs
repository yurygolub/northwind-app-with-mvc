﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Employees;

namespace Northwind.DataAccess.SqlServer.Employees;

/// <summary>
/// Represents a SQL Server-tailored DAO for Northwind products.
/// </summary>
public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject
{
    private readonly SqlConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
    /// </summary>
    /// <param name="connection">A <see cref="SqlConnection"/>.</param>
    public EmployeeSqlServerDataAccessObject(SqlConnection connection)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <inheritdoc/>
    public async Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
    {
        _ = employee ?? throw new ArgumentNullException(nameof(employee));

        using var command = new SqlCommand("InsertEmployee", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        AddSqlParameters(employee, command);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        return await command.ExecuteNonQueryAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        if (employeeId <= 0)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
        }

        using var command = new SqlCommand("DeleteEmployee", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@employeeID", employeeId, SqlDbType.Int, isNullable: false);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    /// <inheritdoc/>
    public async Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
    {
        if (employeeId <= 0)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
        }

        using var command = new SqlCommand("FindEmployee", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@employeeID", employeeId, SqlDbType.Int, isNullable: false);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        return CreateEmployee(reader);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit)
    {
        if (offset < 0)
        {
            throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
        }

        if (limit < 1)
        {
            throw new ArgumentException("Must be greater than zero.", nameof(limit));
        }

        await foreach (var product in SelectEmployeesAsync(offset, limit))
        {
            yield return product;
        }

        async IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit)
        {
            await using var command = new SqlCommand("SelectEmployees", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.SetParameter("@offset", offset, SqlDbType.Int, isNullable: false);
            command.SetParameter("@limit", limit, SqlDbType.Int, isNullable: false);

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateEmployee(reader);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeTransferObject employee)
    {
        _ = employee ?? throw new ArgumentNullException(nameof(employee));

        using var command = new SqlCommand("UpdateEmployee", this.connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.SetParameter("@employeeID", employeeId, SqlDbType.Int, isNullable: false);
        AddSqlParameters(employee, command);

        if (this.connection.State != ConnectionState.Open)
        {
            await this.connection.OpenAsync();
        }

        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
    {
        return new EmployeeTransferObject
        {
            EmployeeID = (int)reader["EmployeeID"],
            LastName = (string)reader["LastName"],
            FirstName = (string)reader["FirstName"],
            Title = GetValueClass<string>("Title"),
            TitleOfCourtesy = GetValueClass<string>("TitleOfCourtesy"),
            BirthDate = GetValueStruct<DateTime>("BirthDate"),
            HireDate = GetValueStruct<DateTime>("HireDate"),
            Address = GetValueClass<string>("Address"),
            City = GetValueClass<string>("City"),
            Region = GetValueClass<string>("Region"),
            PostalCode = GetValueClass<string>("PostalCode"),
            Country = GetValueClass<string>("Country"),
            HomePhone = GetValueClass<string>("HomePhone"),
            Extension = GetValueClass<string>("Extension"),
            Photo = GetValueClass<byte[]>("Photo"),
            Notes = GetValueClass<string>("Notes"),
            ReportsTo = GetValueStruct<int>("ReportsTo"),
            PhotoPath = GetValueClass<string>("PhotoPath"),
        };

        T GetValueClass<T>(string text)
            where T : class
            => reader[text] == DBNull.Value ? null : (T)reader[text];

        T? GetValueStruct<T>(string text)
            where T : struct
            => reader[text] == DBNull.Value ? null : (T)reader[text];
    }

    private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
    {
        command.SetParameter("@lastName", employee.LastName, SqlDbType.NVarChar, 20, false);
        command.SetParameter("@firstName", employee.FirstName, SqlDbType.NVarChar, 10, false);
        command.SetParameter("@title", employee.Title, SqlDbType.NVarChar, 30);
        command.SetParameter("@titleOfCourtesy", employee.TitleOfCourtesy, SqlDbType.NVarChar, 25);
        command.SetParameter("@birthDate", employee.BirthDate, SqlDbType.DateTime);
        command.SetParameter("@hireDate", employee.HireDate, SqlDbType.DateTime);
        command.SetParameter("@address", employee.Address, SqlDbType.NVarChar, 60);
        command.SetParameter("@city", employee.City, SqlDbType.NVarChar, 15);
        command.SetParameter("@region", employee.Region, SqlDbType.NVarChar, 15);
        command.SetParameter("@postalCode", employee.PostalCode, SqlDbType.NVarChar, 10);
        command.SetParameter("@country", employee.Country, SqlDbType.NVarChar, 15);
        command.SetParameter("@homePhone", employee.HomePhone, SqlDbType.NVarChar, 24);
        command.SetParameter("@extension", employee.Extension, SqlDbType.NVarChar, 4);
        command.SetParameter("@photo", employee.Photo, SqlDbType.Image);
        command.SetParameter("@notes", employee.Notes, SqlDbType.NText);
        command.SetParameter("@reportsTo", employee.ReportsTo, SqlDbType.Int);
        command.SetParameter("@photoPath", employee.PhotoPath, SqlDbType.NVarChar, 255);
    }
}
