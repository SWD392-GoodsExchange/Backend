using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs
{
	public class NotificationDto
	{
		public int NotificationId { get; set; }

		public string FeId { get; set; }

		public string Content { get; set; }

		public DateTime CreatedDate { get; set; }

		public string Type { get; set; }


	}
}
