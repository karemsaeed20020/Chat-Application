using Chat.API.Const;
using FluentValidation;

namespace Chat.API.DTOs.Authentication
{
    public class _ResetPasswordRequestValidator : AbstractValidator<_ResetPasswordRequest>
    {
        public _ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Reset code is required.");
            RuleFor(x => x.NewPassword)
            .Matches(RejexPattern.StrongPassword)
            .WithMessage("Password must contains atleast 8 digits, one Uppercase,one Lowercase and NunAlphanumeric");
        }
    }
}
