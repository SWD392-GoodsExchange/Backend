using ExchangeGood.Contract.Payloads.Request.Bookmark;
using FluentValidation;

namespace ExchangeGood.Service.Validations
{
    public class CreateBookmarkRequestValidation : AbstractValidator<CreateBookmarkRequest>

    {
        public CreateBookmarkRequestValidation ()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product Id is required");
        }
    }
}