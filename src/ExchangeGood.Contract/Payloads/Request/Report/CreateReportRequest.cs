using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Report
{
	public class CreateReportRequest
	{
		public string FeId { get; set; }
		public int ProductId { get; set; }
		public string Message { get; set; }
	}
}
