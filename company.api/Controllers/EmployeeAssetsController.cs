using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAssetsController : ControllerBase
    {
        private readonly IEmployeeAssetsService _EmployeeAssetsService;

        public EmployeeAssetsController(IEmployeeAssetsService  EmployeeAssetsService)
        {
            _EmployeeAssetsService = EmployeeAssetsService;
        }


        [HttpGet]
        public async Task<ActionResult<List<EmployeeAssetDto>>> GetEmployeeAssets([FromQuery] int? employeeId, [FromQuery] int? assetId, [FromQuery] bool? active)
        {
            var employeeAssets = await _EmployeeAssetsService.GetEmployeeAssetsAsync(employeeId, assetId, active);
            if (employeeAssets == null) return NotFound("No employee assets found.");
            return Ok(employeeAssets);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeAssetDto>> GetEmployeeAsset(int id)
        {
            var employeeAsset = await _EmployeeAssetsService.GetEmployeeAssetAsync(id);
            if (employeeAsset == null) return NotFound("Employee asset not found.");
            return Ok(employeeAsset);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeAssetDto>> CreateEmployeeAsset([FromBody] CreateEmployeeAssetRequest createEmployeeAssetRequest)
        {
            var createdEmployeeAsset = await _EmployeeAssetsService.CreateEmployeeAssetsAsync(createEmployeeAssetRequest);
            if (createdEmployeeAsset == null) return BadRequest("Failed to create employee asset.");
            return CreatedAtAction(nameof(GetEmployeeAsset), new { id = createdEmployeeAsset.EmployeeAssetId }, createdEmployeeAsset );
        }

        [HttpPut("{id:int}/return")]
        public async Task<ActionResult<EmployeeAssetDto>> UpdateEmployeeAsset(int id, [FromBody] string? notes)
        { 
            var updatedEmployeeAsset = await _EmployeeAssetsService.UpdateEmployeeAssetsAsync(id, notes);
            if (updatedEmployeeAsset == null) return BadRequest("Failed to update employee asset.");
            return Ok(updatedEmployeeAsset);
        }

    }
}
