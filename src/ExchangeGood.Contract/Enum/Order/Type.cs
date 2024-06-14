namespace ExchangeGood.Contract.Enum.Order
{
    public abstract class Type : Enumeration<Order.Type>
    {
        public static readonly Order.Type Exchange = new ExchangeType();
        public static readonly Order.Type Trade = new TradeType();
        
        protected Type(int value, string name) : base(value, name)
        {
        }
        private sealed class ExchangeType : Order.Type
        {
            public ExchangeType() : base(1, nameof(Exchange))
            {
            }
        }
        
        private sealed class TradeType : Order.Type
        {
            public TradeType() : base(2, nameof(Trade))
            {
            }
        }
    }
}