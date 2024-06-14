namespace ExchangeGood.Contract.Enum.Member;

public abstract class Status : Enumeration<Status>
{
    public static readonly Status Banned = new BannedStatus();
    public static readonly Status Available = new AvailableStatus();
    protected Status(int value, string name) : base(value, name)
    {
    }
    private sealed class BannedStatus : Status
    {
        public BannedStatus() : base(1, nameof(Banned))
        {
        }
    }
    private sealed class AvailableStatus : Status
    {
        public AvailableStatus() : base(2, nameof(Available))
        {
        }
    }
}