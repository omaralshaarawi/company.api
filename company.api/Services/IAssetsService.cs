using company.api.Dto;

namespace company.api.Services
{
    public interface IAssetsService
    {
        Task<List<AssetDto>> GetAssetsAsync();
        Task<AssetDto> GetAssetAsync(int assetId);
        Task<AssetDto> CreateAssetAsync(AssetDto assetDto);
        Task<AssetDto> UpdateAssetAsync(int assetId, AssetDto assetDto);
        Task<bool> DeleteAssetAsync(int assetId);
    }
}
