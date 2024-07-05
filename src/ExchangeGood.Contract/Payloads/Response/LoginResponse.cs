namespace ExchangeGood.Contract.Payloads.Response
{
    public class LoginResponse
    {
        public string FeId { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

        public string JwtToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
} 