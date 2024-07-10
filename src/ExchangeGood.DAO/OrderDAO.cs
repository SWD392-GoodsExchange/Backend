using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.DAO {
    public class OrderDAO {
        private readonly GoodsExchangeContext _context;
        public OrderDAO(GoodsExchangeContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder(int orderId) {
            var order = await _context.Orders.FindAsync(orderId);

            if (order != null) {
                await _context.Entry(order)
                    .Collection(i => i.OrderDetails).LoadAsync();
            }

            return order;
        }

        public void AddOrder(Order order) {
            _context.Orders.Add(order);
        }

        public void DeleteOrder(Order order) {
            _context.Orders.Remove(order);
        }

        public void UpdateOrder(Order order) { 
            _context.Orders.Update(order);
        }
    }
}
