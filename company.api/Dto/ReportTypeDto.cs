using System.ComponentModel.DataAnnotations;


namespace company.api.Dto
{
    public record ReportTypeDto(
       [Required] int ReportTypeId,
        [Required] string ReportTypeName
    );
}
