using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Notification {
    public class CreateNewNotification {
        // SE173531(4) gui request cho SE182732(6)
        public string RecipientId { get; set; } // SE182732

        public string OnwerProductId { get; set; } //6

        public string ExchangerProductIds { get; set; } // 4
    }
}
