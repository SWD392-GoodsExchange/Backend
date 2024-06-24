using System.Security.Claims;

namespace ExchangeGood.Service.Interfaces
{
    public interface IClaimsPrincipalExtensions
    {
        ClaimsPrincipal? GetTokenPrincipal(string token);
    }
}