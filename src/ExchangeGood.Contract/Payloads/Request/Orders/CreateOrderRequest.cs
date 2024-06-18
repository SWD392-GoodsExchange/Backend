using ExchangeGood.Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Orders {
    public class CreateOrderRequest {

        public string MemberId { get; set; }
        public string Type { get; set; }  
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
