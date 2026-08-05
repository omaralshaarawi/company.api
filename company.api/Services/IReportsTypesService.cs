using company.api.Dto;

namespace company.api.Services
{
    public interface IReportsTypesService
    {
        Task<List<ReportTypeDto>?> GetReportTypesAsync();

        Task<ReportTypeDto?> GetReportTypeAsync(int id);

        Task<ReportTypeDto> CreateReportTypeAsync(string TypeName);

        Task<ReportTypeDto?> UpdateReportTypeAsync(int id, string TypeName);

        Task<bool> DeleteReportTypeAsync(int id);
    }
}
