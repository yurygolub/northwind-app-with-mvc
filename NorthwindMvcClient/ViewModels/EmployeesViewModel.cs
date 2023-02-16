using System.Collections.Generic;
using NorthwindMvcClient.Models;

namespace NorthwindMvcClient.ViewModels
{
    public class EmployeesViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
