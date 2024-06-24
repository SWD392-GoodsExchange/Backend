namespace ExchangeGood.Repository.Exceptions
{
    public class RefreshTokenIsExpiredException : BadRequestException
    {
        public RefreshTokenIsExpiredException() : base("Refresh Token were Expired")
        {
        }
    }
}