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
            public int ProductId { get; set; }

            public string FeId { get; set; }

            public int CateId { get; set; }

            public string Description { get; set; }

            ublic string Origin { get; set; }

            public string Type { get; set; }

            public decimal Price { get; set; }

            public string Title { get; set; }
         */
        public CreateProductRequestValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Tile is required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required");

            RuleFor(x => x.Origin)
                .NotEmpty()
                .WithMessage("Origin is required");

            RuleFor(x => x.Price)
                .Must(x => double.TryParse(x, out var price))
                .WithMessage("Price must be a number");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File image is required");
        }   
    }
}
