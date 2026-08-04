namespace company.api.Dto
{
    public record EmployeeAssetDto(
        int EmployeeAssetId,
        int EmployeeId,
        int AssetId,
        DateOnly? AssignedDate,
        DateOnly? ReturnedDate,
        string? Notes
    );
    public record CreateEmployeeAssetRequest(
       int EmployeeId,
       int AssetId,
       DateOnly? AssignedDate,
       DateOnly? ReturnDate,
       string? Notes
   );


}
