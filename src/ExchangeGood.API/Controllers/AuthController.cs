using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using ExchangeGood.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
    [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
        {
            var loginResponse = await _authService.RefreshToken(refreshTokenRequest);
            if (loginResponse.IsSuccess)
            {
                return Ok(loginResponse);
            }
            return Unauthorized();
        }
    }
}   