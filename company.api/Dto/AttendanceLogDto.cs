using System;
using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record CreateAttendanceLogsRequest(
        [Required]int EmployeeId,
        string? DeviceId,
        [Required] string eventType
    );

    public record AttendanceLogsDto(
        [Required] int AttendanceLogId,
        [Required] int EmployeeId,
        string? DeviceId,
        [Required] [MaxLength(150)] string EventType,
        DateTime? EventTime
    );

}
