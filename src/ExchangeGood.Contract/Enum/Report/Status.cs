namespace ExchangeGood.Contract.Enum.Report
{
    public abstract class Status : Enumeration<Report.Status>
    {
        public  static readonly Report.Status Processing = new ProcessingStatus();
        public  static readonly Report.Status Approved = new ApprovedStatus();
        public  static readonly Report.Status Rejected = new RejectedStatus();

        protected Status(int value, string name) : base(value, name)
        {
        }
        private sealed class ProcessingStatus : Report.Status
        {
            public ProcessingStatus() : base(1, nameof(Processing))
            {
            }
        }
        private sealed class ApprovedStatus : Report.Status
        {
            public ApprovedStatus() : base(2, nameof(Approved))
            {
            }
        }
        private sealed class RejectedStatus : Report.Status
        {
            public RejectedStatus() : base(2, nameof(Rejected))
            {
            }
        }

    }
}