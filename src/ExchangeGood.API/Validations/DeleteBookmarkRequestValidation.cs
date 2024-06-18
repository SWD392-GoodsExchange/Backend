using ExchangeGood.Contract.Payloads.Request.Bookmark;
using FluentValidation;

namespace ExchangeGood.Service.Validations
{
    public class DeleteBookmarkRequestValidation : AbstractValidator<DeleteBookmarkRequest>
    {
        public DeleteBookmarkRequestValidation()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is Required");
        }
    }
}