using company.api.Data;
using company.api.Dto;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace company.api.Validators
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator(CompanyContext db)
        {
            RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(150);
            RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Enter a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .MaximumLength(20);
            RuleFor(x => x.NationalId)
            .MustAsync(async (nationalId, ct) =>
                nationalId is not null && !await db.Employees.AnyAsync(e => e.NationalId == nationalId, ct))
            .WithMessage("National ID must be unique.")
            .When(x => !string.IsNullOrEmpty(x.NationalId));
            RuleFor(x => x.DepartmentId)
            .MustAsync(async (id, ct) =>
            id is null || await db.Departments.AnyAsync(d => d.DepartmentId == id, ct))
            .WithMessage("DepartmentId does not exist.");
            RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Hire date cannot be in the future.")
            .When(x => x.HireDate is not null);
        }
    }
}
