using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
    public class TestController : BaseApiController
    {
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok();
        }

            [HttpGet("AuthorizedTest")]
        [Authorize]
        public IActionResult AuthorizedTest()
        {
            var authorizationHeader = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            string jwtTokenString = authorizationHeader.Replace("BEARER ", "");

            var jwt = new JwtSecurityToken(jwtTokenString);

            var response = $"Authenticated!{Environment.NewLine}";

            response += $"{Environment.NewLine}Exp Time: {jwt.ValidTo.ToLongTimeString()}, Time: {DateTime.UtcNow.ToLongTimeString()}";

            return Ok(response);
        }

    }
}