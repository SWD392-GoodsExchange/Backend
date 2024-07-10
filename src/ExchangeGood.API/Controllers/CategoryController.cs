using AutoMapper;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
	public class CategoryController : BaseApiController
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		public CategoryController(ICategoryService categoryService, IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var response = await _categoryService.GetAllCategories();
			if (response != null)
			{
				return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<CategoryDto>>(response)));
			}
			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
		}

		[HttpPost]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest categoryRequest)
		{
				var response = await _categoryService.AddCategory(categoryRequest);
				if (response != null)
					return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG));
				
				return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
		}


		[HttpDelete("{id}")]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin))]
        public async Task<IActionResult> DeleteCategory(int id)
		{
			await _categoryService.DeleteCategory(id);
			return Ok(BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG));
		}


		[HttpPut]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin))]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest categoryRequest)
		{
			var response = await _categoryService.UpdateCategory(categoryRequest);
			if(response != null)
				return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<CategoryDto>>(response)));

			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
		}

	}
}
