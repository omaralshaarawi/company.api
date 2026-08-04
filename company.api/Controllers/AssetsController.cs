using company.api.Dto;
using company.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace company.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;

        public AssetsController(IAssetsService assetsService)
        {
            _assetsService = assetsService;
        }

        // GET: api/Assets with opitional filters
        [HttpGet]
        public async Task<ActionResult<List<AssetDto>>> GetAssets([FromQuery] int? assetTypeId, [FromQuery] string? status)
        {
            var assets = await _assetsService.GetAssetsAsync(assetTypeId, status);
            return Ok(assets);
        }

        // GET: api/Assets/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AssetDto>> GetAsset(int id)
        {
            var asset = await _assetsService.GetAssetAsync(id);
            if (asset is null) return NotFound("Asset not found");
            return Ok(asset);
        }
        // POST: api/Assets
        [HttpPost]
        public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] CreateAssetRequest createAssetDto)
        {
            var asset = await _assetsService.CreateAssetAsync(createAssetDto);
            if (asset is null) return BadRequest("Failed to create asset because the specified asset type does not exist");
            return CreatedAtAction(nameof(GetAsset), new { id = asset.AssetId }, asset);
        }

        // PUT: api/Assets/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] AssetDto updateAssetDto)
        {
            var asset = await _assetsService.UpdateAssetAsync(id, updateAssetDto);
            if (asset is null) return NotFound("Asset or Asset type not found");
            return NoContent();
        }

        // DELETE: api/Assets/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            var result = await _assetsService.DeleteAssetAsync(id);
            if (!result) return NotFound("Asset not found");
            return NoContent();
        }

        // GET: api/Assets/history
        [HttpGet("{id:int}/history")]
        public async Task<ActionResult<List<EmployeeAssetDto>>> GetAssetHistory(int id)
        {
            var history = await _assetsService.GetAssetHistoryAsync(id);
            return Ok(history);
        }
    }
}
