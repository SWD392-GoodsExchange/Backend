namespace ExchangeGood.Repository.Exceptions
{
    public class PasswordInvalidException : Exception
    {
        public PasswordInvalidException() : base("Password is invalid")
        {
        }
    }
}