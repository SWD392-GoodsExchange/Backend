using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions
{
	public class CategoryNotFoundException : NotFoundException
	{
		public CategoryNotFoundException(int id) : base($"Category with id {id} is not found.")
		{
		}
	}
}
