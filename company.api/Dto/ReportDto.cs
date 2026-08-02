namespace company.api.Dto
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public int? ReportTypeId { get; set; }
        public string? ReportTypeName { get; set; } 
        public string Title { get; set; } = null!;

        public int? GeneratedById { get; set; }
        public string? GeneratedByName { get; set; } 

        public int? RelatedEmployeeId { get; set; }
        public string? RelatedEmployeeName { get; set; } 

        public int? RelatedAssetId { get; set; }
        public string? RelatedAssetName { get; set; } 

        public string? Summary { get; set; }
        public DateTime? GeneratedDate { get; set; }
    }
}
