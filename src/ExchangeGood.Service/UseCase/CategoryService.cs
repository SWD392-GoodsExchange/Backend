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
			var existingCategory = await _categoryRepository.GetCategoryByName(createCategory.CategoryName);
			if (existingCategory != null)
			{
				return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DUPLCATE_MSG);
			}

			await _categoryRepository.AddCategory(createCategory);
			return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, createCategory);
		}


		public async Task<BaseResponse> DeleteCategory(int id)
		{
			var result = await _categoryRepository.GetCategoryByID(id);

			if (result != null)
			{
				await _categoryRepository.DeleteCategory(id);
				return BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
			}
			else
			{
				return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG);
			}
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

		public async Task<BaseResponse> GetCategoryByName(string name)
		{
			var result = await _categoryRepository.GetCategoryByName(name);

			if (result != null)
			{
				return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
			}

			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
		}

		public async Task<BaseResponse> UpdateCategory(UpdateCategoryRequest updateCategory)
		{
			var result = await _categoryRepository.GetCategoryByID(updateCategory.CategoryId);

			if (result != null)
			{
				await _categoryRepository.UpdateCategory(updateCategory);
				return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, updateCategory);
			}
			else
			{
				return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
			}
		}
	}
}
