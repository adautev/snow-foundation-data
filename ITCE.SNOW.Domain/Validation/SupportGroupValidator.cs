using FluentValidation;

namespace ITCE.SNOW.Domain.Validation
{
    public class SupportGroupValidator : AbstractValidator<SupportGroup>
    {
        public SupportGroupValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Support group name is not filled in.");
            RuleFor(u => u.Manager).NotEmpty().WithMessage("Support group manager is not filled in.").SetValidator(new UserValidator());
            RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Support group email is not filled in or not a valid email address.");
        }
    }
}