namespace ExchangeGood.Repository.Exceptions;

public class ExistMemberException : EntityException
{
    public ExistMemberException(string feId) : base($"The member with Fe Id: {feId} is already exist")
    {
    }
}