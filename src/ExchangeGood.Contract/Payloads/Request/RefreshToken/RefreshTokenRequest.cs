namespace ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken
{
    public class RefreshTokenRequest
    {
        public string FeId { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}