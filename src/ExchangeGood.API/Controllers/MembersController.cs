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

namespace ExchangeGood.API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IMemberService _memberService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public MembersController(IMemberService memberService, IOrderService orderService, IMapper mapper)
        {
            _memberService = memberService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Contract.Enum.Member.Role.Admin.Name))]
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

        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest createMemberRequest)
        {
            var feId = await _memberService.CreateMember(createMemberRequest);
            return CreatedAtAction(nameof(GetMemberById), feId);
        }

        [HttpGet("information")]
        [Authorize]
        public async Task<IActionResult> GetMemberById()
        {
            var feId = User.GetFeID();
            var member = await _memberService.GetMemberByFeId(feId);
            return member != null
                ? Ok(member)
                : NotFound(member);
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
        [HttpPost("checkout")]
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
        [HttpGet("notifications-sended")]
        public async Task<IActionResult> GetNotificationsSendedByUser()
        {
            var feId = User.GetFeID();
            var result = await _memberService.GetNotificationsWereSendedByUser(feId);
            if (result != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG,
                    _mapper.Map<IEnumerable<NotificationDto>>(result)));
            }

            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }
    }
}