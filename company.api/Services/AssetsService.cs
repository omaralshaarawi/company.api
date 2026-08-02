using company.api.Dto;

namespace company.api.Services
{
    public class AssetsService : IAssetsService
    {
        public Task<AssetDto> CreateAssetAsync(AssetDto assetDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAssetAsync(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<AssetDto> GetAssetAsync(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AssetDto>> GetAssetsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AssetDto> UpdateAssetAsync(int assetId, AssetDto assetDto)
        {
            throw new NotImplementedException();
        }
    }
}
