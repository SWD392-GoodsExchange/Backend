using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.DAO
{
    public class RefreshTokenDAO
    {
        private readonly GoodsExchangeContext _goodsExchangeContext;
        public RefreshTokenDAO(GoodsExchangeContext goodsExchangeContext)
        {
            _goodsExchangeContext = goodsExchangeContext;
        }

        public void Add(RefreshToken refreshToken)
        {
            _goodsExchangeContext.RefreshTokens.Add(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(string feId)
        {
            // Lấy refresh token ra
            return await _goodsExchangeContext.RefreshTokens
                .Include(rf => rf.Fe)
                .ThenInclude(member => member.Role)
                .FirstOrDefaultAsync(rf => rf.FeId == feId);
        }
        public void Delete(RefreshToken refreshToken)
        {
            _goodsExchangeContext.RefreshTokens.Remove(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _goodsExchangeContext.RefreshTokens.Update(refreshToken);
        }
        
    }
}