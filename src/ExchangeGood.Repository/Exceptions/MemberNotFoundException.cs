namespace ExchangeGood.Repository.Exceptions
{
    public class MemberNotFoundException : NotFoundException
    {
        public MemberNotFoundException(string feId) : base($"This member with fe id: {feId} was not found")
        {
        }
    }
}