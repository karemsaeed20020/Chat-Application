using FluentValidation;

namespace Chat.API.DTOs.Authentication
{
    public class _LoginRequestValidator : AbstractValidator<_LoginRequest>
    {
        public _LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
