using company.api.Data;
using company.api.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace company.api.Validators
{
    public class AssignAssetRequestValidator : AbstractValidator<CreateEmployeeAssetRequest>
    {
        public AssignAssetRequestValidator(CompanyContext db)
        {
            RuleFor(x => x.EmployeeId)
            .MustAsync(async (id, ct) => await db.Employees.AnyAsync(e => e.EmployeeId == id, ct))
            .WithMessage("Employee does not exist.");
            RuleFor(x => x.AssetId)
            .MustAsync(async (id, ct) =>
            {
                var asset = await db.Assets.FindAsync(new object?[] { id }, ct);
                return asset is not null && asset.Status == "InStock";
            })
            .WithMessage("Asset must exist and currently be InStock to be assigned.");
        }
    }
}
