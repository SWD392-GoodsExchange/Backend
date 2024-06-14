using System.Text.RegularExpressions;
using ExchangeGood.Contract.Payloads.Request.Members;
using FluentValidation;

namespace ExchangeGood.Service.Validations
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.FeId)
                .NotEmpty().WithMessage("Fe Id is Required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");
        }
    }
}