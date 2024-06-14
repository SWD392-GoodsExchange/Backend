using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Repository.Interfaces;

public interface IMemberRepository
{
    Task<PagedList<MemberDto>> GetMembers(GetMembersQuery getMembersQuery);
    Task<Member> GetMemberById(string feId);
    Task<Member> CheckLogin(LoginRequest loginRequest);
    Task<string> CreateMember(CreateMemberRequest createMemberRequest);
}