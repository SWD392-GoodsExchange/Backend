using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO {
    public class NotificationDAO {
        private readonly GoodsExchangeContext _context;

        public NotificationDAO(GoodsExchangeContext context) {
            _context = context;
        }

        public void AddNotification(Notification notifcation) {
            _context.Notifications.Add(notifcation);
        }

        public async Task<Notification> GetNotificatioById(int id) {
            return await _context.Notifications.FindAsync(id);
        }

        // get notification for user -> click the ring button -> get 20 new notification
        public async Task<IEnumerable<Notification>> GetNotificationsOfFeID(string id) {
            var query = await _context.Notifications
                .Where(x => x.RecipientId == id)
                .OrderByDescending(x => x.CreatedDate)
                .Take(25)
                .ToListAsync();

            var unreadNotification = query.Where(m => m.DateRead == null).ToList();
            if(unreadNotification.Any()) {
                foreach(var notification in unreadNotification) {
                    notification.DateRead = DateTime.UtcNow;
                }
            }

            return query.ToList(); 
        }

        public async Task<int> GetNumberUnreadNotificationedOfUser(string id) {
            return await _context.Notifications
                   .Where(x => x.RecipientId == id)
                   .Where(x => x.DateRead == null)
                   .CountAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string id) {

            var query = _context.Notifications
                .Where(n => n.SenderId == id || n.RecipientId == id)
                .Where(n => n.OnwerProductId != null && n.ExchangerProductIds != null)
                .Where(n => n.Type == Contract.Enum.Notification.Type.ExchangeRequest.Name)
                .OrderByDescending(m => m.CreatedDate)
                .AsQueryable();

            query = query.Where(n => n.Type == Contract.Enum.Notification.Type.ExchangeRequest.Name).Take(20);

            var unreadNotification = query.Where(n => n.DateRead == null && n.RecipientId == id).ToList();
            if(unreadNotification.Count > 0) {
                foreach (var notifcation in unreadNotification)
                {
                    notifcation.DateRead = DateTime.UtcNow;
                }
            }

            return await query.ToListAsync();
        }

        public void RemoveNotification(Notification notifcation) {
            _context.Notifications.Remove(notifcation);
        }

        public void UpdateNotification(Notification notifcation) {
            _context.Notifications.Update(notifcation);
        }
    }
}
