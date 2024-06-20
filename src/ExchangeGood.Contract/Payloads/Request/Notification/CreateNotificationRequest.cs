using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Notification
{
	public class CreateNotificationRequest
	{
		public int NotificationId { get; set; }

		public string FeId { get; set; }

		public string Content { get; set; }

	}
}
