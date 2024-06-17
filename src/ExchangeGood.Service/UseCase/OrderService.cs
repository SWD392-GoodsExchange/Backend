using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.UseCase {
    public class OrderService : IOrderService {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse> AddOrder(CreateOrderRequest createOrderRequest) {
            // check user buy their product
            
            // logic total amount of Order
            decimal totalAmount = 0;
            foreach(var orderDetail in createOrderRequest.OrderDetails) {
                totalAmount += orderDetail.Quantity * orderDetail.Amount;
            }
            createOrderRequest.TotalAmount = totalAmount;
            var order = await _orderRepository.AddOrder(createOrderRequest);
            if (order == null) {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
            }

            return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, order);
        }

        public Task<BaseResponse> GetAllOrders() {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetOrder(int orderId) {
            throw new NotImplementedException();
        }
    }
}
