using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces {
    public interface IOrderService {
        public Task<BaseResponse> GetAllOrders();
        public Task<BaseResponse> GetOrder(int orderId);
        public Task<BaseResponse> AddOrder(CreateOrderRequest createOrderRequest);
    }
}
