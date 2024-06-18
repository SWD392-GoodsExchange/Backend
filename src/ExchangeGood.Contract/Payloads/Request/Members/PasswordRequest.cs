namespace ExchangeGood.Contract.Payloads.Request.Members
{
    public class PasswordRequest
    {
        public string FeId { get; set; }
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }
        
    }
}