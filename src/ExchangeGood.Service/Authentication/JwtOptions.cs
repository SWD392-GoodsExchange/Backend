namespace ExchangeGood.Service.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public TimeSpan ExpiryTimeFrame { get; init; }
}