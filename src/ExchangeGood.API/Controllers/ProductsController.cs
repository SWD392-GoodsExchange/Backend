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
using System.Linq.Expressions;

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

        // GET http://localhost:5000/api/products?keyword=orderby=price&pageSize=10&pageNumber=1
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
        {
            var result = await _productService.GetAllProducts(productParams);
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
                return CreatedAtAction(nameof(GetProducts), _mapper.Map<ProductDto>(result));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("exchange")] // get product for exchange through notification
        public async Task<IActionResult> GetProductsForExchange([FromQuery] GetProductsForExchangeRequest request)
        {
            var feId = User.GetFeID();
            if (request.OwnerId == feId) throw new Exception("Request is not for user");
            var result = await _productService.GetProductsForExchangeRequest(request);
            return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<ProductDto>>(result)));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("fe/{id}")]
        public async Task<IActionResult> GetProductsByFeId(string id, [FromQuery] string type) {

            var result = await _productService.GetProductsByFeId(id, type);
            if (result.Count() > 0)
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<ProductDto>>(result)));

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetProductsByCateId(int id)
        {
            var result = await _productService.GetProductsByCateId(id);
            if (result.Count() > 0)
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<ProductDto>>(result)));

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id) {
            var result = await _productService.GetProduct(id, x => x.Cate, x => x.Fe, x => x.Images);
            if (result != null)
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<ProductDto>(result)));

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest productRequest)
        {
            var feId = User.GetFeID();
            productRequest.FeId = feId;
            var result = await _productService.UpdateProduct(productRequest);
            if (result != null)
            {
               return Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }
    }
}
