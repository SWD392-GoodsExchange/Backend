using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Enum.Order;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository {
    public class OrderRepository : IOrderRepository {
        public async Task<Order> AddOrder(CreateOrderRequest createOrderRequest) {
            var order = new Order() {
                FeId = createOrderRequest.FeId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Type = createOrderRequest.Type,
                Status = Status.Pending.Name,
            };

            return order;
        }

        public Task<PagedList<Order>> GetAllOrders() {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(int orderId) {
            throw new NotImplementedException();
        }
    }
}
