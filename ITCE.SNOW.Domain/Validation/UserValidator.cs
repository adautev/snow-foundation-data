using FluentValidation;

namespace ITCE.SNOW.Domain.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("User name is not filled in.");
        }
    }
}
