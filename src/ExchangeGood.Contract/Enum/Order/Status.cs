namespace ExchangeGood.Contract.Enum.Order
{
    public abstract class Status : Enumeration<Order.Status>
    {
        public static readonly Order.Status Pending = new PendingStatus();
        public static readonly Order.Status Delivering = new DeliveringStatus();
        public static readonly Order.Status Delivered = new DeliveredStatus();
        public static readonly Order.Status Cancelled = new CancelledStatus();

        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class CancelledStatus : Order.Status
        {
            public CancelledStatus() : base(4, nameof(Cancelled))
            {
            }
        }
        
        private sealed class DeliveredStatus : Order.Status
        {
            public DeliveredStatus() : base(3, nameof(Delivered))
            {
            }
        }
        private sealed class DeliveringStatus : Order.Status
        {
            public DeliveringStatus() : base(2, nameof(Delivering))
            {
            }
        }
        private sealed class PendingStatus : Order.Status
        {
            public PendingStatus() : base(1, nameof(Pending))
            {
            }
        }
    }
}