using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

public partial class AssetType
{
    [Key]
    public int AssetTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("AssetType")]
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
