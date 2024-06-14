using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Category
{
	public class CreateCategoryRequest
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
	}
}
