﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Notification
{
	public class UpdateNotificationRequest
	{
		public int NotificationId { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }

	}
}
