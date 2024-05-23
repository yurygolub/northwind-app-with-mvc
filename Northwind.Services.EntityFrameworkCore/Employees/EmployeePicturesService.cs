using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore.Employees;

/// <summary>
/// Represents a management service for employee pictures.
/// </summary>
public class EmployeePicturesService : IEmployeePicturesService
{
    private readonly NorthwindContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeePicturesService"/> class.
    /// </summary>
    /// <param name="context">NorthwindContext.</param>
    public EmployeePicturesService(NorthwindContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<Stream> GetEmployeePictureAsync(int employeeId)
    {
        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity?.Photo is null)
        {
            return null;
        }

        return new MemoryStream(employeeEntity.Photo[78..]);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteEmployeePictureAsync(int employeeId)
    {
        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity is null)
        {
            return false;
        }

        employeeEntity.Photo = null;

        await this.context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateEmployeePictureAsync(int employeeId, Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        EmployeeEntity employeeEntity = await this.context.Employees.FindAsync(employeeId);
        if (employeeEntity is null)
        {
            return false;
        }

        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.ToArray().CopyTo(employeeEntity.Photo, 78);

        await this.context.SaveChangesAsync();

        return true;
    }
}
