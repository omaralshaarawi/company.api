using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace company.api.Models;

[Index("Email", Name = "UQ_Employees_Email", IsUnique = true)]
[Index("NationalId", Name = "UQ_Employees_NationalId", IsUnique = true)]
public partial class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? NationalId { get; set; }

    public int? DepartmentId { get; set; }

    [StringLength(100)]
    public string? Position { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Phone { get; set; }

    public DateOnly? HireDate { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("Employees")]
    public virtual Department? Department { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeAsset> EmployeeAssets { get; set; } = new List<EmployeeAsset>();

    [InverseProperty("Employee")]
    public virtual ICollection<Fingerprint> Fingerprints { get; set; } = new List<Fingerprint>();

    [InverseProperty("GeneratedBy")]
    public virtual ICollection<Report> ReportGeneratedBies { get; set; } = new List<Report>();

    [InverseProperty("RelatedEmployee")]
    public virtual ICollection<Report> ReportRelatedEmployees { get; set; } = new List<Report>();
}
