using FluentValidation;

namespace Chat.API.DTOs.Authentication
{
    public class _ConfirmEmailRequestValidator : AbstractValidator<_ConfirmEmailRequest>
    {
        public _ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.UseId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Confirmation code is required.");
        }
    }
}
