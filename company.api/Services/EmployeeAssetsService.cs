using company.api.Data;
using company.api.Dto;
using company.api.Hubs;
using company.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace company.api.Services
{
    public class EmployeeAssetsService : IEmployeeAssetsService
    {
        private readonly CompanyContext _context;
        private readonly IHubContext<NotificationsHub> _hub;
        public EmployeeAssetsService(CompanyContext context, IHubContext<NotificationsHub> hub)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hub = hub;
        }
        public async Task<EmployeeAssetDto?> CreateEmployeeAssetsAsync(CreateEmployeeAssetRequest request)
        {
            var asset = await _context.Assets.FindAsync(request.AssetId);
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (asset == null || employee == null)
            {
                return null;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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

                await transaction.CommitAsync();

                await _hub.Clients.All.SendAsync("AssetAssigned", new
                {
                    employeeAsset.AssetId,
                    employeeAsset.EmployeeId,
                    employeeAsset.AssignedDate
                });

                return new EmployeeAssetDto(
                    employeeAsset.EmployeeAssetId,
                    employeeAsset.EmployeeId,
                    employeeAsset.AssetId,
                    employeeAsset.AssignedDate,
                    employeeAsset.ReturnedDate,
                    employeeAsset.Notes
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EmployeeAssetDto?> GetEmployeeAssetAsync(int employeeAssetId)
        {
            var employeeAsset = await _context.EmployeeAssets.FindAsync(employeeAssetId);
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

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (Notes != null)
                {
                    employeeAsset.Notes = Notes;
                }

                employeeAsset.ReturnedDate = DateOnly.FromDateTime(DateTime.UtcNow);

                var asset = await _context.Assets.FindAsync(employeeAsset.AssetId);
                if (asset != null)
                {
                    asset.Status = "InStock";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await _hub.Clients.All.SendAsync("AssetReturned", new
                {
                    employeeAsset.AssetId,
                    employeeAsset.EmployeeId,
                    employeeAsset.ReturnedDate
                });
                Log.Information("Broadcasted AssetReturned: AssetId={AssetId} EmployeeId={EmployeeId}", employeeAsset.AssetId, employeeAsset.EmployeeId);
                return new EmployeeAssetDto(
                    employeeAsset.EmployeeAssetId,
                    employeeAsset.EmployeeId,
                    employeeAsset.AssetId,
                    employeeAsset.AssignedDate,
                    employeeAsset.ReturnedDate,
                    employeeAsset.Notes
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
