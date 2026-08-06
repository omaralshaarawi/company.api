using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace company.api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CompanyContext _context;

        public EmployeeService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<EmployeeResponse?> AddEmployee(CreateEmployeeRequest req)
        {
            var employee = new Employee
            {
                FullName = req.FullName,
                NationalId = req.NationalId,
                DepartmentId = req.DepartmentId,
                Position = req.Position,
                Email = req.Email,
                Phone = req.Phone,
                HireDate = req.HireDate,
                Status = "Active"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return new EmployeeResponse(employee.EmployeeId, employee.FullName, employee.Position,
            employee.Email, employee.Phone, employee.Status, null);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null) return false;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EmployeeResponse?> GetEmployeeAsync(int id)
        {
            var e = await _context.Employees.Include(x => x.Department)
  .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (e is null) return null;
            return new EmployeeResponse(
            e.EmployeeId, e.FullName, e.Position, e.Email, e.Phone,
            e.Status, e.Department?.DepartmentName);
        }

        public async Task<PagedResult<EmployeeResponse>> GetEmployeeListAsync(int? DepartmentId, string? Status, int pageNumber, int pageSize)
        {
            var query = _context.Employees.Include(e => e.Department).AsQueryable();
            if (DepartmentId is not null)
                query = query.Where(e => e.DepartmentId == DepartmentId);
            if (!string.IsNullOrEmpty(Status))
                query = query.Where(e => e.Status == Status);
            var totalCount = await query.CountAsync();
            var result = await query
            .OrderBy(e => e.EmployeeId) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EmployeeResponse(
            e.EmployeeId, e.FullName, e.Position, e.Email, e.Phone,
            e.Status, e.Department != null ? e.Department.DepartmentName : null))
            .ToListAsync();
            return new PagedResult<EmployeeResponse>
            {
                Items = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EmployeeResponse?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest req)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null) return null;
            employee.FullName = req.FullName;
            employee.DepartmentId = req.DepartmentId;
            employee.Position = req.Position;
            employee.Email = req.Email;
            employee.Phone = req.Phone;
            employee.Status = req.Status;
            await _context.SaveChangesAsync();
            return new EmployeeResponse(employee.EmployeeId, employee.FullName, employee.Position,
            employee.Email, employee.Phone, employee.Status, null);
        }
    }
}
