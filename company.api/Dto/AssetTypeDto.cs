using System.ComponentModel.DataAnnotations;

namespace company.api.Dto
{
    public class AssetTypeDto
    {
        [Required] public int AssetTypeId { get; set; }
        [Required][MaxLength(150)] public string TypeName { get; set; } = null!;
    }
}
