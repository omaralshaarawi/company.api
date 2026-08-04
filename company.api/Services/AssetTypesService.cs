using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using company.api.Data;
using company.api.Dto;
using company.api.Models;

namespace company.api.Services
{
    public class AssetTypesService : IAssetTypesService
    {
        private readonly CompanyContext _context;

        public AssetTypesService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<AssetTypeDto> CreateAssetTypeAsync(string assetTypeName)
        {
            var assetType = new AssetType
            {
                TypeName = assetTypeName
            };
            _context.AssetTypes.Add(assetType);
            await _context.SaveChangesAsync();
            return new AssetTypeDto
            {
                AssetTypeId = assetType.AssetTypeId,
                TypeName = assetType.TypeName
            };
        }

        public async Task<bool> DeleteAssetTypeAsync(int assetTypeId)
        {
            var assetType = _context.AssetTypes.Find(assetTypeId);
            if (assetType is null) return false;
            _context.AssetTypes.Remove(assetType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AssetTypeDto?> GetAssetTypeAsync(int assetTypeId)
        {
            var assetType = await _context.AssetTypes.FindAsync(assetTypeId);
            if (assetType is null) return null;
            return new AssetTypeDto
            {
                AssetTypeId = assetType.AssetTypeId,
                TypeName = assetType.TypeName
            };
      
        }

        public async Task<List<AssetTypeDto>> GetAssetTypesAsync()
        {
            var assetTypes = await _context.AssetTypes.ToListAsync();
            return assetTypes.Select(at => new AssetTypeDto
            {
                AssetTypeId = at.AssetTypeId,
                TypeName = at.TypeName
            }).ToList();
        }

        public async Task<AssetTypeDto?> UpdateAssetTypeAsync(int assetTypeId, string assetTypeName)
        {
            var assetType = await _context.AssetTypes.FindAsync(assetTypeId);
            if (assetType is null) return null;

            assetType.TypeName = assetTypeName;
            _context.AssetTypes.Update(assetType);
            await _context.SaveChangesAsync();

            return new AssetTypeDto
            {
                AssetTypeId = assetType.AssetTypeId,
                TypeName = assetType.TypeName
            };
        }
    }
}
