using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Service.Interfaces;

public interface IMemberService
{
    Task<PagedList<MemberDto>> GetAllMembers(GetMembersQuery getMembersQuery);
    Task<LoginResponse> CreateMember(CreateMemberRequest createMemberRequest);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    Task<bool> UpdatePassword(PasswordRequest passwordRequest);
    Task<Member> GetMemberByFeId(string feId);
    Task<List<Bookmark>> GetBookMarkByFeId(string feId);
    Task<bool> CreateBookmark(CreateBookmarkRequest createBookmarkRequest);
    Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest);
    Task<IEnumerable<Notification>> GetNotificationsOfUser(string feId);
    Task<Notification> GetNotificationsById(int notificationId);
    Task<int> GetNumberUnreadNotificationedOfUser(string userId);
    Task<bool> AddNotification(Notification notification);
    Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string feId);
    Task<bool> SendResetPasswordEmail(string email, string resetLink);
    Task<Member> GetMemberByEmail(string email);
    Task<bool> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    Task<bool> RemoveNotification(int notificationId);
}