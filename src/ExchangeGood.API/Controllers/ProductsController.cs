using AutoMapper;
using ExchangeGood.API.Extensions;
using ExchangeGood.Contract;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ExchangeGood.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET http://localhost:5000/api/products?keyword=a&orderby=price&pageSize=10&pageNumber=1
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
        {
            var result  = await _productService.GetAllProducts(productParams);
            if (result != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ProductDto>(result, result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages)));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }

        // POST http://localhost:5000/api/products
        [Authorize(Roles = nameof(Role.Member))]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest productRequest)
        {
            var feId = User.GetFeID();
            productRequest.FeId = feId;
            var result = await _productService.AddProduct(productRequest);
            if (result != null)
            {
                return CreatedAtAction(nameof(CreateProduct), result);
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("exchanges")]
        public async Task<IActionResult> GetProductsForExchange([FromQuery] GetProductsForExchangeRequest request)
        {
            var feId = User.GetFeID();
            request.FeId = feId;
            var result = await _productService.GetProductsForExchangeRequest(request);
            return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<ProductDto>>(result)));
        }
    }
}
