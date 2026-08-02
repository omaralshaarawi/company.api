using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record DepartmentResponse(
     int DepartmentId, string Name,
     DateTime? CreatedAt);
    public record CreateDepartmentRequest(
    [Required][MaxLength(150)] string Name);
    public record UpdateDepartmentRequest(
    [Required][MaxLength(150)] string Name
    );

}
