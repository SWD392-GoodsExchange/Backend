using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public CategoryRepository(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}

		public async Task<int> AddCategory(CreateCategoryRequest createCategory)
		{
			var existingCategory = await GetCategoryByName(createCategory.CategoryName);
			if (existingCategory != null)
			{
				return existingCategory.CateId;
			}

			var category = _mapper.Map<Category>(createCategory);
			_uow.CategoryDAO.AddCategory(category);

			await _uow.SaveChangesAsync();
			return category.CateId;
		}

		public async Task DeleteCategory(int id)
		{
			Category existedCategory = await _uow.CategoryDAO.GetCategoryByIdAsync(id);
			if (existedCategory == null)
			{
				throw new CategoryNotFoundException(id);
			}
			_uow.CategoryDAO.RemoveCategory(existedCategory);

			await _uow.SaveChangesAsync();
		}
		public async Task<List<CategoryDto>> GetAllCategories()
		{
			var query = _uow.CategoryDAO.GetCategories();

			var result = await query
				.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return result;
		}

		public async Task<CategoryDto> GetCategoryByID(int id)
		{
			var category = await _uow.CategoryDAO.GetCategoryByIdAsync(id);
			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<CategoryDto> GetCategoryByName(string name)
		{
			var category = await _uow.CategoryDAO.GetCategoryByNameAsync(name);
			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<int> UpdateCategory(UpdateCategoryRequest updateCategory)
		{
			Category existedCategory = await _uow.CategoryDAO.GetCategoryByIdAsync(updateCategory.CategoryId);
			if (existedCategory == null)
			{
				throw new CategoryNotFoundException(updateCategory.CategoryId);
			}
			_mapper.Map(updateCategory, existedCategory);
			_uow.CategoryDAO.UpdateCategory(existedCategory);

			await _uow.SaveChangesAsync();
			return existedCategory.CateId;
		}
	}
}
