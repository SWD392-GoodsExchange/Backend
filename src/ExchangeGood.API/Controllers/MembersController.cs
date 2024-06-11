using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers {
    public class MembersController : BaseApiController
    {
        private readonly IMemberService _memberService;

        public MembersController( IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers([FromQuery]GetMembersQuery getMembersQuery)
        {
            var result = await _memberService.GetAllMembers(getMembersQuery);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
