using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers {
    public class ProductsController : BaseApiController {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]ProductParams productParams) {
            var response = await _productService.GetAllProducts(productParams);
            if(response.Data != null) {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest productRequest) {
            System.Console.WriteLine(productRequest.Price);
            return Ok();
        }
    }
}
