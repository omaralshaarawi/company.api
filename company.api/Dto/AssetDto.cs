namespace company.api.Dto
{
    public record CreateAssetRequest(
        int AssetTypeId,
        string AssetName,
        string? SerialNumber,
        DateOnly? PurchaseDate,
        decimal? PurchaseCost
    );
    public record AssetDto(
        int AssetId,
        int? AssetTypeId,
        string AssetName,
        string? SerialNumber,
        DateOnly? PurchaseDate,
        decimal? PurchaseCost,
        string? Status
    );
    

    

}
