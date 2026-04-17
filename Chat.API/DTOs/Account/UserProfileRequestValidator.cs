using Chat.API.Const;
using FluentValidation;

namespace Chat.API.DTOs.Account
{
    public class UserProfileRequestValidator : AbstractValidator<UserProfileRequest>
    {
        public UserProfileRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(3, 100);
            RuleFor(x => x.LastName).NotEmpty().Length(3, 100);
            RuleFor(x => x.PhoneNumber)
                .Length(11)
                .Matches(RejexPattern.ValidphoneNumber)
                .WithMessage("Invalid phone number");
        }
    }
}
