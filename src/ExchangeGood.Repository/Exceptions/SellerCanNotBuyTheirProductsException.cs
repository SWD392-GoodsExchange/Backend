using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions {
    public class SellerCanNotBuyTheirProductsException : BadRequestException {
        public SellerCanNotBuyTheirProductsException() : base("Seller can not buy their products") {
        }
    }
}
