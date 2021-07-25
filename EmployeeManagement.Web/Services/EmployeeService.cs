using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Services
{
    public class EmployeeService : IEmployeeService
    {
        /* This Http Client Package is avilable only via PM Console installation method...
         * Install-Package Microsoft.AspNetCore.Blazor.HttpClient -Version 3.2.0-preview3.20168.3
         */
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _httpClient.GetJsonAsync<Employee[]>("api/employees");
        }
    }
}
