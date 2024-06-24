using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;

namespace ExchangeGood.Service.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}
