using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore.Employees;

/// <summary>
/// Represents a management service for employees.
/// </summary>
public class EmployeeManagementService : IEmployeeManagementService
{
    private readonly NorthwindContext context;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeManagementService"/> class.
    /// </summary>
    /// <param name="context">NorthwindContext.</param>
    /// <param name="mapper">Mapper for entity mapping.</param>
    public EmployeeManagementService(NorthwindContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<int> CreateEmployeeAsync(Employee employee)
    {
        _ = employee ?? throw new ArgumentNullException(nameof(employee));

        await this.context.Employees.AddAsync(this.mapper.Map<EmployeeEntity>(employee));
        await this.context.SaveChangesAsync();
        return employee.EmployeeID;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity is not null)
        {
            this.context.Employees.Remove(employeeEntity);

            IQueryable<Order> orders = this.context.Orders.Where(order => order.Employee == employeeEntity);
            this.context.Orders.RemoveRange(orders);

            await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
    {
        IQueryable<Employee> employees = this.context.Employees
            .Skip(offset)
            .Take(limit)
            .Select(e => this.mapper.Map<Employee>(e));

        await foreach (Employee employee in employees.AsAsyncEnumerable())
        {
            yield return employee;
        }
    }

    /// <inheritdoc/>
    public async Task<Employee> GetEmployeeAsync(int employeeId)
    {
        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity is null)
        {
            return null;
        }

        return this.mapper.Map<Employee>(employeeEntity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
    {
        _ = employee ?? throw new ArgumentNullException(nameof(employee));

        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity is null)
        {
            return false;
        }

        employeeEntity.Address = employee.Address;
        employeeEntity.BirthDate = employee.BirthDate;
        employeeEntity.City = employee.City;
        employeeEntity.Country = employee.Country;
        employeeEntity.Extension = employee.Extension;
        employeeEntity.FirstName = employee.FirstName;
        employeeEntity.LastName = employee.LastName;
        employeeEntity.HireDate = employee.HireDate;
        employeeEntity.HomePhone = employee.HomePhone;
        employeeEntity.Notes = employee.Notes;
        employeeEntity.Photo = employee.Photo;
        employeeEntity.PhotoPath = employee.PhotoPath;
        employeeEntity.PostalCode = employee.PostalCode;
        employeeEntity.Region = employee.Region;
        employeeEntity.ReportsTo = employee.ReportsTo;
        employeeEntity.Title = employee.Title;
        employeeEntity.TitleOfCourtesy = employee.TitleOfCourtesy;

        await this.context.SaveChangesAsync();
        return true;
    }
}
