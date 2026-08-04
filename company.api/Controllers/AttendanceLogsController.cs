using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLogsController : ControllerBase
    {
        private readonly IAttendanceLogsService _attendanceLogsService;

        public AttendanceLogsController(IAttendanceLogsService attendanceLogsService)
        {
            _attendanceLogsService = attendanceLogsService ;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<AttendanceLogsDto>>> GetAttendanceLogs(int id)
        {
            var attendanceLogs = await _attendanceLogsService.GetAttendanceLogAsyncById(id);
            return Ok(attendanceLogs);
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceLogsDto>?>> GetAttendanceLogsByDate([FromQuery] int? employeeId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var attendanceLogs = await _attendanceLogsService.GetAttendanceLogsAsync(employeeId, from, to);
            if(attendanceLogs == null)return NotFound("No attendance logs found for the specified criteria.");
            return Ok(attendanceLogs);
        }

        [HttpPost]
        public async Task<ActionResult<AttendanceLogsDto>> CreateAttendanceLog([FromBody] CreateAttendanceLogsRequest createAttendanceLogsRequest)
        {
            var attendanceLog = await _attendanceLogsService.CreateAttendanceLogAsync(createAttendanceLogsRequest);
            if (attendanceLog is null) return BadRequest("Failed to create attendance log because the specified employee does not exist or the event type is invalid.");
            return CreatedAtAction(nameof(GetAttendanceLogs), new { id = attendanceLog.EmployeeId }, attendanceLog);
        }

    }
}
