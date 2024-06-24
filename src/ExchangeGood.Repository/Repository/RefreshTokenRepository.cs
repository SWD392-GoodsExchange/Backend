using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;
using Microsoft.VisualBasic;

namespace ExchangeGood.Repository.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IUnitOfWork _uow;

        public RefreshTokenRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RefreshToken> GetRefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = await _uow.RefreshTokenDAO.GetRefreshToken(refreshTokenRequest.FeId);
            if (refreshToken == null)
            {
                throw new RefreshTokenNotFoundException(refreshTokenRequest.FeId);
            }
            // check refresh token có trùng với request refresh token không?
            if (refreshTokenRequest.RefreshToken != refreshToken.Token)
            {
                throw new RefreshTokenDoesNotMatch();
            }
            // Refresh Token hết hạn => login lại để cấp refresh token mới
            if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new RefreshTokenIsExpiredException();
            }

            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshTokenByFeId(string feId)
        {
            var refreshToken = await _uow.RefreshTokenDAO.GetRefreshToken(feId);
            return (refreshToken != null) ? refreshToken : default;
        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            _uow.RefreshTokenDAO.Add(refreshToken);
            return await _uow.SaveChangesAsync();
        }

        public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
        {
            _uow.RefreshTokenDAO.Update(refreshToken);
            return await _uow.SaveChangesAsync();
        }
    }
}