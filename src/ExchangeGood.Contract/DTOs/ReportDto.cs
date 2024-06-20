using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs
{
	public class ReportDto
	{
		public int ReportId { get; set; }
		public string FeId { get; set; }
		public int ProductId { get; set; }
		public string Message { get; set; }
		public string Status { get; set; }
		public DateTime createdTime { get; set; }

	}
}
