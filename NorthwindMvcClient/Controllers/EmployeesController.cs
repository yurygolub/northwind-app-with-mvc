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

        public async Task<IActionResult> Index([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            var employees = new List<Models.Employee>();
            await foreach (var item in this.managementService.GetEmployeesAsync(offset, limit))
            {
                employees.Add(this.mapper.Map<Models.Employee>(item));
            }

            return this.View(new EmployeesViewModel
            {
                Employees = employees,
            });
        }
    }
}
