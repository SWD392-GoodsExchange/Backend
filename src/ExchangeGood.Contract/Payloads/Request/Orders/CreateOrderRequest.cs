using ExchangeGood.Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Orders {
    public class CreateOrderRequest {
        public string FeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Type { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
