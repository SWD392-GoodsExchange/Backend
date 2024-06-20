using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces
{
	public interface INotificationRepository
	{
		public Task<List<NotificationDto>> GetAllNotifications();
		public Task<NotificationDto> GetNotificationByID(int id);
		public Task<NotificationDto> AddNotification(CreateNotificationRequest createNotification);
		public Task<int> UpdateNotification(UpdateNotificationRequest updateNotification);
		public Task DeleteNotification(int id);
	}
}
