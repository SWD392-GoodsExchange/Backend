using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Service.Interfaces;

public interface IMemberService
{
    Task<PagedList<MemberDto>> GetAllMembers(GetMembersQuery getMembersQuery);
    Task<string> CreateMember(CreateMemberRequest createMemberRequest);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    Task<bool> UpdatePassword(PasswordRequest passwordRequest);
    Task<Member> GetMemberByFeId(string feId);
    Task<List<ProductDto>> GetBookMarkByFeId(string feId);
    Task<bool> CreateBookmark(CreateBookmarkRequest createBookmarkRequest);
    Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest);
    Task<IEnumerable<Notification>> GetNotificationsOfUser(string feId);
    Task<bool> AddNotification(Notification notification);
    Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string feId);
}