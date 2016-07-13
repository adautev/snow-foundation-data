using FluentValidation;

namespace ITCE.SNOW.Domain.Validation
{
    public class ServicePortalRequestValidator : AbstractValidator<ServicePortalRequest>
    {
        public  ServicePortalRequestValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Service portal request name is not filled in.");
            RuleFor(u => u.AssignmentGroup).NotEmpty().WithMessage("Service portal request implementer group is not filled in.");
            RuleFor(u => u.Country).NotEmpty();
            RuleFor(u => u.Category).NotEmpty();
            RuleFor(u => u.BusinessService).NotEmpty();
        }
    }
}