using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace company.api.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly CompanyContext _context;

        public AssetsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<AssetDto?> CreateAssetAsync(CreateAssetRequest createAssetRequest)
        {
            var assetType = await _context.AssetTypes.FindAsync(createAssetRequest.AssetTypeId);
            if (assetType is null)
            {
                return null;
            }

            var asset = new Asset
            {
                AssetName = createAssetRequest.AssetName,
                AssetTypeId = createAssetRequest.AssetTypeId,
                PurchaseCost = createAssetRequest.PurchaseCost,
                SerialNumber = createAssetRequest.SerialNumber,
                PurchaseDate = createAssetRequest.PurchaseDate
            };
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
            return new AssetDto(asset.AssetId, asset.AssetTypeId, asset.AssetName,
                asset.SerialNumber,asset.PurchaseDate, asset.PurchaseCost, asset.Status);
        
        }

        public async Task<bool> DeleteAssetAsync(int assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null)
            {
                return false;
            }
             _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AssetDto?> GetAssetAsync(int assetId)
        {   
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset is null)
            {
                return null;
            }
            return new AssetDto(asset.AssetId, asset.AssetTypeId, asset.AssetName,
                asset.SerialNumber, asset.PurchaseDate, asset.PurchaseCost, asset.Status);
        }

        public async Task<List<EmployeeAssetDto>> GetAssetHistoryAsync(int assetId)
        {
            var history = await _context.EmployeeAssets
                .Where(ea => ea.AssetId == assetId)
                .Select(ea => new EmployeeAssetDto(
                    ea.EmployeeAssetId,
                    ea.EmployeeId,
                    ea.AssetId,
                    ea.AssignedDate,
                    ea.ReturnedDate,
                    ea.Notes))
                .ToListAsync();
            return history;
        }

        public async Task<List<AssetDto>> GetAssetsAsync(int? assetTypeId, string? status)
        {
            var query = _context.Assets.AsQueryable();

            if (assetTypeId.HasValue)
            {
                query = query.Where(a => a.AssetTypeId == assetTypeId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var assets = await query
                .Select(a => new AssetDto(a.AssetId, a.AssetTypeId, a.AssetName,
                a.SerialNumber, a.PurchaseDate, a.PurchaseCost, a.Status))
                .ToListAsync();
            return assets;
        }

        public async Task<AssetDto?> UpdateAssetAsync(int assetId, AssetDto assetDto)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            var assetType = await _context.AssetTypes.FindAsync(assetDto.AssetTypeId);
            if (asset == null || assetType == null)
            {
                return null;
            }
            asset.AssetName = assetDto.AssetName;
            asset.AssetTypeId = assetDto.AssetTypeId;
            asset.SerialNumber = assetDto.SerialNumber;
            asset.PurchaseDate = assetDto.PurchaseDate;
            asset.PurchaseCost = assetDto.PurchaseCost;
            asset.Status = assetDto.Status;
            await _context.SaveChangesAsync();
            return new AssetDto(asset.AssetId, asset.AssetTypeId, asset.AssetName,
                asset.SerialNumber, asset.PurchaseDate, asset.PurchaseCost, asset.Status);
        }
    }
}
