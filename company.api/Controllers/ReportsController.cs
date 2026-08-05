using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportDto>> GetReport(int id)
        {
            var report = await _reportsService.GetReportsAsyncByReportId(id);
            if (report == null)
            {
                return NotFound("Report not found.");
            }
            return Ok(report);
        }

        [HttpGet]
        public async Task<ActionResult<List<ReportDtoResponse>>> GetReports([FromQuery] int? employeeId, [FromQuery] int? assetId, [FromQuery] int? reportTypeId)
        {
            var reports = await _reportsService.GetReportsAsync(employeeId, assetId, reportTypeId);
            if (reports == null || !reports.Any())
            {
                return NotFound("No reports found.");
            }
            return Ok(reports);
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport([FromBody] CreateReportRequest req)
        {
            var createdReport = await _reportsService.CreateReportAsync(req);
            if (createdReport == null)
            {
                return BadRequest("Failed to create report.");
            }
            return CreatedAtAction(nameof(GetReport), new { id = createdReport.ReportId }, createdReport);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] UpdateReportRequest req)
        {
            var updatedReport = await _reportsService.UpdateReportAsync(id, req);
            if (updatedReport == null)
            {
                return NotFound("Couldn't update report.");
            }
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var deleted = await _reportsService.DeleteReportAsync(id);
            if (!deleted)
            {
                return NotFound("Report not found.");
            }
            return NoContent();
        }
    }
}
