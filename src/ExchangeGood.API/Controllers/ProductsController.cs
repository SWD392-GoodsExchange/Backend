using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ExchangeGood.API.Controllers {
    public class ProductsController : BaseApiController {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) {
            _productService = productService;
        }

        // GET http://localhost:5000/api/products?keyword=a&orderby=price&pageSize=10&pageNumber=1
        [HttpGet] 
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams) {
            var response = await _productService.GetAllProducts(productParams);
            if(response.Data != null) {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // POST http://localhost:5000/api/products
        [Authorize(Roles = nameof(Role.Member.Name))]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest productRequest) {
            var feId = User.GetFeID();
            productRequest.FeId = feId;
            var result = await _productService.AddProduct(productRequest);
            if(result.IsSuccess) {
                return CreatedAtAction(nameof(CreateProduct), result.Data);
            }
            return BadRequest(result);            
        }


    }
}
