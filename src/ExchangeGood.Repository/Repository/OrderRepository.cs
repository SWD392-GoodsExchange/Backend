﻿using AutoMapper;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Enum.Order;
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

        public async Task<Order> UpdateOrder(Order order)
        {
            _uow.OrderDAO.UpdateOrder(order);
            return await _uow.SaveChangesAsync() ? order : default;
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await _uow.OrderDAO.GetOrder(orderId);
        }
    }
}
