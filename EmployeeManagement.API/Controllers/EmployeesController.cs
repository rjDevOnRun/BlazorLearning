using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Models;
using EmployeeManagement.Models;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search
            (string name, Gender? gender)
        {
            try
            {
                var result = await _employeeRepository.Search(name, gender);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);
                if(result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                // Check if repository already contains email-id
                var emp = _employeeRepository.GetEmployeeByEmail(employee.Email);

                if(emp.Result != null)
                {
                    ModelState.AddModelError("email", $"{employee.Email} is already in use..");
                    return BadRequest(ModelState);
                }

                // Create employee
                var createdEmployee = await _employeeRepository.AddEmployee(employee);

                // Return: status code, created resource, location header
                return CreatedAtAction(nameof(GetEmployee), 
                    new { id = createdEmployee.EmployeeId },
                    createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if(id != employee.EmployeeId)
                {
                    return BadRequest("Employee Id mismatch");
                }

                var empToUpdate = await _employeeRepository.GetEmployee(id);

                if(empToUpdate == null)
                {
                    return NotFound($"Employee with Id: {id} is not Found!");
                }

                return await _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var empToDelete = await _employeeRepository.GetEmployee(id);
                if (empToDelete == null)
                {
                    return NotFound($"Employee with Id: {id} is not Found!");
                }

                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error: {ex.Message}");
            }
        }
    }
}
