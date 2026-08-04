using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FingerprintsController : ControllerBase
    {
        private readonly IFingerprintsService _fingerprintsService;

        public FingerprintsController(IFingerprintsService fingerprintsService)
        {
            _fingerprintsService = fingerprintsService;
        }


        [HttpGet]
        public async Task<ActionResult<List<FingerprintDto>>> GetFingerprintByEmployeeId([FromQuery] int employeeId)
        {
            var fingerprint = await _fingerprintsService.GetFingerprintAsyncByEmployeeId(employeeId);
            if(fingerprint == null) return NotFound("Fingerprint or employee not found");
            return Ok(fingerprint);
        }

        [HttpPost]
        public async Task<ActionResult<FingerprintDto>> CreateFingerprint([FromBody] CreateFingerprintRequest req)
        {
            var fingerprint = await _fingerprintsService.CreateFingerprintAsync(req);
            if (fingerprint == null) return BadRequest("Failed to create fingerprint. Employee may not exist.");
            return CreatedAtAction(nameof(GetFingerprintByEmployeeId), new { id = fingerprint.EmployeeId }, fingerprint);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FingerprintDto>> GetFingerprintById(int id)
        {
            var fingerprint = await _fingerprintsService.GetFingerprintAsyncById(id);
            if (fingerprint == null) return NotFound("Fingerprint not found");
            return Ok(fingerprint);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFingerprint(int id)
        {
            var deleted = await _fingerprintsService.DeleteFingerprintAsync(id);
            if (!deleted) return NotFound("Fingerprint not found");
            return NoContent();
        }
    }
}
