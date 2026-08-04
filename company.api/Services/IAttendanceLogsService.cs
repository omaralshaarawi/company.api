using company.api.Dto;

namespace company.api.Services
{
    public interface IAttendanceLogsService
    {
        Task<List<AttendanceLogsDto>?> GetAttendanceLogsAsync(int? employeeId, DateTime? startDate, DateTime? endDate);
        
        Task<List<AttendanceLogsDto>?> GetAttendanceLogAsyncById(int attendanceLogId);

        Task<AttendanceLogsDto?> CreateAttendanceLogAsync(CreateAttendanceLogsRequest createAttendanceLogRequest);
    }
}
