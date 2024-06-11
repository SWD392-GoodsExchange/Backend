using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.DTOs {
    public class ProductDto {
    public int ProductId { get; set; }

    public string FeId { get; set; }

    public int CateId { get; set; }

    public string UsageInformation { get; set; }

    public string Origin { get; set; }

    public string Status { get; set; }

    public decimal Price { get; set; }

    public string Title { get; set; }
    }
}
