using company.api.Dto;

namespace company.api.Services
{
    public interface IEmployeeAssetsService
    {
        Task<List<EmployeeAssetDto>> GetEmployeeAssetsAsync(int? employeeId,int? assetId,bool? active);
        Task<EmployeeAssetDto?> GetEmployeeAssetAsync(int employeeAssetId);
        Task<EmployeeAssetDto?> CreateEmployeeAssetsAsync(CreateEmployeeAssetRequest request);
        Task<EmployeeAssetDto?> UpdateEmployeeAssetsAsync(int employeeAssetId, String? Notes);

    }
}
