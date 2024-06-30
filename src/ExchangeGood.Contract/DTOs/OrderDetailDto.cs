using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs {
    public class OrderDetailDto {
        public int ProductId { get; set; }
        public string SellerId { get; set; }
        public decimal Amount { get; set; }
    }
}
