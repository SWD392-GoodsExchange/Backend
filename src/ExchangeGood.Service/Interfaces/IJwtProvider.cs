using ExchangeGood.Data.Models;

namespace ExchangeGood.Service.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(Member member);
    string GenerateRefreshToken();
}