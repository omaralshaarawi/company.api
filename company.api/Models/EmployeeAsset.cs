using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

public partial class EmployeeAsset
{
    [Key]
    public int EmployeeAssetId { get; set; }

    public int EmployeeId { get; set; }

    public int AssetId { get; set; }

    public DateOnly? AssignedDate { get; set; }

    public DateOnly? ReturnedDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("AssetId")]
    [InverseProperty("EmployeeAssets")]
    public virtual Asset Asset { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeAssets")]
    public virtual Employee Employee { get; set; } = null!;
}
