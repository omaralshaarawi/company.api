using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

public partial class AttendanceLog
{
    [Key]
    public int LogId { get; set; }

    public int EmployeeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeviceId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string EventType { get; set; } = null!;

    public DateTime? EventTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("AttendanceLogs")]
    public virtual Employee Employee { get; set; } = null!;
}
