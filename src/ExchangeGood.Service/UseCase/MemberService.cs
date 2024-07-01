using Azure.Core;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using Microsoft.VisualBasic;

namespace ExchangeGood.Service.UseCase;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IJwtProvider _jwtProvider;

    public MemberService(IMemberRepository memberRepository,
        IProductRepository productRepository,
        IBookmarkRepository bookmarkRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        INotificationRepository notificationRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _bookmarkRepository = bookmarkRepository;
        _productRepository = productRepository;
        _memberRepository = memberRepository;
        _notificationRepository = notificationRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<PagedList<MemberDto>> GetAllMembers(GetMembersQuery getMembersQuery)
    {
        return await _memberRepository.GetMembers(getMembersQuery);
    }

    public async Task<string> CreateMember(CreateMemberRequest createMemberRequest)
    {
        var feId = await _memberRepository.CreateMember(createMemberRequest);

        return feId;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var member = await _memberRepository.CheckLogin(loginRequest);
        bool result = false;
        // get refresh token
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenByFeId(loginRequest.FeId);
        // Gen token & refresh token
        var jwtToken = _jwtProvider.GenerateToken(member);
        var refreshTokenString = _jwtProvider.GenerateRefreshToken();

        // check refresh token if null => create 
        if (refreshToken == null)
        {
            var newRefreshToken = new RefreshToken
            {
                FeId = member.FeId,
                Token = refreshTokenString,
                ExpiryDate = DateTime.UtcNow.AddHours(12)
            };
            result = await _refreshTokenRepository.AddRefreshToken(newRefreshToken);
            if (!result)
            {
                return default;
            }
        }
        // Update token
        else
        {
            refreshToken.Token = refreshTokenString;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddHours(12);
            result = await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
            if (!result)
            {
                return default;
            }
        }
        var loginResponse = new LoginResponse
        {
            JwtToken = jwtToken,
            RefreshToken = refreshTokenString
        };
        return loginResponse;
    }

    public async Task<bool> UpdatePassword(PasswordRequest passwordRequest)
    {
        var isUpdate = await _memberRepository.UpdatePassword(passwordRequest);
        return isUpdate;
    }

    public async Task<Member> GetMemberByFeId(string feId)
    {
        return await _memberRepository.GetMemberById(feId);
        // return member == null
        //     ? BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG)
        //     : BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, member);
    }

    public async Task<List<ProductDto>> GetBookMarkByFeId(string feId)
    {
        var product = await _bookmarkRepository.GetAllBookmarks(feId);
        return product;
    }

    public async Task<bool> CreateBookmark(CreateBookmarkRequest createBookmarkRequest)
    {
        // check if product is sold => can not bookmark
        if (!Int32.TryParse(createBookmarkRequest.ProductId, out int productId))
            return false;
        var product = await _productRepository.GetProduct(productId);
        var checkProductStatus = product.Status == Contract.Enum.Product.Status.Sold.Name;
        if (checkProductStatus)
            return false;

        var isAdd = await _bookmarkRepository.AddBookmark(createBookmarkRequest);
        return isAdd;
    }

    public async Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest)
    {
        if (!Int32.TryParse(deleteBookmarkRequest.ProductId, out int productId))
            return false;
        var isDelete = await _bookmarkRepository.DeleteBookmark(deleteBookmarkRequest);
        return isDelete;
    }


    // Notification
    public async Task<IEnumerable<Notification>> GetNotificationsOfUser(string feId)
    {
        var result = await _notificationRepository.GetNotifcationsForUser(feId);
        return result;
    }

    public async Task<bool> AddNotification(Notification notification)
    {
        return await _notificationRepository.AddNotifcation(notification);
    }

    public async Task<IEnumerable<Notification>> GetNotificationsWereSendedByUser(string feId)
    {
        return await _notificationRepository.GetNotifcationsSendedByUser(feId);
    }
}