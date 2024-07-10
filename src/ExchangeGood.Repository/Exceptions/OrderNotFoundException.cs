namespace ExchangeGood.Repository.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int orderId) : base($"Order with id {orderId} not found")
        {
        }
    }
}