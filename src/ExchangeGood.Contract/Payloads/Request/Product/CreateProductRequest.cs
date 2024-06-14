using ExchangeGood.Contract.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request.Product
{
    public class CreateProductRequest
    {
        public string FeId { get; set; }

        public int CateId { get; set; }

        public string UsageInformation { get; set; }

        public string Origin { get; set; }

        public string Price { get; set; }

        public string Title { get; set; }

        public IFormFile File { get; set; }

        public ImageDto Image { get; set; }
    }
}
