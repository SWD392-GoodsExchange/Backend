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
    public class OrderRepository : IOrderRepository {
        private readonly IUnitOfWork _uow;
        // private readonly IMapper _mapper;
        public OrderRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> AddOrder(Order order) {
            _uow.OrderDAO.AddOrder(order);
            
            return await _uow.SaveChangesAsync();
        }

        public Task<PagedList<Order>> GetAllOrders() {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(int orderId) {
            throw new NotImplementedException();
        }
    }
}
