using Azure.Core;
using ExchangeGood.Contract.Common;
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
    private readonly INotificationRepository _notificationRepository;

    private readonly IJwtProvider _jwtProvider;

    public MemberService(IMemberRepository memberRepository, IProductRepository productRepository,
        IBookmarkRepository bookmarkRepository, INotificationRepository notificationRepository, IJwtProvider jwtProvider)
    {
        _bookmarkRepository = bookmarkRepository;
        _productRepository = productRepository;
        _memberRepository = memberRepository;
        _notificationRepository = notificationRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<BaseResponse> GetAllMembers(GetMembersQuery getMembersQuery)
    {
        var result = await _memberRepository.GetMembers(getMembersQuery);
        return result.TotalCount > 0
            ? BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
    }

    public async Task<BaseResponse> CreateMember(CreateMemberRequest createMemberRequest)
    {
        var result = await _memberRepository.CreateMember(createMemberRequest);

        return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
    }

    public async Task<BaseResponse> Login(LoginRequest loginRequest)
    {
        var member = await _memberRepository.CheckLogin(loginRequest);
        var token = _jwtProvider.Generate(member);
        return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, token);
    }

    public async Task<BaseResponse> UpdatePassword(PasswordRequest passwordRequest)
    {
         var result = await _memberRepository.UpdatePassword(passwordRequest);
         return result
             ? BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG,
                 nameof(UpdatePassword) + " successful")
             : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
    }

    public async Task<Member> GetMemberByFeId(string feId)
    {
        return await _memberRepository.GetMemberById(feId);
        // return member == null
        //     ? BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG)
        //     : BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, member);
    }

    public async Task<BaseResponse> GetBookMarkByFeId(string feId)
    {
        var product = await _bookmarkRepository.GetAllBookmarks(feId);
        return product.Count > 0
            ? BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
    }

    public async Task<BaseResponse> CreateBookmark(CreateBookmarkRequest createBookmarkRequest)
    {
        // check if product is sold => can not bookmark
        if (!Int32.TryParse(createBookmarkRequest.ProductId, out int productId))
            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
        var product = await _productRepository.GetProduct(productId);
        var checkProductStatus = product.Status == Contract.Enum.Product.Status.Sold.Name;
        if (checkProductStatus) return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);

        var result = await _bookmarkRepository.AddBookmark(createBookmarkRequest);
        return result
            ? BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
    }

    public async Task<BaseResponse> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest)
    {
        if (!Int32.TryParse(deleteBookmarkRequest.ProductId, out int productId))
            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
        var result = await _bookmarkRepository.DeleteBookmark(deleteBookmarkRequest);
        return result
            ? BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG)
            : BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG);
    }


    // Notification
    public async Task<IEnumerable<Notification>> GetNotificationsOfUser(string feId) {
        var result = await _notificationRepository.GetNotifcationsForUser(feId);    
        return result;
    }

    public async Task<bool> AddNotification(Notification notification) {
        return await _notificationRepository.AddNotifcation(notification);
    }
}