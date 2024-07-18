using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs {
    public class OrderDto {
        public int OrderId { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public decimal? TotalAmount { get; set; }

        public int? TotalOrderDetails { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
        
        public List<OrderDetailDto> OrderDetails { get; set; }

    }
}
