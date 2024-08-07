using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces {
    public interface INotificationRepository {
        public Task<Notification> GetNotifcation(int notificationId);
        public Task<bool> AddNotifcation(Notification notifcation);
        public Task<IEnumerable<Notification>> GetNotifcationsForUser(string userId);
        public Task<int> GetNumberUnreadNotificationedOfUser(string userId);
        public Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string userId);
        public Task<bool> RemoveNotification(int notificationId);
    }
} 
