namespace ExchangeGood.Contract.Enum.Product
{
    public abstract class Status : Enumeration<Product.Status>
    {
        public static readonly Product.Status Selling = new SellingStatus();
        public static readonly Product.Status Processing = new ProcessingStatus();
        public static readonly Product.Status Sold = new SoldStatus();
        
        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class SellingStatus : Product.Status
        {
            public SellingStatus() : base(1, nameof(Selling))
            {
            }
        }
        private sealed class ProcessingStatus : Product.Status
        {
            public ProcessingStatus() : base(2, nameof(Processing))
            {
                
            }
        }
        private sealed class SoldStatus : Product.Status
        {
            public SoldStatus() : base(3, nameof(Sold))
            {
            }
        }
    }
}