using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions {
    public class ProductNotFoundException : NotFoundException {
        public ProductNotFoundException(int id) : base($"Product with id {id} is not found.") {
        }
    }
}
