namespace ExchangeGood.Contract.Enum.OrderDetail
{
    public abstract class Status : Enumeration<OrderDetail.Status>
    {
        public static readonly OrderDetail.Status Pending = new PendingStatus();
        public static readonly OrderDetail.Status Delivering = new DeliveringStatus();
        public static readonly OrderDetail.Status Delivered = new DeliveredStatus();
        public static readonly OrderDetail.Status Cancelled = new CancelledStatus();

        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class CancelledStatus : OrderDetail.Status
        {
            public CancelledStatus() : base(4, nameof(Cancelled))
            {
            }
        }
        
        private sealed class DeliveredStatus : OrderDetail.Status
        {
            public DeliveredStatus() : base(3, nameof(Delivered))
            {
            }
        }
        private sealed class DeliveringStatus : OrderDetail.Status
        {
            public DeliveringStatus() : base(2, nameof(Delivering))
            {
            }
        }
        private sealed class PendingStatus : OrderDetail.Status
        {
            public PendingStatus() : base(1, nameof(Pending))
            {
            }
        }
    }
}