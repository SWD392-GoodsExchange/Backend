namespace ExchangeGood.Contract.Enum.Product
{
    public abstract class Status : Enumeration<Product.Status>
    {
        public static readonly Product.Status Sale = new SaleStatus();
        public static readonly Product.Status Sold = new SoldStatus();
        
        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class SaleStatus : Product.Status
        {
            public SaleStatus() : base(1, nameof(Sale))
            {
            }
        }
        private sealed class SoldStatus : Product.Status
        {
            public SoldStatus() : base(2, nameof(Sold))
            {
            }
        }
    }
}