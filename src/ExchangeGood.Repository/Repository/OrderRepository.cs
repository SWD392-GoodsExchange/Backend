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

namespace ExchangeGood.Repository.Repository {
    public class OrderRepository : IOrderRepository {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public async Task<Order> AddOrder(CreateOrderRequest createOrderRequest) {
            // Add new Order
            var order = new Order() {
                BuyerId = createOrderRequest.BuyerId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                TotalAmount = createOrderRequest.TotalAmount,
                Status = Contract.Enum.Order.Status.Pending.Name,
                TotalOrderDetails = createOrderRequest.OrderDetails.Count()
            };
            // Add OrderDetail for Order
            foreach(var orderDetailDto in createOrderRequest.OrderDetails) {
                order.OrderDetails.Add(new OrderDetail() 
                {   
                    ProductId = orderDetailDto.ProductId, 
                    Amount = orderDetailDto.Amount,
                    Quantity = orderDetailDto.Quantity,
                    Status = Contract.Enum.OrderDetail.Status.Pending.Name
                });
            }

            _uow.OrderDAO.AddOrder(order);
            if(await _uow.SaveChangesAsync()) 
                return order;

            return null;
        }

        public Task<PagedList<Order>> GetAllOrders() {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(int orderId) {
            throw new NotImplementedException();
        }
    }
}
