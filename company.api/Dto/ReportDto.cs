using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record ReportDto(
       [Required] int ReportId,
        int? ReportTypeId,
        [Required] string Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId,
       [MaxLength(250)] string? Summary, 
        DateTime? CreatedAt
    );

    public record ReportDtoResponse(
       [Required] int ReportId,
        string? employeeName,
        string? ReportTypeName,
        [Required] string Title,
        [MaxLength(250)] string? summary,
        DateTime? CreatedAt
    );

    public record CreateReportRequest(
        int? ReportTypeId,
        [Required] string Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId
    );

    public record UpdateReportRequest(
        int? ReportTypeId,
        [MaxLength(250)] string? Title,
        int? GeneratedById,
        int? RelatedEmployeeId,
        int? RelatedAssetId,
        [MaxLength(250)] string? Summary,
        DateTime? CreatedAt
    );
}
