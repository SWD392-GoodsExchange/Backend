using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using ExchangeGood.Contract.Enum.Notification;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public NotificationRepository(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<NotificationDto> AddNotification(CreateNotificationRequest createNotification)
		{
			var notification = _mapper.Map<Notifcation>(createNotification);
			_uow.NotificationDAO.AddNotification(notification);
			notification.NotificationId = 0;
			notification.CreatedDate = DateTime.Now;
			notification.Type = Contract.Enum.Notification.Type.Send.Name;
			await _uow.SaveChangesAsync();

			return _mapper.Map<NotificationDto>(notification);
		}

		public async Task DeleteNotification(int id)
		{
			Notifcation existedNoti = await _uow.NotificationDAO.GetNotificationByIdAsync(id);
			if (existedNoti == null)
			{
				throw new CategoryNotFoundException(id);
			}
			_uow.NotificationDAO.RemoveNotification(existedNoti);

			await _uow.SaveChangesAsync();
		}

		public async Task<List<NotificationDto>> GetAllNotifications()
		{
			var query = _uow.NotificationDAO.GetNotifications();

			var result = await query
				.ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return result;
		}

		public async Task<NotificationDto> GetNotificationByID(int id)
		{
			var notification = await _uow.NotificationDAO.GetNotificationByIdAsync(id);
			return _mapper.Map<NotificationDto>(notification);
		}

		public async Task<int> UpdateNotification(UpdateNotificationRequest updateNotification)
		{
			Notifcation existedNoti = await _uow.NotificationDAO.GetNotificationByIdAsync(updateNotification.NotificationId);
			if (existedNoti == null)
			{
				throw new CategoryNotFoundException(existedNoti.NotificationId);
			}
			_mapper.Map(updateNotification, existedNoti);
			_uow.NotificationDAO.UpdateNotification(existedNoti);

			await _uow.SaveChangesAsync();
			return existedNoti.NotificationId;
		}
	}
}
