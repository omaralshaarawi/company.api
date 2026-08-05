using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace company.api.Services
{
    public class ReportsTypesService : IReportsTypesService
    {
        private readonly CompanyContext _context;

        public ReportsTypesService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ReportTypeDto> CreateReportTypeAsync(string TypeName)
        {
            var reportType = new ReportType
            {
                TypeName = TypeName
            };
            _context.ReportTypes.Add(reportType);
            await _context.SaveChangesAsync();
            return new ReportTypeDto ( reportType.ReportTypeId, reportType.TypeName );
        }

        public async Task<bool> DeleteReportTypeAsync(int id)
        {
            
            var reportType = await _context.ReportTypes.FindAsync(id);
            var reports = await _context.Reports.Where(r => r.ReportTypeId == id).ToListAsync();
            if (reportType == null || !reports.IsNullOrEmpty()) return false; 
            _context.ReportTypes.Remove(reportType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReportTypeDto?> GetReportTypeAsync(int id)
        {
            var reportType = await _context.ReportTypes.FindAsync(id);
            if (reportType == null) return null;
            return new ReportTypeDto(reportType.ReportTypeId, reportType.TypeName);
        }

        public async Task<List<ReportTypeDto>?> GetReportTypesAsync()
        {
            var reportTypes = await _context.ReportTypes.ToListAsync();
            return reportTypes.Select(rt => new ReportTypeDto(rt.ReportTypeId, rt.TypeName)).ToList();
        }

        public async Task<ReportTypeDto?> UpdateReportTypeAsync(int id,string TypeName)
        {
            var reportType = await _context.ReportTypes.FindAsync(id);
            var reports = await _context.Reports.Where(r => r.ReportTypeId == id).ToListAsync();
            if (reportType == null) return null;
            reportType.TypeName = TypeName;
            reports.ForEach(r => r.ReportType = reportType);
            _context.ReportTypes.Update(reportType);
            await _context.SaveChangesAsync();
            return new ReportTypeDto(reportType.ReportTypeId, reportType.TypeName);
        }
    }
}
