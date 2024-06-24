using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using FluentValidation;

namespace ExchangeGood.Service.Validations
{
    public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidation()
        {
            RuleFor(rf => rf.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh Token is Required");
            RuleFor(rf => rf.JwtToken)
                .NotEmpty()
                .WithMessage("Jwt Token is Required");
        }
    }
}