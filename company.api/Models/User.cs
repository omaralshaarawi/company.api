using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace company.api.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    public int? EmployeeId { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "User";

    // Navigation property back to Employee
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}