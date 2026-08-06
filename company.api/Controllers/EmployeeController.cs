using Azure.Core;
using company.api.Dto;
using company.api.Models;
using company.api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace company.api.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<CreateEmployeeRequest> _validator;
        public EmployeesController(IEmployeeService employeeService, IValidator<CreateEmployeeRequest> validator)
        {
            _employeeService = employeeService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetEmployees([FromQuery] int? departmentId, [FromQuery] string? status, [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var employees = await _employeeService.GetEmployeeListAsync(departmentId, status, pageNumber, pageSize);
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        // POST api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> AddEmployee(CreateEmployeeRequest req)
        {
            var validationResult = await _validator.ValidateAsync(req);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var employee = await _employeeService.AddEmployee(req);
            if (employee == null)
                return BadRequest();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        // PUT api/employees/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeRequest req)
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, req);
            if (employee == null)
                return NotFound();
            return NoContent();
        }

        // DELETE api/employees/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (result == false)
                return NotFound();
            return NoContent();
        }
    }
}
