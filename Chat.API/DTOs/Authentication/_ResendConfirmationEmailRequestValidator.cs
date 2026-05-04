using FluentValidation;

namespace Chat.API.DTOs.Authentication
{
    public class _ResendConfirmationEmailRequestValidator : AbstractValidator<_ResendConfirmationEmailRequest>
    {
        public _ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
