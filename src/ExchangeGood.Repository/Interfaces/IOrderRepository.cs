using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces {
    public interface IOrderRepository {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<Order> GetOrder(int orderId);
        public Task<Order> AddOrder(Order order);
    }
}
