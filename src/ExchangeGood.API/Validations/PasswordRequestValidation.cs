using ExchangeGood.Contract.Payloads.Request.Members;
using FluentValidation;

namespace ExchangeGood.Service.Validations
{
    public class PasswordRequestValidation : AbstractValidator<PasswordRequest>
    {
        public PasswordRequestValidation ()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old Password is Required");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password is Required")
                .Length(6, 20).WithMessage("New Password must must be 6 characters and less than 20 characters");
        }
    }
}