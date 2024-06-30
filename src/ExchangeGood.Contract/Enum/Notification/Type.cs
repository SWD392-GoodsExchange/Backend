namespace ExchangeGood.Contract.Enum.Notification
{
    public abstract class Type : Enumeration<Type>
    {
        public  static readonly Type ExchangeRequest = new ExchangeRequestType();
        public  static readonly Type Notification = new NotificationType();
        
        protected Type(int value, string name) : base(value, name)
        {
        }
        private sealed class ExchangeRequestType : Type
        {
            public ExchangeRequestType() : base(1, nameof(ExchangeRequest))
            {
            }
        }
        
        private sealed class NotificationType : Type
        {
            public NotificationType() : base(2, nameof(Notification))
            {
            }
        }
    }
}