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

		public async Task<Category> AddCategory(CreateCategoryRequest createCategory)
		{
			var category = _mapper.Map<Category>(createCategory);
			_uow.CategoryDAO.AddCategory(category);
			if(await _uow.SaveChangesAsync())
				return category;
			return null;
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
		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return await _uow.CategoryDAO.GetCategories();
		}

		public async Task<Category> GetCategoryByID(int id)
		{
			return await _uow.CategoryDAO.GetCategoryByIdAsync(id);
		}

		public async Task<Category> UpdateCategory(UpdateCategoryRequest updateCategory)
		{
			Category existedCategory = await _uow.CategoryDAO.GetCategoryByIdAsync(updateCategory.CategoryId);
			if (existedCategory == null)
			{
				throw new CategoryNotFoundException(updateCategory.CategoryId);
			}
			_mapper.Map(updateCategory, existedCategory);
			_uow.CategoryDAO.UpdateCategory(existedCategory);

			if(await _uow.SaveChangesAsync())
				return existedCategory;

			return null;
		}
	}
}
