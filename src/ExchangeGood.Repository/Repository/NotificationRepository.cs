using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository {
    public class NotificationRepository : INotificationRepository{
        private readonly IUnitOfWork _uow;

        public NotificationRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> AddNotifcation(Notification notification) {
            _uow.NotificationDAO.AddNotification(notification);

            return await _uow.SaveChangesAsync();
        }

        public async Task<Notification> GetNotifcation(int notificationId) {
            return await _uow.NotificationDAO.GetNotificatioById(notificationId);
        }

        public async Task<IEnumerable<Notification>> GetNotifcationsForUser(string userId) {
            var resul = await _uow.NotificationDAO.GetNotificationsOfFeID(userId);

            await _uow.SaveChangesAsync(); // because updating of DateRead of notification
            return resul;
        }

        public async Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string userId)
        {
            var result = await _uow.NotificationDAO.GetAllRequestExchangesFromUserAndOtherUserRequestForUser(userId); 
            await _uow.SaveChangesAsync(); // because updating of DateRead of notification
            return result;
        }
    }
}
