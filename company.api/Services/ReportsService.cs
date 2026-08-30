using company.api.Data;
using company.api.Dto;
using company.api.Hubs;
using company.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace company.api.Services
{
    public class ReportsService : IReportsService
    {
        private readonly CompanyContext _context;
        private readonly IAttendanceLogsService _attendanceLogsService;
        private readonly IAssetsService _assetsService;
        private readonly IAssetTypesService _assetTypesService;
        private readonly IEmployeeAssetsService _employeeAssetsService;
        private readonly IHubContext<NotificationsHub> _hub;

        public ReportsService(CompanyContext context, IAttendanceLogsService attendanceLogsService, IAssetsService assetsService, IAssetTypesService assetTypesService,IEmployeeAssetsService employeeAssetsService, IHubContext<NotificationsHub> hub)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _attendanceLogsService = attendanceLogsService ?? throw new ArgumentNullException(nameof(attendanceLogsService));
            _assetsService = assetsService ?? throw new ArgumentNullException(nameof(assetsService));
            _assetTypesService = assetTypesService ?? throw new ArgumentNullException(nameof(assetTypesService));
            _employeeAssetsService = employeeAssetsService ?? throw new ArgumentException(nameof(employeeAssetsService));
            _hub = hub;
        }
        public async Task<ReportDto?> CreateReportAsync(CreateReportRequest createReportRequest)
        {
            var reportType = await _context.ReportTypes.FindAsync(createReportRequest.ReportTypeId);
            Employee? relatedEmployee = null;
            if (createReportRequest.RelatedEmployeeId != null)
            {
                relatedEmployee = await _context.Employees.FindAsync(createReportRequest.RelatedEmployeeId);
                if (relatedEmployee == null) return null;
            }
            if (createReportRequest.RelatedAssetId != null)
            {
                var relatedAsset = await _context.Assets.FindAsync(createReportRequest.RelatedAssetId);
                if (relatedAsset == null) return null;
            }
            if (reportType == null) return null;

            if (reportType.ReportTypeId == 1003)
            {
                    DateTime toDate = DateTime.Today;
                    DateTime fromDate = toDate.AddDays(-30);
                    var attendanceLogs = await _attendanceLogsService.GetAttendanceLogsAsync(null, fromDate, toDate);
                    var numberOfEmployees = attendanceLogs.Select(log => log.EmployeeId).Distinct().Count();
                    var numberOfCheckIns = attendanceLogs.Count(log => log.EventType == "CheckIn");
                    var numberOfCheckOuts = attendanceLogs.Count(log => log.EventType == "CheckOut");
                    var summary = "Covers all attendance events recorded across the Engineering and Operations departments for last 30 days." + numberOfEmployees + " Employees  logged a total of" + numberOfCheckIns + " check-ins and " + numberOfCheckOuts + " check-outs,So " + (numberOfCheckOuts - numberOfCheckIns) + " unresolved missing check-outs were found in this period.";
                    var report = new Report
                    {
                        ReportTypeId = createReportRequest.ReportTypeId,
                        Title = createReportRequest.Title,
                        GeneratedById = createReportRequest.GeneratedById,
                        RelatedEmployeeId = createReportRequest.RelatedEmployeeId,
                        RelatedAssetId = createReportRequest.RelatedAssetId,
                        Summary = summary,
                        GeneratedDate = DateTime.UtcNow
                        
                    };
                    _context.Reports.Add(report);
                    await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("ReportGenerated", new
                {
                    reportId = report.ReportId,
                    title = report.Title,
                    relatedEmployeeId = report.RelatedEmployeeId
                });
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
                else if(reportType.ReportTypeId == 1004)
                {
                    var assets = await _assetsService.GetAssetsAsync(null,null);
                    var assetsInStockOrAssigned = assets.Count(a => a.Status == "Instock" || a.Status == "Assigned");
                    var summary = "Quarterly audit of " + assets.Count+ ", ";
                    var assetTypes = await _assetTypesService.GetAssetTypesAsync();
                    foreach(AssetTypeDto assetType in assetTypes)
                    {
                        var count= assets.Count(a => a.AssetTypeId == assetType.AssetTypeId);
                        summary += count + " " + assetType.TypeName + ", ";
                    }
                    summary += assetsInStockOrAssigned + " Assets InStock or Assigned.";
                    var report = new Report
                    {
                         ReportTypeId = createReportRequest.ReportTypeId,
                         Title = createReportRequest.Title,
                         GeneratedById = createReportRequest.GeneratedById,
                         RelatedEmployeeId = createReportRequest.RelatedEmployeeId,
                         RelatedAssetId = createReportRequest.RelatedAssetId,
                         Summary = summary,
                         GeneratedDate = DateTime.UtcNow

                    };
                    _context.Reports.Add(report);
                    await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("ReportGenerated", new
                {
                    reportId = report.ReportId,
                    title = report.Title,
                    relatedEmployeeId = report.RelatedEmployeeId
                });
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
                else
                {
                    if (relatedEmployee == null) return null;
                    DateTime toDate = DateTime.Today.AddDays(1);
                    DateTime fromDate = toDate.AddDays(-30);
                    var attendanceLogs = await _attendanceLogsService.GetAttendanceLogsAsync(createReportRequest.RelatedEmployeeId, fromDate, toDate);
                    var assets = await _assetsService.GetAssetsAsync(null, null);
                    var countAssigned = assets.Count(a => a.Status == "Assigned");
                    var returned = await _employeeAssetsService.GetEmployeeAssetsAsync(createReportRequest.RelatedEmployeeId, null, false);
                    var summary = "Activity report,In the last 30 days "+ (attendanceLogs?.Count ?? 0) + " attendance events, "+ countAssigned+" Assets Currently assigned and "+ returned.Count + " Completed asset return.";
                    var report = new Report
                    {
                        ReportTypeId = createReportRequest.ReportTypeId,
                        Title = createReportRequest.Title,
                        GeneratedById = createReportRequest.GeneratedById,
                        RelatedEmployeeId = createReportRequest.RelatedEmployeeId,
                        RelatedAssetId = createReportRequest.RelatedAssetId,
                        Summary = summary,
                        GeneratedDate = DateTime.UtcNow

                    };
                    _context.Reports.Add(report);
                    await _context.SaveChangesAsync();
                    await _hub.Clients.All.SendAsync("ReportGenerated", new
                    {
                        reportId = report.ReportId,
                        title = report.Title,
                        relatedEmployeeId = report.RelatedEmployeeId
                    });
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
