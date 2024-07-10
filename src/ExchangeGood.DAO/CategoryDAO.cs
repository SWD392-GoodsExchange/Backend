using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO
{
	public class CategoryDAO
	{
		private readonly GoodsExchangeContext _context;

		public CategoryDAO(GoodsExchangeContext context)
		{
			_context = context;
		}

		public void AddCategory(Category category)
		{
			_context.Categories.Add(category);
		}

		public async Task<Category> GetCategoryByIdAsync(int id)
		{
			return await _context.Categories.FindAsync(id);
		}

		public async Task<IEnumerable<Category>> GetCategories()
		{
			var query = _context.Categories.AsQueryable()
				.AsNoTracking();

			return await query.ToListAsync();
		}

		public void RemoveCategory(Category category)
		{
			_context.Categories.Remove(category);
		}

		public void UpdateCategory(Category category)
		{
			_context.Categories.Update(category);
		}
	}

}
