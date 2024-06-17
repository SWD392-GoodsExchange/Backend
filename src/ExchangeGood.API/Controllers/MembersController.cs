using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Request.Orders;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
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

        public MembersController(IMemberService memberService, IOrderService orderService)
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
                : BadRequest(member);
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

        // Đặt hàng
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutOrder([FromBody] CreateOrderRequest createOrderRequest) {
            var result = await _orderService.AddOrder(createOrderRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}