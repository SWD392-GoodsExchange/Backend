﻿using ExchangeGood.Contract.Common;
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
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Repository;

namespace ExchangeGood.Service.UseCase
{
    public class OrderService : IOrderService {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _uow;

        public OrderService(IOrderRepository orderRepository, IProductService productService, IUnitOfWork uow)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _uow = uow;
        }

        public async Task<bool> CreateOrdersForExchange(CreateOrderExchangeRequest createOrderRequest) {
            // checkout order type Exchange
            using var transaction = _uow.BeginTransaction();
            try {
                // create Order for seller who has a product for other want to exchange
                var product = await _productService.GetProduct(createOrderRequest.OwnerProduct.ProductId) ?? throw new ProductNotFoundException(createOrderRequest.OwnerProduct.ProductId);
                product.Status = Contract.Enum.Product.Status.Sold.Name;
                var orderOwner = new Order() {
                    BuyerId = createOrderRequest.ExchangerID,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    TotalAmount = 0,
                    Type = Contract.Enum.Order.Type.Exchange.Name,
                    Status = Contract.Enum.Order.Status.Proccessing.Name,
                    TotalOrderDetails = 1,
                    OrderDetails = new List<OrderDetail>() { new OrderDetail() {
                        ProductId = createOrderRequest.OwnerProduct.ProductId,
                        SellerId = createOrderRequest.OwnerID,
                        Amount = 0,
                    }}
                };
                await _orderRepository.AddOrder(orderOwner);

                // create Order for exchanger
                var orderExchanger = new Order() {
                    BuyerId = createOrderRequest.ExchangerID,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    TotalAmount = 0,
                    Type = Contract.Enum.Product.Type.Exchange.Name,
                    Status = Contract.Enum.Order.Status.Proccessing.Name,
                    TotalOrderDetails = 1,
                    OrderDetails = createOrderRequest.ExchangerProducts.Select(p => new OrderDetail() {
                        ProductId = p.ProductId,
                        SellerId = p.SellerId,
                        Amount = 0,
                    }).ToList()
                };
                foreach (var orderDetailDto in createOrderRequest.ExchangerProducts) {
                    var exchangeProduct = await _productService.GetProduct(orderDetailDto.ProductId) ?? throw new ProductNotFoundException(createOrderRequest.OwnerProduct.ProductId);
                    exchangeProduct.Status = Contract.Enum.Product.Status.Sold.Name;
                }
                await _orderRepository.AddOrder(orderExchanger);
            
                transaction.Commit();
                return true;
            }
            catch (Exception) {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<Order> CreateOrderForTrade(CreateOrderRequest createOrderRequest) {
            decimal totalAmount = 0;
            // Check Product and Update
            foreach (var orderDetail in createOrderRequest.OrderDetails) {
                // check product status
                Product product = await _productService.GetProduct(orderDetail.ProductId) ?? throw new ProductNotFoundException(orderDetail.ProductId);
                if(product.Status.Equals(Contract.Enum.Product.Status.Sold.Name)) {
                    continue;
                }
                // check buyer buy their product
                if (createOrderRequest.MemberId.Equals(orderDetail.SellerId)) {
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
            var order = new Order() {
                BuyerId = createOrderRequest.MemberId,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = Contract.Enum.Order.Status.Proccessing.Name,
                Type = Contract.Enum.Order.Type.Trade.Name,
                TotalOrderDetails = createOrderRequest.OrderDetails.Count()
            };
            // Add OrderDetail for Order
            foreach (var orderDetailDto in createOrderRequest.OrderDetails) {
                order.OrderDetails.Add(new OrderDetail() {
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

        public Task<Order> GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}