using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Notification {
    public class CreateNewNotification {
        public string RecipientId { get; set; }

        public string OnwerProductId { get; set; }

        public string ExchangerProductIds { get; set; }

        public string Content { get; set; }
    }
}
