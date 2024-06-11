using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;

namespace ExchangeGood.Service.Interfaces;

public interface IMemberService
{
    Task<BaseResponse> GetAllMembers(GetMembersQuery getMembersQuery);
}