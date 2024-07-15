﻿using AutoMapper.Execution;
using Azure.Core;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Bookmark;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MimeKit;
using MailKit.Net.Smtp;

namespace ExchangeGood.Service.UseCase;

public class MemberService : IMemberService {
    private readonly IMemberRepository _memberRepository;
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly SmtpSettings _smtpSetting;

    public MemberService(IMemberRepository memberRepository,
        IProductRepository productRepository,
        IBookmarkRepository bookmarkRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        INotificationRepository notificationRepository,
        IOptions<SmtpSettings> smtpSetting) {
        _refreshTokenRepository = refreshTokenRepository;
        _bookmarkRepository = bookmarkRepository;
        _productRepository = productRepository;
        _memberRepository = memberRepository;
        _notificationRepository = notificationRepository;
        _jwtProvider = jwtProvider;
        _smtpSetting = smtpSetting.Value;
    }

    public async Task<PagedList<MemberDto>> GetAllMembers(GetMembersQuery getMembersQuery) {
        return await _memberRepository.GetMembers(getMembersQuery);
    }

    public async Task<LoginResponse> CreateMember(CreateMemberRequest createMemberRequest) {
        var member = await _memberRepository.CreateMember(createMemberRequest);
        bool result = false;
        // get refresh token
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenByFeId(member.FeId);
        // Gen token & refresh token
        var jwtToken = _jwtProvider.GenerateToken(member);
        var refreshTokenString = _jwtProvider.GenerateRefreshToken();

        // check refresh token if null => create 
        if (refreshToken == null) {
            var newRefreshToken = new RefreshToken {
                FeId = member.FeId,
                Token = refreshTokenString,
                ExpiryDate = DateTime.UtcNow.AddHours(12)
            };
            result = await _refreshTokenRepository.AddRefreshToken(newRefreshToken);
            if (!result) {
                return default;
            }
        }
        // Update token
        else {
            refreshToken.Token = refreshTokenString;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddHours(12);
            result = await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
            if (!result) {
                return default;
            }
        }
        var loginResponse = new LoginResponse {
            FeId = member.FeId,
            UserName = member.UserName,
            Avatar = AvatarImage.GetImage(member.FeId),
            JwtToken = jwtToken,
            RefreshToken = refreshTokenString
        };
        return loginResponse;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest) {
        var member = await _memberRepository.CheckLogin(loginRequest);
        bool result = false;
        // get refresh token
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenByFeId(loginRequest.FeId);
        // Gen token & refresh token
        var jwtToken = _jwtProvider.GenerateToken(member);
        var refreshTokenString = _jwtProvider.GenerateRefreshToken();

        // check refresh token if null => create 
        if (refreshToken == null) {
            var newRefreshToken = new RefreshToken {
                FeId = member.FeId,
                Token = refreshTokenString,
                ExpiryDate = DateTime.UtcNow.AddHours(12)
            };
            result = await _refreshTokenRepository.AddRefreshToken(newRefreshToken);
            if (!result) {
                return default;
            }
        }
        // Update token
        else {
            refreshToken.Token = refreshTokenString;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddHours(12);
            result = await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
            if (!result) {
                return default;
            }
        }
        var loginResponse = new LoginResponse {
            FeId = member.FeId,
            UserName = member.UserName,
            Avatar = AvatarImage.GetImage(member.FeId),
            JwtToken = jwtToken,
            RefreshToken = refreshTokenString
        };
        return loginResponse;
    }

    public async Task<bool> UpdatePassword(PasswordRequest passwordRequest) {
        var isUpdate = await _memberRepository.UpdatePassword(passwordRequest);
        return isUpdate;
    }

    public async Task<Data.Models.Member> GetMemberByFeId(string feId) {
        return await _memberRepository.GetMemberById(feId);
        // return member == null
        //     ? BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG)
        //     : BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, member);
    }

    public async Task<List<ProductDto>> GetBookMarkByFeId(string feId) {
        var product = await _bookmarkRepository.GetAllBookmarks(feId);
        return product;
    }

    public async Task<bool> CreateBookmark(CreateBookmarkRequest createBookmarkRequest) {
        // check if product is sold => can not bookmark
        var product = await _productRepository.GetProduct(createBookmarkRequest.ProductId);
        var checkProductStatus = product.Status == Contract.Enum.Product.Status.Sold.Name;
        if (checkProductStatus)
            return false;

        var isAdd = await _bookmarkRepository.AddBookmark(createBookmarkRequest);
        return isAdd;
    }

    public async Task<bool> DeleteBookmark(DeleteBookmarkRequest deleteBookmarkRequest) {
        var isDelete = await _bookmarkRepository.DeleteBookmark(deleteBookmarkRequest);
        return isDelete;
    }


    // Notification
    public async Task<IEnumerable<Notification>> GetNotificationsOfUser(string feId) {
        var result = await _notificationRepository.GetNotifcationsForUser(feId);
        return result;
    }

    public async Task<Notification> GetNotificationsById(int notificationId) {
        var result = await _notificationRepository.GetNotifcation(notificationId);
        return result;
    }

    public async Task<bool> AddNotification(Notification notification) {
        return await _notificationRepository.AddNotifcation(notification);
    }

    public async Task<IEnumerable<Notification>> GetAllRequestExchangesFromUserAndOtherUserRequestForUser(string feId) {
        return await _notificationRepository.GetAllRequestExchangesFromUserAndOtherUserRequestForUser(feId);
    }
       
    public async Task<bool> SendResetPasswordEmail(string email, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ExchangeGood System", _smtpSetting.Username));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Reset Your Password";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"
                <p>Hello,</p>
                <p>You have requested to reset your password. Please click the link below to reset your password:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you did not request this, please ignore this email.</p>
                <p>Thank you,</p>
            ";

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient()) {
            try {
                client.Connect(_smtpSetting.SmtpServer, _smtpSetting.Port, _smtpSetting.UseSsl);
                client.Authenticate(_smtpSetting.Username, _smtpSetting.Password);
                await client.SendAsync(message);
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
            finally {
                await client.DisconnectAsync(true);
            }
        }
    }

    public async Task<Data.Models.Member> GetMemberByEmail(string email) {
        return await _memberRepository.GetMemberByEmail(email);
    }
}