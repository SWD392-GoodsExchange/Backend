namespace ExchangeGood.Repository.Exceptions;

public class BadRequestException : EntityException {
    public BadRequestException(string message) : base(message)
    { 
    }
}