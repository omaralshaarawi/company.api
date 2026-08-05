using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public record CreateAssetRequest(
       [Required] int AssetTypeId,
        [Required][MaxLength(150)] string AssetName,
        string? SerialNumber,
        DateOnly? PurchaseDate,
        decimal? PurchaseCost
    );
    public record AssetDto(
        [Required] int AssetId,
        [Required] int? AssetTypeId,
        [Required][MaxLength(150)] string AssetName,
        string? SerialNumber,
        DateOnly? PurchaseDate,
        decimal? PurchaseCost,
        string? Status
    );
    

    

}
