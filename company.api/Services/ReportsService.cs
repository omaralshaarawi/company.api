using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace company.api.Services
{
    public class ReportsService : IReportsService
    {
        private readonly CompanyContext _context;

        public ReportsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ReportDto?> CreateReportAsync(CreateReportRequest createReportRequest)
        {
            var reportType = await _context.ReportTypes.FindAsync(createReportRequest.ReportTypeId);
            if(createReportRequest.GeneratedById != null)
            {
                var generatedBy = await _context.Employees.FindAsync(createReportRequest.GeneratedById);
                if (generatedBy == null) return null;
            }
            if (createReportRequest.RelatedEmployeeId != null)
            {
                var relatedEmployee = await _context.Employees.FindAsync(createReportRequest.RelatedEmployeeId);
                if (relatedEmployee == null) return null;
            }
            if (createReportRequest.RelatedAssetId != null)
            {
                var relatedAsset = await _context.Assets.FindAsync(createReportRequest.RelatedAssetId);
                if (relatedAsset == null) return null;
            }
            if (reportType == null) return null;
            var report = new Report
            {
                ReportTypeId = createReportRequest.ReportTypeId,
                Title = createReportRequest.Title,
                GeneratedById = createReportRequest.GeneratedById,
                RelatedEmployeeId = createReportRequest.RelatedEmployeeId,
                RelatedAssetId = createReportRequest.RelatedAssetId,
                Summary = createReportRequest.Summary,
                GeneratedDate = DateTime.UtcNow
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return new ReportDto(

                report.ReportId,
                report.ReportTypeId,
                report.Title,
                report.GeneratedById,
                report.RelatedEmployeeId,
                report.RelatedAssetId,
                report.Summary,
                report.GeneratedDate
           );
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return false;
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReportDtoResponse>> GetReportsAsync(int? employeeId, int? assetId, int? reportTypeId)
        {
            var query = _context.Reports.AsQueryable();
            if (employeeId.HasValue)
            {
                query = query.Where(r => r.RelatedEmployeeId == employeeId.Value);
            }
            if (assetId.HasValue)
            {
                query = query.Where(r => r.RelatedAssetId == assetId.Value);
            }
            if (reportTypeId.HasValue)
            {
                query = query.Where(r => r.ReportTypeId == reportTypeId.Value);
            }
            var reports = await query.ToListAsync();
            return reports.Select( r => new ReportDtoResponse(
                r.ReportId,
                _context.Employees
                .Where(e => e.EmployeeId == r.GeneratedById)
                .Select(e => e.FullName)
                .FirstOrDefault(),
                _context.ReportTypes
                .Where(rt => rt.ReportTypeId == r.ReportTypeId)
                .Select(rt => rt.TypeName)
                .FirstOrDefault(),
                r.Title,
                r.Summary,
                r.GeneratedDate
            )).ToList();
        }

        public async Task<ReportDto?> GetReportsAsyncByReportId(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return null;
            return new ReportDto(
                report.ReportId,
                report.ReportTypeId,
                report.Title,
                report.GeneratedById,
                report.RelatedEmployeeId,
                report.RelatedAssetId,
                report.Summary,
                report.GeneratedDate
            );
        }

        public async Task<ReportDto?> UpdateReportAsync(int id,UpdateReportRequest updateReportRequest)
        {
            var report = await _context.Reports.FindAsync(id);
            if (updateReportRequest.GeneratedById != null)
            {
                var generatedBy = await _context.Employees.FindAsync(updateReportRequest.GeneratedById);
                if (generatedBy == null) return null;
            }
            if (updateReportRequest.RelatedEmployeeId != null)
            {
                var relatedEmployee = await _context.Employees.FindAsync(updateReportRequest.RelatedEmployeeId);
                if (relatedEmployee == null) return null;
            }
            if (updateReportRequest.RelatedAssetId != null)
            {
                var relatedAsset = await _context.Assets.FindAsync(updateReportRequest.RelatedAssetId);
                if (relatedAsset == null) return null;
            }
            if (report == null) return null;
            report.ReportTypeId = updateReportRequest.ReportTypeId ?? report.ReportTypeId;
            report.Title = updateReportRequest.Title ?? report.Title;
            report.GeneratedById = updateReportRequest.GeneratedById ?? report.GeneratedById;
            report.RelatedEmployeeId = updateReportRequest.RelatedEmployeeId ?? report.RelatedEmployeeId;
            report.RelatedAssetId = updateReportRequest.RelatedAssetId ?? report.RelatedAssetId;
            report.Summary = updateReportRequest.Summary ?? report.Summary;
            report.GeneratedDate = updateReportRequest.CreatedAt ?? report.GeneratedDate;
            await _context.SaveChangesAsync();
            return new ReportDto(
                report.ReportId,
                report.ReportTypeId,
                report.Title,
                report.GeneratedById,
                report.RelatedEmployeeId,
                report.RelatedAssetId,
                report.Summary,
                report.GeneratedDate
            );
        }
    }
}
