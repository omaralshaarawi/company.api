using System;

namespace company.api.Dto
{
    public class AttendanceLogDto
    {
        public int LogId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; } 
        public string? DeviceId { get; set; }
        public string EventType { get; set; } = null!;
        public DateTime? EventTime { get; set; }
    }


}
