using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService _departmentsService;

        private readonly IEmployeeService _employeeService;
        public DepartmentsController(IDepartmentsService departmentsService, IEmployeeService employeeService)
        {
            _departmentsService = departmentsService;
            _employeeService = employeeService;
        }


        [HttpGet]
        public async Task<ActionResult<List<DepartmentResponse>>> GetDepartments()
        {
            var departments = await _departmentsService.GetDepartmentListAsync();
            if(departments == null || !departments.Any())
                return NotFound("No departments found.");
            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentResponse>> GetDepartment(int id)
        {
            var department = await _departmentsService.GetDepartmentAsync(id);
            if (department == null)
                return NotFound("Department not found.");
            return Ok(department);
        }

        // POST api/departments
        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> AddDepartment(CreateDepartmentRequest req)
        {
            var department = await _departmentsService.AddDepartment(req);
            if (department == null)
                return BadRequest("Failed to create department.");
            return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
        }

        // PUT api/departments/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentRequest req)
        {
            var department = await _departmentsService.UpdateDepartmentAsync(id, req);
            if (department  == null)
                return NotFound("Department not found.");
            return NoContent();
        }

        // DELETE api/departments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartmentAsync(int id)
        {
            var employeesInDepartment = await _employeeService.GetEmployeeListAsync(id, "Active");
            if(!employeesInDepartment.IsNullOrEmpty())
                return BadRequest("Cannot delete department with active employees.");
            var result = await _departmentsService.DeleteDepartmentAsync(id);
            if (result == false)
                return NotFound("Department not found.");
            return NoContent();
        }
    }
}
