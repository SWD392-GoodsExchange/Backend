using ExchangeGood.Contract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Report
{
	public class ReportParam : PaginationParams
	{
		public string? Keyword { get; set; }
		public string Orderby { get; set; } = "lastActive";
	}
}
