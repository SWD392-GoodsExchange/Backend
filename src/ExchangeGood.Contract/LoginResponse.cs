namespace ExchangeGood.Contract.Payloads.Response
{
    public class LoginResponse
    {
        public string JwtToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
} 