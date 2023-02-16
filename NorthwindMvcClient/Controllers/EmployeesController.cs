using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Employees;
using NorthwindMvcClient.ViewModels;

namespace NorthwindMvcClient.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeManagementService managementService;
        private readonly IMapper mapper;

        public EmployeesController(IEmployeeManagementService managementService, IMapper mapper)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 5)
        {
            var employees = new List<Models.Employee>();

            IAsyncEnumerable<Employee> result = this.managementService.GetEmployeesAsync(offset, limit);
            int count = 0;
            await foreach (var item in result)
            {
                var employee = this.mapper.Map<Models.Employee>(item);
                if (employee.Photo?.Length != 0)
                {
                    employee.Photo = employee.Photo[78..];
                }

                employees.Add(employee);
                count++;
            }

            return this.View(new EmployeesViewModel
            {
                Employees = employees,
                PageViewModel = new PageViewModel
                {
                    Offset = offset,
                    Limit = limit,
                    Count = count,
                },
            });
        }
    }
}
