namespace company.api.Dto
{
    public class EmployeeAssetDto
    {
        public int EmployeeAssetId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; } 
        public int AssetId { get; set; }
        public string? AssetName { get; set; } 
        public DateOnly? AssignedDate { get; set; }
        public DateOnly? ReturnedDate { get; set; }
        public string? Notes { get; set; }
    }
}
