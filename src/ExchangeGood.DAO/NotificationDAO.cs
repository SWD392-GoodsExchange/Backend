using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO
{
	public class NotificationDAO
	{
		private readonly GoodsExchangeContext _context;

		public NotificationDAO(GoodsExchangeContext context)
		{
			_context = context;
		}

		public void AddNotification(Notifcation notifcation)
		{
			_context.Notifcations.Add(notifcation);
		}

		public async Task<Notifcation> GetNotificationByIdAsync(int id)
		{
			return await _context.Notifcations.FindAsync(id);
		}

		public IQueryable<Notifcation> GetNotifications()
		{
			var query = _context.Notifcations
				.AsQueryable()
				.AsNoTracking();

			return query.AsNoTracking();
		}

		public void RemoveNotification(Notifcation notifcation)
		{
			_context.Notifcations.Remove(notifcation);
		}

		public void UpdateNotification(Notifcation notifcation)
		{
			_context.Notifcations.Update(notifcation);
		}
	}
}
