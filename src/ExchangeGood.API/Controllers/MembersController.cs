using AutoMapper;
using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Role = ExchangeGood.Contract.Enum.Member.Role;
using ExchangeGood.Contract.Payloads.Request.Notification;

namespace ExchangeGood.API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IMemberService _memberService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public MembersController(IMemberService memberService, IOrderService orderService, IProductService productService, IMapper mapper)
        {
            _memberService = memberService;
            _orderService = orderService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin))]
        public async Task<IActionResult> GetMembers([FromQuery] GetMembersQuery getMembersQuery)
        {
            var result = await _memberService.GetAllMembers(getMembersQuery);
            return result != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG,
                    new PaginationResponse<MemberDto>(result, result.CurrentPage, result.PageSize, result.TotalCount,
                        result.TotalPages)))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = await _memberService.Login(loginRequest);

            return loginResponse != null 
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, loginResponse)) 
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordRequest passwordRequest)
        {
            passwordRequest.FeId = User.GetFeID();
            var isUpdate = await _memberService.UpdatePassword(passwordRequest);
            return isUpdate 
                ? Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, nameof(UpdatePassword) + " successful"))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest createMemberRequest)
        {
            var loginResponse = await _memberService.CreateMember(createMemberRequest);
            return loginResponse != null
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, loginResponse))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("information")]
        [Authorize]
        public async Task<IActionResult> GetMemberById()
        {
            var feId = User.GetFeID();
            var member = await _memberService.GetMemberByFeId(feId);
            return member != null
                ? Ok(_mapper.Map<MemberDto>(member))
                : NotFound();
        }

        [HttpGet("bookmark")]
        [Authorize]
        public async Task<IActionResult> GetBookMark()
        {
            var feId = User.GetFeID();
            var list = await _memberService.GetBookMarkByFeId(feId);
            return list.Count != 0
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, list))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpPost("bookmark")]
        [Authorize]
        public async Task<IActionResult> CreateBookmark(CreateBookmarkRequest createBookmarkRequest)
        {
            createBookmarkRequest.FeId = User.GetFeID();
            var isAdd = await _memberService.CreateBookmark(createBookmarkRequest);
            return isAdd 
                ? Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG))
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE,Const.FAIL_CREATE_MSG));
        }

        [HttpDelete("bookmark")]
        [Authorize]
        public async Task<IActionResult> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest)
        {
            deleteBookmarkRequest.FeId = User.GetFeID();
            var isDelete = await _memberService.DeleteBookmark(deleteBookmarkRequest);
            return isDelete 
                ? NoContent() 
                : BadRequest(BaseResponse.Failure(Const.FAIL_CODE,Const.FAIL_DELETE_MSG));
        }

        // Đặt hàng
        [Authorize(Roles = "Member")]
        [HttpPost("checkout")] // làm lại với payment service
        public async Task<IActionResult> CheckoutOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            var feId = User.GetFeID();
            createOrderRequest.MemberId = feId;
            var result = await _orderService.CreateOrderForTrade(createOrderRequest);
            if (result == null)
            {
                return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
            }

            return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG,
                _mapper.Map<OrderDto>(result)));
        }

        [Authorize(Roles = "Member")]
        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeOrder([FromBody] CreateOrderExchangeRequest createOrderRequest)
        {
            var feId = User.GetFeID();
            createOrderRequest.OwnerID = feId;
            var result = await _orderService.CreateOrdersForExchange(createOrderRequest);
            if (result)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG));
            }

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
        }

        // notification
        [Authorize(Roles = "Member")]
        [HttpGet("exchange/requests")]
        public async Task<IActionResult> GetAllRequestExchangesFromUserAndOtherUserRequestForUser()
        {
            var feId = User.GetFeID();
            var notifications = await _memberService.GetAllRequestExchangesFromUserAndOtherUserRequestForUser(feId);
            List<ExchangeRequestDto> result = null;
            if (notifications != null)
            {
                foreach (var notification in notifications)
                {
                    try
                    {
                        var products = await _productService.GetProductsForExchangeRequest(new Contract.GetProductsForExchangeRequest
                        {
                            OwnerId = notification.RecipientId,
                            ExchangerId = notification.SenderId,
                            ProductIds = GetProductIds(notification.OnwerProductId, notification.ExchangerProductIds),
                        });

                        result.Add(new ExchangeRequestDto
                        {
                            SenderId = notification.SenderId,
                            SenderUsername = notification.SenderUsername,
                            RecipientId = notification.RecipientId,
                            RecipientUsername = notification.RecipientUsername,
                            OnwerProduct = _mapper.Map<ProductDto>(products.SingleOrDefault(x => x.FeId == notification.RecipientId)),
                            ExchangerProducts = _mapper.Map<IEnumerable<ProductDto>>(products.Select(x => x.FeId == notification.SenderId)),
                            Content = notification.Content,
                            DateRead = notification.DateRead,
                            CreatedDate = notification.CreatedDate,
                        });
                    }
                    catch (System.Exception)
                    {
                           continue;
                    }
                }
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result));
            }

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        private List<int> GetProductIds(string productId, string exchangeProductIds)
        {
            var result = new List<int>(int.Parse(productId));
            result.AddRange(exchangeProductIds.Split(',').Select(x => int.Parse(x)));
            return result;
        }
    }
}