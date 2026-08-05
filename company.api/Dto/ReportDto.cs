using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record ReportDto(
        int ReportId,
        int? ReportTypeId,
        string Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId,
        string? Summary, 
        DateTime? CreatedAt
    );

    public record ReportDtoResponse(
        int ReportId,
        string? employeeName,
        string? ReportTypeName,
        string Title,
        string? summary,
        DateTime? CreatedAt
    );

    public record CreateReportRequest(
        int? ReportTypeId,
        string Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId,
        string? Summary
    );

    public record UpdateReportRequest(
        int? ReportTypeId,
        string? Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId,
        string? Summary,
        DateTime? CreatedAt
    );
}
