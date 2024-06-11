using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;

namespace ExchangeGood.Service.UseCase;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<BaseResponse> GetAllMembers(GetMembersQuery getMembersQuery)
    {
        var result = await _memberRepository.GetMembers(getMembersQuery);
        return result.TotalCount > 0
            ? BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
    }
    
}