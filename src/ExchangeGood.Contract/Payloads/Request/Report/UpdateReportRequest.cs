using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Report
{
	public class UpdateReportRequest
	{
		public int ReportId { get; set; }
		public string Message { get; set; }
		public string Status { get; set; }
	}
}
