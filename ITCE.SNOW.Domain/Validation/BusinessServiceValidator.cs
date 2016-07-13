using FluentValidation;

namespace ITCE.SNOW.Domain.Validation
{
    public class BusinessServiceValidator : AbstractValidator<BusinessService>
    {
        public BusinessServiceValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Business Service name is not filled in.");
            RuleFor(u => u.Manager).NotEmpty().WithMessage("Business Service manager is not filled in.").SetValidator(new UserValidator());
            RuleFor(u => u.BusinessCriticality).NotEmpty().WithMessage("Business service criticality is not filled in.");
            RuleFor(u => u.AssignmentGroup).NotEmpty().WithMessage("Business service assignment group is not filled in.");
        }
    }
}