using ExchangeGood.Contract.Payloads.Request.Orders;
using FluentValidation;

namespace ExchangeGood.API.Validations
{
    public class GetOrderRequestValidation : AbstractValidator<GetOrderRequest>
    {
        public GetOrderRequestValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required");
        }
    }
}