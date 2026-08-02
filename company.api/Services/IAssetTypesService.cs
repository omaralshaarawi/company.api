using company.api.Dto;

namespace company.api.Services
{
    public interface IAssetTypesService
    {
        Task<List<AssetTypeDto>> GetAssetTypesAsync();

        Task<AssetTypeDto> GetAssetTypeAsync(int assetTypeId);

        Task<AssetTypeDto> CreateAssetTypeAsync(string assetTypeName);

        Task<AssetTypeDto> UpdateAssetTypeAsync(int assetTypeId, string assetTypeName);
     
        Task<bool> DeleteAssetTypeAsync(int assetTypeId);
    }
}
