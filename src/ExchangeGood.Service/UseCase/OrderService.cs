using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Contract.Enum.Order;
using ExchangeGood.Contract.Enum.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Repository;

namespace ExchangeGood.Service.UseCase
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IMemberService _memberService;
        private readonly IUnitOfWork _uow;

        public OrderService(IOrderRepository orderRepository, IProductService productService, IMemberService memberService, IUnitOfWork uow)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _memberService = memberService;
            _uow = uow;
        }

        public async Task<bool> CreateOrdersForExchange(CreateOrderExchangeRequest createOrderRequest) {
            var notification = await _memberService.GetNotificationsById(createOrderRequest.NotificationID);
            if(notification == null) {
                throw new BadRequestException("ExchangeRequest does not exist.");
            }

            if(notification.RecipientId != createOrderRequest.OwnerID) {
                throw new Exception("Wrong User.");
            }
            // checkout order type Exchange
            using var transaction = _uow.BeginTransaction();
            try {
                // create Order for owner who has a product for other want to exchange
                var product = await _productService.GetProduct(int.Parse(notification.OnwerProductId));
                if (product.Status == Contract.Enum.Product.Status.Sold.Name) throw new BadRequestException($"{product.Title} was sold out");
                product.Status = Contract.Enum.Product.Status.Sold.Name;
                /*if(product.FeId != createOrderRequest.OwnerID) {
                    throw new BadRequestException($"Product {product.ProductId} is not belong to user {createOrderRequest.OwnerID}");
                }*/
                // Create order for the sender -> it like the sender buy the recipient's product
                var orderForSender = new Order() {
                    BuyerId = notification.SenderId,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    TotalAmount = 0,
                    Type = Contract.Enum.Order.Type.Exchange.Name,
                    Status = Contract.Enum.Order.Status.Completed.Name,
                    TotalOrderDetails = 1,
                    OrderDetails = new List<OrderDetail>() { new OrderDetail() {
                        ProductId = int.Parse(notification.OnwerProductId),
                        SellerId = createOrderRequest.OwnerID,
                        Amount = 0,
                    }}
                };
                await _orderRepository.AddOrder(orderForSender);

                List<OrderDetail> orderDetailsOfOwner = new();
                foreach (var productExchangeId in notification.ExchangerProductIds.Split(',')) {
                    var exchangeProduct = await _productService.GetProduct(int.Parse(productExchangeId));
                    if (exchangeProduct.Status == Contract.Enum.Product.Status.Sold.Name) throw new BadRequestException($"{product.Title} was sold out");
                    exchangeProduct.Status = Contract.Enum.Product.Status.Sold.Name;

                    orderDetailsOfOwner.Add(new OrderDetail() {
                        ProductId = int.Parse(productExchangeId),
                        SellerId = notification.SenderId,
                        Amount = 0,
                    });
                }
                // create Order for recipiten who own the product and submit this request-> like that user buy the products of sender
                var orderForOwner = new Order() {
                    BuyerId = createOrderRequest.OwnerID,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    TotalAmount = 0,
                    Type = Contract.Enum.Product.Type.Exchange.Name,
                    Status = Contract.Enum.Order.Status.Completed.Name,
                    OrderDetails = orderDetailsOfOwner,
                    TotalOrderDetails = orderDetailsOfOwner.Count(),
                };
                await _orderRepository.AddOrder(orderForOwner);

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> UpdateOrderStatus(int orderId, bool isCompleted = true)
        {
            using var transaction = _uow.BeginTransaction();
                try
                {
                    // get order
                    Order order = await _orderRepository.GetOrder(orderId) ?? throw new OrderNotFoundException(orderId);
                    // check order is completed 
                    order.Status = isCompleted
                        ? Contract.Enum.Order.Status.Completed.Name
                        : Contract.Enum.Order.Status.Cancelled.Name;
                    
                    // check if order status is cancelled => set product status back to selling
                    if (order.Status.Equals(Contract.Enum.Order.Status.Cancelled.Name))
                    {
                        foreach (var orderDetail in order.OrderDetails)
                        {
                            var product = await _productService.GetProduct(orderDetail.ProductId) ??
                                          throw new ProductNotFoundException(orderDetail.ProductId);
                            product.Status = Contract.Enum.Product.Status.Selling.Name;
                        }
                    }
                    await _orderRepository.UpdateOrder(order);

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
        }

        public async Task<Order> CreateOrderForTrade(CreateOrderRequest createOrderRequest)
        {
            decimal totalAmount = 0;
            // Check Product and Update
            foreach (var orderDetail in createOrderRequest.OrderDetails)
            {
                // check product status
                Product product = await _productService.GetProduct(orderDetail.ProductId) ??
                                  throw new ProductNotFoundException(orderDetail.ProductId);
                if (product.Status.Equals(Contract.Enum.Product.Status.Sold.Name))
                {
                    continue;
                }

                // check buyer buy their product
                if (createOrderRequest.MemberId.Equals(orderDetail.SellerId))
                {
                    throw new SellerCanNotBuyTheirProductsException();
                }

                // check type order
                /*if (!product.Type.Equals(createOrderRequest.Type)) {
                    throw new InvalidOperationException();
                }*/
                // update status of product
                product.Status = Contract.Enum.Product.Status.Sold.Name;
                totalAmount += orderDetail.Amount;
            }

            // Add new Order
            var order = new Order()
            {
                BuyerId = createOrderRequest.MemberId,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = Contract.Enum.Order.Status.Processing.Name,
                Type = Contract.Enum.Order.Type.Trade.Name,
                TotalOrderDetails = createOrderRequest.OrderDetails.Count()
            };
            // Add OrderDetail for Order
            foreach (var orderDetailDto in createOrderRequest.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail()
                {
                    ProductId = orderDetailDto.ProductId,
                    SellerId = orderDetailDto.SellerId,
                    Amount = orderDetailDto.Amount,
                });
            }

            return await _orderRepository.AddOrder(order);
        }

        public Task<IEnumerable<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrdersByFeId(string feId)
        {
            return await _orderRepository.GetAllOrdersByFeId(feId);
        }

        public Task<Order> GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}