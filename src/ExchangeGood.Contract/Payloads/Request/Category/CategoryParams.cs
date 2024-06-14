using ExchangeGood.Contract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Category
{
	public class CategoryParams : PaginationParams
	{
		public string? Keyword { get; set; }
		public string OrderBy { get; set; } = "lastActive";
	}
}
