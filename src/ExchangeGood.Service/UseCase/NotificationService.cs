using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Repository.Repository;
using ExchangeGood.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.UseCase
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IMemberRepository _memberRepository;


		public NotificationService(INotificationRepository notificationRepository, IMemberRepository memberRepository)
		{
			_notificationRepository = notificationRepository;
			_memberRepository = memberRepository;
		}

		public async Task<BaseResponse> AddNotification(CreateNotificationRequest createNotification)
		{
			var existingMember = await _memberRepository.GetMemberById(createNotification.FeId);

			if (existingMember != null)
			{
				NotificationDto notificationDto = await _notificationRepository.AddNotification(createNotification);
				return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, notificationDto);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
		}

		public async Task<BaseResponse> DeleteNotification(int id)
		{
			var existingNoti = await _notificationRepository.GetNotificationByID(id);
			if (existingNoti != null)
			{
				await _notificationRepository.DeleteNotification(id);
				return BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, existingNoti);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG);
		}

		public async Task<BaseResponse> GetAllNotifications()
		{
			var result = await _notificationRepository.GetAllNotifications();
			return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new List<NotificationDto>(result));
		}

		public async Task<BaseResponse> GetNotificationByID(int id)
		{
			var result = await _notificationRepository.GetNotificationByID(id);

			if (result != null)
			{
				return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
			}

			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
		}

		public async Task<BaseResponse> UpdateNotification(UpdateNotificationRequest updateNotification)
		{
			var result = await _notificationRepository.GetNotificationByID(updateNotification.NotificationId);

			if (result != null)
			{
				await _notificationRepository.UpdateNotification(updateNotification);
				return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, updateNotification);
			}
			else
			{
				return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
			}
		}
	}
}
