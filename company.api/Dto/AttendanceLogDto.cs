using System;

namespace company.api.Dto
{
    public record CreateAttendanceLogsRequest(
        int EmployeeId,
        string? DeviceId,
        string eventType
    );

    public record AttendanceLogsDto(
        int AttendanceLogId,
        int EmployeeId,
        string? DeviceId,
        string EventType,
        DateTime? EventTime
    );

}
