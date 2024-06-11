namespace ExchangeGood.Repository.Exceptions;

public class NotFoundException : EntityException {
    public NotFoundException(string message) : base(message)
    { 
    }
}