namespace company.api.Dto
{
    public class AssetDto
    {
        public int AssetId { get; set; }
        public int? AssetTypeId { get; set; }
        public string? AssetTypeName { get; set; } 
        public string AssetName { get; set; } = null!;
        public string? SerialNumber { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public decimal? PurchaseCost { get; set; }
        public string? Status { get; set; }
    }
}
