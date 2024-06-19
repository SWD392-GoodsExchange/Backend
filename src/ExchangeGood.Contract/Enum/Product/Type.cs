namespace ExchangeGood.Contract.Enum.Product
{
    public abstract class Type : Enumeration<Product.Type>
    {
        public static readonly Product.Type Exchange = new ExchangeType();
        public static readonly Product.Type Trade = new TradeType();
        
        protected Type(int value, string name) : base(value, name)
        {
        }
        private sealed class ExchangeType : Product.Type
        {
            public ExchangeType() : base(1, nameof(Exchange))
            {
            }
        }
        
        private sealed class TradeType : Product.Type
        {
            public TradeType() : base(2, nameof(Trade))
            {
            }
        }
    }
}