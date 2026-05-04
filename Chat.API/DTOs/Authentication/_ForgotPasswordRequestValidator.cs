using FluentValidation;

namespace Chat.API.DTOs.Authentication
{
    public class _ForgotPasswordRequestValidator : AbstractValidator<_ForgotPasswordRequest>
    {
        public _ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
