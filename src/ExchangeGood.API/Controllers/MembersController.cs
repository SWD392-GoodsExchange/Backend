using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Role = ExchangeGood.Contract.Enum.Member.Role;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using Azure.Core;
using ExchangeGood.DAO;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Service.UseCase;

namespace ExchangeGood.API.Controllers {
    public class MembersController : BaseApiController {
        private readonly IMemberService _memberService;

        // private readonly HttpClient _httpClient;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IVnPayService _vnPayService;
        private readonly IMapper _mapper;

        public MembersController(IMemberService memberService, IOrderService orderService,
            IProductService productService, IVnPayService vnPayService, IMapper mapper) {
            _memberService = memberService;
            _orderService = orderService;
            _productService = productService;
            _vnPayService = vnPayService;
            _mapper = mapper;
            // _httpClient = httpClient;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin))]
        public async Task<IActionResult> GetMembers([FromQuery] GetMembersQuery getMembersQuery) {
            var result = await _memberService.GetAllMembers(getMembersQuery);
            return result != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG,
                    new PaginationResponse<MemberDto>(result, result.CurrentPage, result.PageSize, result.TotalCount,
                        result.TotalPages)))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) {
            var loginResponse = await _memberService.Login(loginRequest);

            return loginResponse != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, loginResponse))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordRequest passwordRequest) {
            passwordRequest.FeId = User.GetFeID();
            var isUpdate = await _memberService.UpdatePassword(passwordRequest);
            return isUpdate
                ? Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG,
                    nameof(UpdatePassword) + " successful"))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest createMemberRequest) {
            var loginResponse = await _memberService.CreateMember(createMemberRequest);
            return loginResponse != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, loginResponse))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("information")]
        [Authorize]
        public async Task<IActionResult> GetMemberById() {
            var feId = User.GetFeID();
            var member = await _memberService.GetMemberByFeId(feId);
            return member != null
                ? Ok(_mapper.Map<MemberDto>(member))
                : NotFound();
        }

        [HttpGet("bookmark")]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Member))]
        public async Task<IActionResult> GetBookMark() {
            var feId = User.GetFeID();
            var list = await _memberService.GetBookMarkByFeId(feId);
            return list.Count != 0
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<ProductDto>>(list)))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("bookmark")]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Member))]
        public async Task<IActionResult> CreateBookmark(CreateBookmarkRequest createBookmarkRequest) {
            createBookmarkRequest.FeId = User.GetFeID();
            var isAdd = await _memberService.CreateBookmark(createBookmarkRequest);
            return isAdd
                ? Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
        }

        [HttpDelete("bookmark/{productId}")]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Member))]
        public async Task<IActionResult> DeleteBookmark(int productId)
        {
            var deleteBookmarkRequest = new DeleteBookmarkRequest { 
                FeId = User.GetFeID(),
                ProductId = productId 
            };
            var isDelete = await _memberService.DeleteBookmark(deleteBookmarkRequest);
            return isDelete
                ? NoContent()
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG));
        }

        // Đặt hàng
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Member))]
        [HttpPost("checkout")] // làm lại với payment service
        public async Task<IActionResult> CheckoutOrder([FromBody] CreateOrderRequest createOrderRequest) {
            var feId = User.GetFeID();
            createOrderRequest.MemberId = feId;
            var result = await _orderService.CreateOrderForTrade(createOrderRequest);
            if (result == null) {
                return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
            }

            // after add order success => redirect to Payment action to create payment url
            return RedirectToAction(nameof(Payment), "members", new {
                amount = result.TotalAmount.ToString(),
                fullName = feId,
                orderId = result.OrderId.ToString()
            });
        }

        [HttpGet("Payment")]
        public async Task<IActionResult> Payment(string orderId, string amount, string fullName) {
            VnPaymentRequestModel vnPaymentRequestModel = new VnPaymentRequestModel {
                FullName = fullName,
                OrderId = orderId,
                Amount = Convert.ToDouble(amount),
                CreatedDate = DateTime.UtcNow,
                Description = $"Payment for the student's order with ID number {fullName}"
            };
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPaymentRequestModel);
            return !string.IsNullOrEmpty(paymentUrl)
                ? Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_PAYMENT_MSG,paymentUrl))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_PAYMENT_MSG));
        }

        [HttpGet("paymentCallback")]
        public async Task<IActionResult> PaymentCallback() {
            // sau khi thanh toán xong sẽ auto gen url để vào paymentCallback
            var response = _vnPayService.PaymentExecute(Request.Query);

            var updateOrderStatusCheck = response.ResponseCode != "00"
                ? await _orderService.UpdateOrderStatus(response.OrderId, false)
                : await _orderService.UpdateOrderStatus(response.OrderId);

            if (!updateOrderStatusCheck)
                return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));

            return (response.ResponseCode == "00")
                ? Ok(BaseResponse.Success(Const.SUCCESS_PAYMENT_CODE, Const.SUCCESS_PAYMENT_MSG))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_PAYMENT_MSG));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeOrder([FromBody] CreateOrderExchangeRequest createOrderRequest) {
            var feId = User.GetFeID();
            if (feId != createOrderRequest.OwnerID) throw new Exception();
            createOrderRequest.OwnerID = feId;
            var result = await _orderService.CreateOrdersForExchange(createOrderRequest);
            if (result) {
                return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG));
            }

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
        }

        // notification
        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("exchange/requests")]
        public async Task<IActionResult> GetAllRequestExchangesFromUserAndOtherUserRequestForUser() {
            var feId = User.GetFeID();
            var notifications = await _memberService.GetAllRequestExchangesFromUserAndOtherUserRequestForUser(feId);
            List<ExchangeRequestDto> result = new List<ExchangeRequestDto>();
            if (notifications != null) {
                foreach (var notification in notifications) {
                    try {
                        var products = await _productService.GetProductsForExchangeRequest(
                            new Contract.GetProductsForExchangeRequest {
                                OwnerId = notification.RecipientId,
                                ExchangerId = notification.SenderId,
                                ProductIds = GetProductIds(notification.OnwerProductId,
                                    notification.ExchangerProductIds),
                            });

                        result.Add(new ExchangeRequestDto {
                            NotificationId = notification.NotificationId,
                            SenderId = notification.SenderId,
                            SenderUsername = notification.SenderUsername,
                            RecipientId = notification.RecipientId,
                            RecipientUsername = notification.RecipientUsername,
                            OnwerProduct =
                                _mapper.Map<ProductDto>(
                                    products.SingleOrDefault(x => x.FeId == notification.RecipientId)),
                            ExchangerProducts =
                                _mapper.Map<IEnumerable<ProductDto>>(products.Where(x =>
                                    x.FeId == notification.SenderId).ToList()),
                            Content = notification.Content,
                            DateRead = notification.DateRead,
                            CreatedDate = notification.CreatedDate,
                        });
                    }
                    catch(BadRequestException ex) {
                        continue;
                    }
                    /*catch (Exception) {
                        continue;
                    }*/
                }
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result));
            }

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        private List<int> GetProductIds(string productId, string exchangeProductIds) {
            var result = new List<int>();
            result.Add(int.Parse(productId));
            result.AddRange(exchangeProductIds.Split(',').Select(x => int.Parse(x)));
            return result;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> ResetPassword(SendEmailRequest request) {
            var member = await _memberService.GetMemberByEmail(request.Email);
            if (member == null) {
                return NotFound("Email not found");
            }

            string resetLink = GenerateResetLink();
            bool emailSent = await _memberService.SendResetPasswordEmail(request.Email, resetLink);

            return Ok("Reset password email sent");
        }

        private string GenerateResetLink()
        {
            return "https://example.com/reset-password";
        }
        
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            var isUpdate = await _memberService.ResetPassword(resetPassword);
            return isUpdate
                ? Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG,
                    nameof(UpdatePassword) + " successful"))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }

        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Member))]
        [HttpGet("order")]
        public async Task<IActionResult> GetOrdersForMember()
        {
            var orderList = await _orderService.GetOrdersByFeId(User.GetFeID());
            var orderResponse = _mapper.Map<IEnumerable<OrderDto>>(orderList);
            return orderList.Any()
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG,
                    orderResponse))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("notifications")]
        [Authorize(Roles = nameof(Role.Member))]
        public async Task<IActionResult> GetNotificationsOfUser() {
            var feId = User.GetFeID();
            var notifications = await _memberService.GetNotificationsOfUser(feId);
            return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<IEnumerable<NotificationDto>>(notifications)));
        }

        [Authorize(Roles = nameof(Role.Member))]
        [HttpGet("getorder")]
        public async Task<IActionResult> GetOrderByOrderId([FromQuery] GetOrderRequest getOrderRequest)
        {
            getOrderRequest.FeId = User.GetFeID();
            var order = await _orderService.GetOrder(getOrderRequest);
            return order != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<OrderDto>(order)))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("top3PostingProducts")]
        public async Task<IActionResult> GetTop3PostingProducts()
        {
            var member = await _memberService.GetTop3PostingProducts();
            return member != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, member))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("top3PostingProductsTradeType")]
        public async Task<IActionResult> GetTop3PostingProductsTradeType()
        {
            var member = await _memberService.GetTop3PostingProductsTradeType();
            return member != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, member))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("totalAccounts")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> GetTotalAccount()
        {
            var result = await _memberService.GetTotalAccountsAsync();
            return result != 0
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

    }
}