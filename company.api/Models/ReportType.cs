using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

public partial class ReportType
{
    [Key]
    public int ReportTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("ReportType")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
