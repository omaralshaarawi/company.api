using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsTypesController : ControllerBase
    {
        private readonly IReportsTypesService _reportsTypesService;

        public ReportsTypesController(IReportsTypesService reportsTypesService)
        {
            _reportsTypesService = reportsTypesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReportTypeDto>?>> GetReportsTypes()
        {
            var reportsTypes = await _reportsTypesService.GetReportTypesAsync();
            if (reportsTypes == null || !reportsTypes.Any())
                return NotFound("No report types found.");
            return Ok(reportsTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportTypeDto?>> GetReportType(int id)
        {
            var reportType = await _reportsTypesService.GetReportTypeAsync(id);
            if (reportType == null)
                return NotFound("Report type not found.");
            return Ok(reportType);
        }

        [HttpPost]
        public async Task<ActionResult<ReportTypeDto?>> CreateReportType([FromBody] string reportTypeName)
        {
            var reportType = await _reportsTypesService.CreateReportTypeAsync(reportTypeName);
            if (reportType == null)
                return BadRequest("Failed to create report type.");
            return CreatedAtAction(nameof(GetReportType), new { id = reportType.ReportTypeId }, reportType);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ReportTypeDto?>> UpdateReportType(int id, [FromBody] string reportTypeName)
        {
            var reportType = await _reportsTypesService.UpdateReportTypeAsync(id, reportTypeName);
            if (reportType == null)
                return NotFound("Report type not found.");
            return Ok(reportType);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteReportType(int id)
        {
            var result = await _reportsTypesService.DeleteReportTypeAsync(id);
            if (!result)
                return NotFound("Report type not found or cant be deleted as it is used in other reports.");
            return Ok(result);
        }
    }
}
