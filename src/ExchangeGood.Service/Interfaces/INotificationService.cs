using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
	public interface INotificationService
	{
		public Task<BaseResponse> GetAllNotifications();
		public Task<BaseResponse> GetNotificationByID(int id);
		public Task<BaseResponse> AddNotification(CreateNotificationRequest createNotification);
		public Task<BaseResponse> UpdateNotification(UpdateNotificationRequest updateNotification);
		public Task<BaseResponse> DeleteNotification(int id);
	}
}
