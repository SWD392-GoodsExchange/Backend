namespace ExchangeGood.Repository.Exceptions
{
    public class RefreshTokenNotFoundException : NotFoundException
    {
        public RefreshTokenNotFoundException(string feId) : base($"Refresh token of {feId} was not found")
        {
        }
    }
}