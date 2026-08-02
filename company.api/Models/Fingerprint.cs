using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

[Index("EmployeeId", "FingerIndex", Name = "UQ_Employee_Finger", IsUnique = true)]
public partial class Fingerprint
{
    [Key]
    public int FingerprintId { get; set; }

    public int EmployeeId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string FingerIndex { get; set; } = null!;

    public byte[] TemplateData { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? DeviceId { get; set; }

    public DateTime? EnrolledDate { get; set; }

    public byte? Quality { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Fingerprints")]
    public virtual Employee Employee { get; set; } = null!;
}
