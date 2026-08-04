
namespace company.api.Dto
{
    public record FingerprintDto(
        int FingerprintId,
        int EmployeeId,
        string FingerIndex, 
        string? DeviceId,
        DateTime? EnrolledDate,
        byte? Quality
    );
    public record CreateFingerprintRequest(
        int EmployeeId,
        string FingerIndex,
        string? DeviceId,
        string  TemplateData,
        DateTime? EnrolledDate,
        string? Quality
    );
}
