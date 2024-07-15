using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Orders;
using FluentValidation;

namespace ExchangeGood.API.Validations
{
    public class OrderDetailDtoValidation : AbstractValidator<OrderDetailDto>
    {
        public OrderDetailDtoValidation()
        {
            RuleFor(x => x.Amount).NotNull().WithMessage("The amount of order can not be null");
            RuleFor(x => x.SellerId).NotEmpty().WithMessage("The Seller Id can not be empty");
        }
    }
    public class CreateOrderRequestValidation : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidation()
        {
            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailDtoValidation());
        }
    }
    
    
}