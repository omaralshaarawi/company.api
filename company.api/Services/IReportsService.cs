using company.api.Dto;


namespace company.api.Services
{
    public interface IReportsService
    {
        Task<List<ReportDtoResponse>> GetReportsAsync(int? employeeId,int? assetId,int? reportTypeId);
        Task<ReportDto?> GetReportsAsyncByReportId(int reportId);
        Task<ReportDto?> CreateReportAsync(CreateReportRequest createReportRequest);
        Task<ReportDto?> UpdateReportAsync(int id, UpdateReportRequest updateReportRequest);
        Task<bool> DeleteReportAsync(int reportId);
    }
}
