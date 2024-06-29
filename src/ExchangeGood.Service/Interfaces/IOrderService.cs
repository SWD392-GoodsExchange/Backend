using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces {
    public interface IOrderService {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> GetOrder(int orderId);
        public Task<Order> CreateOrderForTrade(CreateOrderRequest createOrderRequest);
        public Task<bool> CreateOrdersForExchange(CreateOrderExchangeRequest createOrderRequest);
    }
}
