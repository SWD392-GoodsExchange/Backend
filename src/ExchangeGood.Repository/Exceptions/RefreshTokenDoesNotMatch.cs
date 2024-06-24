namespace ExchangeGood.Repository.Exceptions
{
    public class RefreshTokenDoesNotMatch : BadRequestException
    {
        public RefreshTokenDoesNotMatch() : base("In correct refresh token")
        {
        }
    }
}