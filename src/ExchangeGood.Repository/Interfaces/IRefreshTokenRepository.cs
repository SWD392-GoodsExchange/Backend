using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetRefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<RefreshToken> GetRefreshTokenByFeId(string feId);
        Task<bool> AddRefreshToken(RefreshToken refreshToken);
        Task<bool> UpdateRefreshToken(RefreshToken refreshToken);
    }
}