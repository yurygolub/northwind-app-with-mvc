using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.Services.Employees;

namespace Northwind.Services.EntityFrameworkCore.Employees
{
    /// <summary>
    /// Represents a management service for employee pictures.
    /// </summary>
    public class EmployeePicturesService : IEmployeePicturesService
    {
        private readonly Context.NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeePicturesService"/> class.
        /// </summary>
        /// <param name="context">NorthwindContext.</param>
        public EmployeePicturesService(Context.NorthwindContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<Stream> GetEmployeePictureAsync(int employeeId)
        {
            var employee = await this.context.Employees.FindAsync(employeeId);
            if (employee?.Photo is null)
            {
                return null;
            }

            return new MemoryStream(employee.Photo[78..]);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployeePictureAsync(int employeeId)
        {
            var employee = await this.context.Employees.FindAsync(employeeId);
            if (employee is null)
            {
                return false;
            }

            employee.Photo = null;

            await this.context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeePictureAsync(int employeeId, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var employee = await this.context.Employees.FindAsync(employeeId);
            if (employee is null)
            {
                return false;
            }

            await using MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.ToArray().CopyTo(employee.Photo, 78);

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
