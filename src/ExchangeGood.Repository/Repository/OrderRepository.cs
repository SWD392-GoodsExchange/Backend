using AutoMapper;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Enum.Order;
using ExchangeGood.Contract.Enum.OrderDetail;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IUnitOfWork _uow;
        public OrderRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _uow.OrderDAO.AddOrder(order);

            if (await _uow.SaveChangesAsync())
                return order;
            
            return null;
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
