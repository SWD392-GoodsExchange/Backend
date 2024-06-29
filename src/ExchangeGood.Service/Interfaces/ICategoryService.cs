using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
	public interface ICategoryService
	{
		public Task<IEnumerable<Category>> GetAllCategories();
		public Task<Category> GetCategoryByID(int id);
		public Task<Category> AddCategory(CreateCategoryRequest createCategory);
		public Task<Category> UpdateCategory(UpdateCategoryRequest updateCategory);
		public Task DeleteCategory(int id);
	}
}
