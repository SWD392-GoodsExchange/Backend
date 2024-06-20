using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
	public class NotificationController : BaseApiController
	{
		private readonly INotificationService _notificationService;
		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> GetNotifications()
		{
			var response = await _notificationService.GetAllNotifications();
			if (response.Data != null)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNotifcation([FromBody] CreateNotificationRequest notificationRequest)
		{
			var response = await _notificationService.AddNotification(notificationRequest);
			return Ok(response);
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotification(int id)
		{
			var response = await _notificationService.DeleteNotification(id);
			return Ok(response);
		}


		[HttpPut]
		public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationRequest updateNotification)
		{
			var response = await _notificationService.UpdateNotification(updateNotification);
			return Ok(response);
		}








	}
}
