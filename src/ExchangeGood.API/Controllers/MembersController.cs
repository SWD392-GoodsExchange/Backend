using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers {
    public class MembersController : BaseApiController
    {
        private readonly IMemberService _memberService;
        private readonly IOrderService _orderService;

        public MembersController(IMemberService memberService,IOrderService orderService)
        {
            _memberService = memberService;
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetMembers([FromQuery] GetMembersQuery getMembersQuery)
        {
            var result = await _memberService.GetAllMembers(getMembersQuery);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _memberService.Login(loginRequest);
            return Ok(result);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordRequest passwordRequest)
        {
            passwordRequest.FeId = User.GetFeID();
            var result = await _memberService.UpdatePassword(passwordRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest createMemberRequest)
        {
            var result = await _memberService.CreateMember(createMemberRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetMemberById), result.Data);
        }

        [HttpGet("information")]
        public async Task<IActionResult> GetMemberById()
        {
            var feId = User.GetFeID();
            var member = await _memberService.GetMemberByFeId(feId);
            return member.IsSuccess
                ? Ok(member)
                : NotFound(member);
        }

        [HttpGet("bookmark")]
        public async Task<IActionResult> GetBookMark()
        {
            var feId = User.GetFeID();
            var list = await _memberService.GetBookMarkByFeId(feId);
            return list.IsSuccess
                ? Ok(list)
                : BadRequest(list);
        }

        [HttpPost("bookmark")]
        public async Task<IActionResult> CreateBookmark(CreateBookmarkRequest createBookmarkRequest)
        {
            createBookmarkRequest.FeId = User.GetFeID();
            var result = await _memberService.CreateBookmark(createBookmarkRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("bookmark")]
        public async Task<IActionResult> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest)
        {
            deleteBookmarkRequest.FeId = User.GetFeID();
            var result = await _memberService.DeleteBookmark(deleteBookmarkRequest);
            return result.IsSuccess ? NoContent() : BadRequest(result);
        }

        // Đặt hàng
        [Authorize(Roles = "Member")]
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutOrder([FromBody] CreateOrderRequest createOrderRequest) {
            var feId = User.GetFeID();
            createOrderRequest.MemberId = feId;
            var result = await _orderService.CreateOrderForTrade(createOrderRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeOrder([FromBody] CreateOrderExchangeRequest createOrderRequest) {
            var feId = User.GetFeID();
            createOrderRequest.OwnerID = feId;
            var result = await _orderService.CreateOrderForExchange(createOrderRequest);
            if (!result.IsSuccess) {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}