using company.api.Data;
using company.api.Dto;
using Microsoft.EntityFrameworkCore;
using company.api.Models;

namespace company.api.Services
{
    public class EmployeeAssetsService : IEmployeeAssetsService
    {
        private readonly CompanyContext _context;

        public EmployeeAssetsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<EmployeeAssetDto?> CreateEmployeeAssetsAsync(CreateEmployeeAssetRequest request)
        {
            var asset = await _context.Assets.FindAsync(request.AssetId);
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (asset == null || employee == null)
            {
                return null;
            }
            var employeeAsset = new EmployeeAsset
            {
                EmployeeId = request.EmployeeId,
                AssetId = request.AssetId,
                AssignedDate = request.AssignedDate,
                Notes = request.Notes
            };
            _context.EmployeeAssets.Add(employeeAsset);
            await _context.SaveChangesAsync();
            asset.Status = "Assigned";
            await _context.SaveChangesAsync();
            return new EmployeeAssetDto(
                employeeAsset.EmployeeAssetId,
                employeeAsset.EmployeeId,
                employeeAsset.AssetId,
                employeeAsset.AssignedDate,
                employeeAsset.ReturnedDate,
                employeeAsset.Notes
            );
        }

        public async Task<EmployeeAssetDto?> GetEmployeeAssetAsync(int employeeAssetId)
        {
            var employeeAsset = _context.EmployeeAssets.Find(employeeAssetId);
            if (employeeAsset == null) return null;
            return new EmployeeAssetDto(
                employeeAsset.EmployeeAssetId,
                employeeAsset.EmployeeId,
                employeeAsset.AssetId,
                employeeAsset.AssignedDate,
                employeeAsset.ReturnedDate,
                employeeAsset.Notes
            );
        }

        public async Task<List<EmployeeAssetDto>> GetEmployeeAssetsAsync(int? employeeId, int? assetId, bool? active)
        {
            var query = _context.EmployeeAssets.AsQueryable();
            if (employeeId.HasValue)
            {
                query = query.Where(ea => ea.EmployeeId == employeeId.Value);
            }
            if (assetId.HasValue)
            {
                query = query.Where(ea => ea.AssetId == assetId.Value);
            }
            if (active.HasValue)
            {
                query = query.Where(ea => ea.ReturnedDate == null);
            }
            var employeeAssets = await query.ToListAsync();
            return employeeAssets.Select(ea => new EmployeeAssetDto(
                ea.EmployeeAssetId,
                ea.EmployeeId,
                ea.AssetId,
                ea.AssignedDate,
                ea.ReturnedDate,
                ea.Notes
            )).ToList();    
        }

        public async Task<EmployeeAssetDto?> UpdateEmployeeAssetsAsync(int employeeAssetId, string? Notes)
        {
            var employeeAsset = await _context.EmployeeAssets.FindAsync(employeeAssetId);
            if (employeeAsset == null) return null;
            if(Notes != null)
            {
                employeeAsset.Notes = Notes;
            }
            employeeAsset.ReturnedDate = DateOnly.FromDateTime(DateTime.Now);
            await _context.SaveChangesAsync();
            return new EmployeeAssetDto(
                employeeAsset.EmployeeAssetId,
                employeeAsset.EmployeeId,
                employeeAsset.AssetId,
                employeeAsset.AssignedDate,
                employeeAsset.ReturnedDate,
                employeeAsset.Notes
            );
        }
    }
}
