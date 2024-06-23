using System.Security.Claims;

namespace ExchangeGood.API.Extensions {
    public static class ClaimsPrincipalExtensions {
        public static string GetFeID(this ClaimsPrincipal user) {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
