namespace ExchangeGood.Repository.Exceptions;

public class ExistMemberException : BadRequestException
{
    public ExistMemberException(string feId) : base($"The member with Fe Id: {feId} is already exist")
    {
    }
}