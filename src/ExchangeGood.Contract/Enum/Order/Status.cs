namespace ExchangeGood.Contract.Enum.Order
{
    public abstract class Status : Enumeration<Order.Status>
    {
        public static readonly Order.Status Proccessing = new ProccessingStatus();
        public static readonly Order.Status Completed = new CompletedStatus();
        public static readonly Order.Status Cancelled = new CancelledStatus();

        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class CancelledStatus : Order.Status
        {
            public CancelledStatus() : base(3, nameof(Cancelled))
            {
            }
        }
        private sealed class CompletedStatus : Order.Status
        {
            public CompletedStatus() : base(2, nameof(Completed))
            {
            }
        }
        private sealed class ProccessingStatus : Order.Status
        {
            public ProccessingStatus() : base(1, nameof(Proccessing))
            {
            }
        }
    }
}