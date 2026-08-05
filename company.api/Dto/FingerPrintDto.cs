using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record FingerprintDto(
       [Required] int FingerprintId,
        [Required] int EmployeeId,
        [Required] string FingerIndex, 
       [MaxLength(150)] string? DeviceId,
        DateTime? EnrolledDate,
        byte? Quality
    );
    public record CreateFingerprintRequest(
        [Required] int EmployeeId,
        [Required] string FingerIndex,
        [MaxLength(150)] string? DeviceId,
        [Required] string  TemplateData,
        DateTime? EnrolledDate,
        [MaxLength(150)] string? Quality
    );
}
