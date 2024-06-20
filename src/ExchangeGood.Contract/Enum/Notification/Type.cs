namespace ExchangeGood.Contract.Enum.Notification
{
    public abstract class Type : Enumeration<Type>
    {
        public  static readonly Type Send = new SendType();
        public  static readonly Type Receive = new ReceiveType();
        
        protected Type(int value, string name) : base(value, name)
        {
        }
        private sealed class SendType : Type
        {
            public SendType() : base(1, nameof(Send))
            {
            }
        }
        
        private sealed class ReceiveType : Type
        {
            public ReceiveType() : base(2, nameof(Receive))
            {
            }
        }
    }
}