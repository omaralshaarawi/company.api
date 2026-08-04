using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.EntityFrameworkCore;

namespace company.api.Services
{
    public class AttendanceLogsService : IAttendanceLogsService
    {
        private readonly CompanyContext _context;

        public AttendanceLogsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<AttendanceLogsDto?> CreateAttendanceLogAsync(CreateAttendanceLogsRequest createAttendanceLogRequest)
        {
            if(createAttendanceLogRequest.eventType != "CheckIn" && createAttendanceLogRequest.eventType != "CheckOut")
            {
                return null;
            }
            var attendanceLog = new AttendanceLog
            {
                EmployeeId = createAttendanceLogRequest.EmployeeId,
                DeviceId = createAttendanceLogRequest.DeviceId,
                EventType = createAttendanceLogRequest.eventType,
                EventTime = DateTime.UtcNow
            };
            _context.AttendanceLogs.Add(attendanceLog);
            await _context.SaveChangesAsync();
            return new AttendanceLogsDto(
                attendanceLog.LogId,
                attendanceLog.EmployeeId,
                attendanceLog.DeviceId,
                attendanceLog.EventType,
                attendanceLog.EventTime
            );
        }

        public async Task<List<AttendanceLogsDto>?> GetAttendanceLogAsyncById(int attendanceLogId)
        {
            var attendanceLog = await _context.AttendanceLogs.FindAsync(attendanceLogId);
            if (attendanceLog == null)
            {
                return null;
            }
           return new List<AttendanceLogsDto>
            {
                new AttendanceLogsDto(
                    attendanceLog.LogId,
                    attendanceLog.EmployeeId,
                    attendanceLog.DeviceId,
                    attendanceLog.EventType,
                    attendanceLog.EventTime
                )
            };
        }

        public async Task<List<AttendanceLogsDto>?> GetAttendanceLogsAsync(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.AttendanceLogs.AsQueryable();
            if (employeeId.HasValue)
            {
                query = query.Where(log => log.EmployeeId == employeeId.Value);
            }
            if (startDate.HasValue)
            {
                query = query.Where(log => log.EventTime >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(log => log.EventTime <= endDate.Value);
            }
            var attendanceLogs = await query.ToListAsync();
            return attendanceLogs.Select(log => new AttendanceLogsDto(
                log.LogId,
                log.EmployeeId,
                log.DeviceId,
                log.EventType,
                log.EventTime
            )).ToList();
        }
    }
}
