using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
	public interface ICategoryService
	{
		public Task<BaseResponse> GetAllCategories();
		public Task<BaseResponse> GetCategoryByID(int id);
		public Task<BaseResponse> GetCategoryByName(string name);
		public Task<BaseResponse> AddCategory(CreateCategoryRequest createCategory);
		public Task<BaseResponse> UpdateCategory(UpdateCategoryRequest updateCategory);
		public Task<BaseResponse> DeleteCategory(int id);
	}
}
