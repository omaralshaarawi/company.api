using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

public partial class Report
{
    [Key]
    public int ReportId { get; set; }

    public int? ReportTypeId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public int? GeneratedById { get; set; }

    public int? RelatedEmployeeId { get; set; }

    public int? RelatedAssetId { get; set; }

    [StringLength(2000)]
    public string? Summary { get; set; }

    public DateTime? GeneratedDate { get; set; }

    [ForeignKey("GeneratedById")]
    [InverseProperty("ReportGeneratedBies")]
    public virtual Employee? GeneratedBy { get; set; }

    [ForeignKey("RelatedAssetId")]
    [InverseProperty("Reports")]
    public virtual Asset? RelatedAsset { get; set; }

    [ForeignKey("RelatedEmployeeId")]
    [InverseProperty("ReportRelatedEmployees")]
    public virtual Employee? RelatedEmployee { get; set; }

    [ForeignKey("ReportTypeId")]
    [InverseProperty("Reports")]
    public virtual ReportType? ReportType { get; set; }
}
