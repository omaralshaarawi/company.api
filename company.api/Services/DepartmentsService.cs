using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace company.api.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly CompanyContext _context;

        public DepartmentsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<DepartmentResponse?> AddDepartment(CreateDepartmentRequest req)
        {
            var department = new Department
            {
                DepartmentName = req.Name,
                CreatedAt = DateTime.UtcNow
            };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return new DepartmentResponse(department.DepartmentId, department.DepartmentName, department.CreatedAt);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department is null) return false;
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DepartmentResponse?> GetDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department is null) return null;
            return new DepartmentResponse(department.DepartmentId, department.DepartmentName, department.CreatedAt);
        
        }

        public async Task<List<DepartmentResponse>> GetDepartmentListAsync()
        {
            var query = _context.Departments.AsQueryable();
            var result = await query
            .Select(d => new DepartmentResponse(
            d.DepartmentId, d.DepartmentName, d.CreatedAt))
            .ToListAsync();
            return result;
        }

        public async Task<DepartmentResponse?> UpdateDepartmentAsync(int id, UpdateDepartmentRequest req)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department is null) return null;
            department.DepartmentName = req.Name;
            await _context.SaveChangesAsync();
            return new DepartmentResponse(department.DepartmentId, department.DepartmentName, department.CreatedAt);
        }
    }
}
