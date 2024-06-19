using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;

namespace ExchangeGood.Service.Interfaces;

public interface IMemberService
{
    Task<BaseResponse> GetAllMembers(GetMembersQuery getMembersQuery);
    Task<BaseResponse> CreateMember(CreateMemberRequest createMemberRequest);
    Task<BaseResponse> Login(LoginRequest loginRequest);
    Task<BaseResponse> UpdatePassword(PasswordRequest passwordRequest);
    Task<BaseResponse> GetMemberByFeId(string feId);
    Task<BaseResponse> GetBookMarkByFeId(string feId);
    Task<BaseResponse> CreateBookmark(CreateBookmarkRequest createBookmarkRequest);
    Task<BaseResponse> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest);
}