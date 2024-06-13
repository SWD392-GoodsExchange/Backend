using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.UseCase
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}
		public async Task<BaseResponse> AddCategory(CreateCategoryRequest createCategory)
		{
			await _categoryRepository.AddCategory(createCategory);
			return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
		}

		public async Task<BaseResponse> DeleteCategory(int id)
		{
			await _categoryRepository.DeleteCategory(id);
			return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
		}

		public async Task<BaseResponse> GetAllCategories()
		{
			var result = await _categoryRepository.GetAllCategories();	
			return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new List<CategoryDto>(result));
		}

		public async Task<BaseResponse> GetCategoryByID(int id)
		{
			var result = await _categoryRepository.GetCategoryByID(id);

			if (result != null)
			{
				return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
			}

			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
		}

		public async Task<BaseResponse> UpdateCategory(UpdateCategoryRequest updateCategory)
		{
			await _categoryRepository.UpdateCategory(updateCategory);
			return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
		}
	}
}
