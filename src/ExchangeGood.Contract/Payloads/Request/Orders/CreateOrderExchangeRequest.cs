using ExchangeGood.Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Orders {
    public class CreateOrderExchangeRequest {
        public string OwnerID { get; set; }
        public string ExchangerID { get; set; }
        public OrderDetailDto OwnerProduct { get; set; }
        public IEnumerable<OrderDetailDto> ExchangerProducts { get; set; }
    }
}
