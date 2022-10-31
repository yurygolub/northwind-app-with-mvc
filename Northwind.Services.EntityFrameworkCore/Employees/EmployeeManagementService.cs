using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly Context.NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementService"/> class.
        /// </summary>
        /// <param name="context">NorthwindContext.</param>
        /// <param name="mapper">Mapper for entity mapping.</param>
        public EmployeeManagementService(Context.NorthwindContext context, IMapper mapper)
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
        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            var employee = await this.context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                this.context.Employees.Remove(employee);

                var orders = this.context.Orders.Where(order => order.Employee == employee);
                this.context.Orders.RemoveRange(orders);

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            var employees = this.context.Employees
                    .Skip(offset)
                    .Take(limit)
                    .Select(e => this.mapper.Map<Employee>(e));

            await foreach (var employee in employees.AsAsyncEnumerable())
            {
                yield return employee;
            }
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            var contextEmployee = await this.context.Employees.FindAsync(employeeId);
            if (contextEmployee is null)
            {
                return null;
            }

            return this.mapper.Map<Employee>(contextEmployee);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            _ = employee ?? throw new ArgumentNullException(nameof(employee));

            var contextEmployee = await this.context.Employees.FindAsync(employeeId);
            if (contextEmployee is null)
            {
                return false;
            }

            contextEmployee.Address = employee.Address;
            contextEmployee.BirthDate = employee.BirthDate;
            contextEmployee.City = employee.City;
            contextEmployee.Country = employee.Country;
            contextEmployee.Extension = employee.Extension;
            contextEmployee.FirstName = employee.FirstName;
            contextEmployee.LastName = employee.LastName;
            contextEmployee.HireDate = employee.HireDate;
            contextEmployee.HomePhone = employee.HomePhone;
            contextEmployee.Notes = employee.Notes;
            contextEmployee.Photo = employee.Photo;
            contextEmployee.PhotoPath = employee.PhotoPath;
            contextEmployee.PostalCode = employee.PostalCode;
            contextEmployee.Region = employee.Region;
            contextEmployee.ReportsTo = employee.ReportsTo;
            contextEmployee.Title = employee.Title;
            contextEmployee.TitleOfCourtesy = employee.TitleOfCourtesy;

            await this.context.SaveChangesAsync();
            return true;
        }
    }
}
