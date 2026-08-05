using System.ComponentModel.DataAnnotations;


namespace company.api.Dto
{
    public record EmployeeAssetDto(
       [Required] int EmployeeAssetId,
        [Required] int EmployeeId,
        [Required] int AssetId,
        DateOnly? AssignedDate,
        DateOnly? ReturnedDate,
        [MaxLength(250)] string? Notes
    );
    public record CreateEmployeeAssetRequest(
       [Required] int EmployeeId,
       [Required] int AssetId,
       DateOnly? AssignedDate,
       DateOnly? ReturnDate,
       [MaxLength(250)] string? Notes
   );


}
