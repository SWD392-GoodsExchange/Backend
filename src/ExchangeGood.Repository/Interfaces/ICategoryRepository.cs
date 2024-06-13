using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces
{
	public interface ICategoryRepository
	{
		public Task<List<CategoryDto>> GetAllCategories();
		public Task<CategoryDto> GetCategoryByID(int id);
		public Task<int> AddCategory(CreateCategoryRequest  createCategory);
		public Task<int> UpdateCategory(UpdateCategoryRequest updateCategory);	
		public Task DeleteCategory(int id);

	}
}
