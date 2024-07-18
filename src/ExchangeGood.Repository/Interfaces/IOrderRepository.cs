using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces {
    public interface IOrderRepository {
        public Task<IEnumerable<Order>> GetAllOrders(string feId);
        public Task<Order> GetOrder(int orderId, params Expression<Func<Order, bool>>[] validateField);
        public Task<IEnumerable<Order>> GetAllOrdersByFeId(string feId);
        public Task<Order> AddOrder(Order order);
        public Task<Order> UpdateOrder(Order order);
    }
}
