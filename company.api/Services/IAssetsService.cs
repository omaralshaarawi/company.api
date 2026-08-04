using company.api.Dto;

namespace company.api.Services
{
    public interface IAssetsService
    {
        Task<List<AssetDto>> GetAssetsAsync(int? assetTypeId, string? status);
        Task<AssetDto?> GetAssetAsync(int assetId);
        Task<AssetDto?> CreateAssetAsync(CreateAssetRequest createAssetRequest);
        Task<AssetDto?> UpdateAssetAsync(int assetId, AssetDto assetDto);
        Task<bool> DeleteAssetAsync(int assetId);
        Task<List<EmployeeAssetDto>> GetAssetHistoryAsync(int assetId);
    }
}
