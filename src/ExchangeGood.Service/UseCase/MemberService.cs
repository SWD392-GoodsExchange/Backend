using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;

namespace ExchangeGood.Service.UseCase;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IJwtProvider _jwtProvider;

    public MemberService(IMemberRepository memberRepository, IJwtProvider jwtProvider)
    {
        _memberRepository = memberRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<BaseResponse> GetAllMembers(GetMembersQuery getMembersQuery)
    {
        var result = await _memberRepository.GetMembers(getMembersQuery);
        return result.TotalCount > 0
            ? BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
    }

    public async Task<BaseResponse> CreateMember(CreateMemberRequest createMemberRequest) {
        var memberFeId = await _memberRepository.CreateMember(createMemberRequest);
        return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, memberFeId);
    }

    public async Task<BaseResponse> Login(LoginRequest loginRequest)
    {
        var member = await _memberRepository.CheckLogin(loginRequest);
        var token = _jwtProvider.Generate(member);
        return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, token);
    }
}