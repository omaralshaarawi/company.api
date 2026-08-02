using company.api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse?> GetEmployeeAsync(int id);
        Task<List<EmployeeResponse>> GetEmployeeListAsync(int? DepartmentId, string? Status);
        Task<EmployeeResponse?> AddEmployee(CreateEmployeeRequest req);
        Task<EmployeeResponse?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest req);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}

