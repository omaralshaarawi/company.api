using company.api.Dto;
using company.api.Models;
using company.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetEmployees([FromQuery] int? departmentId, [FromQuery] string? status)
        {
            var employees = await _employeeService.GetEmployeeListAsync(departmentId, status);
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
