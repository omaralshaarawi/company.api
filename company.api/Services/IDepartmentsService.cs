using company.api.Dto;

namespace company.api.Services
{
    public interface IDepartmentsService
    {
        Task<DepartmentResponse?> GetDepartmentAsync(int id);
        Task<List<DepartmentResponse>> GetDepartmentListAsync();
        Task<DepartmentResponse?> AddDepartment(CreateDepartmentRequest req);
        Task<DepartmentResponse?> UpdateDepartmentAsync(int id, UpdateDepartmentRequest req);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
