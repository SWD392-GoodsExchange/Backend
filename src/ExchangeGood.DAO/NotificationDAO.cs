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

        public async Task<IEnumerable<Notification>> GetNotificationsOfFeID(string id) {
            var query =  await _context.Notifications
                .Where(x => x.RecipientId.Equals(id))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            var unreadNotification = query.Where(m => m.DateRead == null).ToList();
            if(unreadNotification.Any()) {
                foreach(var notification in unreadNotification) {
                    notification.DateRead = DateTime.UtcNow;
                }
            }

            return query.ToList(); 
        }

        public async Task<IEnumerable<Notification>> GetNotificationsSendedByFeID(string id) {

            return await _context.Notifications
                .Where(n => n.SenderId.Equals(id))
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();
        }

        public void RemoveNotification(Notification notifcation) {
            _context.Notifications.Remove(notifcation);
        }

        public void UpdateNotification(Notification notifcation) {
            _context.Notifications.Update(notifcation);
        }
    }
}
