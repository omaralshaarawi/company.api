using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

[Index("SerialNumber", Name = "UQ_Assets_SerialNumber", IsUnique = true)]
public partial class Asset
{
    [Key]
    public int AssetId { get; set; }

    public int? AssetTypeId { get; set; }

    [StringLength(150)]
    public string AssetName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? SerialNumber { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal? PurchaseCost { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("AssetTypeId")]
    [InverseProperty("Assets")]
    public virtual AssetType? AssetType { get; set; }

    [InverseProperty("Asset")]
    public virtual ICollection<EmployeeAsset> EmployeeAssets { get; set; } = new List<EmployeeAsset>();

    [InverseProperty("RelatedAsset")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
