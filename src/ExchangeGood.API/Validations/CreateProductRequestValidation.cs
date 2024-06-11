using ExchangeGood.Contract.Payloads.Request.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Validations {
    public class CreateProductRequestValidation : AbstractValidator<CreateProductRequest>{
        /*
         public int MemberId { get; set; }

        public int CateId { get; set; }

        public string UsageInformation { get; set; }

        public string Origin { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }
         */
        public CreateProductRequestValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Tile is required");

            RuleFor(x => x.UsageInformation)
                .NotEmpty()
                .WithMessage("Description is required");

            RuleFor(x => x.Origin)
                .NotEmpty()
                .WithMessage("Origin is required");

            RuleFor(x => x.Price)
                .Must(x => double.TryParse(x, out var price))
                .WithMessage("Price must be a number"); 
        }   
    }
}
