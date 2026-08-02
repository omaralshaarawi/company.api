using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record EmployeeResponse(
    int EmployeeId, string FullName, string? Position,
    string? Email, string? Phone, string Status, string? DepartmentName);
    public record CreateEmployeeRequest(
    [ Required] [MaxLength(150)] string FullName,
    string? NationalId,
    int? DepartmentId,
    string? Position,
    [EmailAddress] string? Email,
    string? Phone,
    DateOnly? HireDate);
    public record UpdateEmployeeRequest(
    [ Required] [MaxLength(150)] string FullName,
    int? DepartmentId,
    string? Position,
    [EmailAddress] string? Email,
    string? Phone,
    [Required] string Status);
}
