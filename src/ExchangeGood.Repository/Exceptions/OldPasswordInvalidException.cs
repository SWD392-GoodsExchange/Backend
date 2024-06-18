namespace ExchangeGood.Repository.Exceptions
{
    public class OldPasswordInvalidException : Exception
    {
        public OldPasswordInvalidException() : base("Old Password is incorrect")
        {
        }
    }
}