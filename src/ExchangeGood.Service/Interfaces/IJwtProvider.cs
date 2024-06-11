using ExchangeGood.Data.Models;

namespace ExchangeGood.Service.Interfaces;

public interface IJwtProvider
{
    string Generate(Member member);
}