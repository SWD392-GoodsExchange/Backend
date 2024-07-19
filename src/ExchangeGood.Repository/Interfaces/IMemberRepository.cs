using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Repository.Interfaces;

public interface IMemberRepository
{
    public Task<PagedList<MemberDto>> GetMembers(GetMembersQuery getMembersQuery);
    public Task<Member> GetMemberById(string feId);
    public Task<Member> CheckLogin(LoginRequest loginRequest);
    public Task<bool> UpdatePassword(PasswordRequest passwordRequest);
    Task<Member> CreateMember(CreateMemberRequest createMemberRequest);
    Task<Member> GetMemberByEmail(string email);
    public Task<IEnumerable<Top3MemberDto>> GetTop3PostingProducts();
    public Task<IEnumerable<Top3MemberDto>> GetTop3PostingProductsTradeType();
}