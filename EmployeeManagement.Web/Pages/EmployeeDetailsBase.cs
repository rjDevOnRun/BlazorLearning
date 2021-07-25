using EmployeeManagement.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Pages
{
    public class EmployeeDetailsBase: ComponentBase
    {
        
        // for handling the route parameter
        [Parameter]
        public string Id { get; set; }

        // for handling the service calls
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        // for the View-Model data
        public Employee Employee { get; set; } = new Employee();

        protected async override Task OnInitializedAsync()
        {
            // Assign default Id
            Id = Id ?? "1";

            // Using Employee Services
            Employee = await EmployeeService.GetEmployee(int.Parse(Id));
        }

    }
}
