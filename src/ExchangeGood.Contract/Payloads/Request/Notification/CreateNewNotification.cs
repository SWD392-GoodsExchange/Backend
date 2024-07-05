using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Notification {
    public class CreateNewNotification {
        // Luan(A) gui request cho Huy(B)
        public string RecipientId { get; set; } // huy

        public string OnwerProductId { get; set; } //B

        public string ExchangerProductIds { get; set; } // A
    }
}
