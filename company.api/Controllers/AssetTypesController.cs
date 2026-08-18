using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypesController : ControllerBase
    {

        private readonly IAssetTypesService _assetTypesService;

        public AssetTypesController(IAssetTypesService assetTypesService)
        {
            _assetTypesService = assetTypesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssetTypeDto>>> GetAssetTypes()
        {
            var assetTypes = await _assetTypesService.GetAssetTypesAsync();
            if((assetTypes==null || !assetTypes.Any())) {
                return NotFound("No asset types found");
            }
            return Ok(assetTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AssetTypeDto>> GetAssetType(int id)
        {
            var assetType = await _assetTypesService.GetAssetTypeAsync(id);
            if (assetType is null) return NotFound("Asset type not found");
            return Ok(assetType);
        }

        [HttpPost]
        public async Task<ActionResult<AssetTypeDto>> CreateAssetType([FromBody] string assetTypeName)
        {
            var assetType = await _assetTypesService.CreateAssetTypeAsync(assetTypeName);
            if (assetType is null) return BadRequest("Failed to create asset type");
            return CreatedAtAction(nameof(GetAssetType), new { id = assetType.AssetTypeId }, assetType);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAssetType(int id, [FromBody] string assetTypeName)
        {
            var updatedAssetType = await _assetTypesService.UpdateAssetTypeAsync(id, assetTypeName);
            if (updatedAssetType is null) return NotFound("Asset type not found");
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAssetType(int id)
        {
            var deleted = await _assetTypesService.DeleteAssetTypeAsync(id);
            if (!deleted) return NotFound("Asset type not found");
            return NoContent();
        }
    }
}
