using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
	public class CategoryController : BaseApiController
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var response = await _categoryService.GetAllCategories();
			if (response.Data != null)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest categoryRequest)
		{
				var response = await _categoryService.AddCategory(categoryRequest);
				return Ok(response);
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var response = await _categoryService.DeleteCategory(id);
			return Ok(response);
		}


		[HttpPut]
		public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest categoryRequest)
		{
			var response = await _categoryService.UpdateCategory(categoryRequest);
			return Ok(response);
		}

	}
}
