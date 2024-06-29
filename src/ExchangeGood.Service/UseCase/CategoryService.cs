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
		public async Task<Category> AddCategory(CreateCategoryRequest createCategory)
		{
			return await _categoryRepository.AddCategory(createCategory);
		}

		public async Task DeleteCategory(int id)
		{
			await _categoryRepository.DeleteCategory(id);
		}

		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return await _categoryRepository.GetAllCategories();	
		}

		public async Task<Category> GetCategoryByID(int id)
		{
			return await _categoryRepository.GetCategoryByID(id);
		}

		public async Task<Category> UpdateCategory(UpdateCategoryRequest updateCategory)
		{
			return await _categoryRepository.UpdateCategory(updateCategory);
		}
	}
}
