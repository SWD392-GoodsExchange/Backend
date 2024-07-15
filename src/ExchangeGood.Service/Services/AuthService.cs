using System.Security.Claims;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Contract.Payloads.Response.Payloads.Request.RefreshToken;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Extensions;
using ExchangeGood.Service.Interfaces;

namespace ExchangeGood.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMemberService _memberService;
        private readonly IClaimsPrincipalExtensions _claimsPrincipalExtensions;

        public AuthService(IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IClaimsPrincipalExtensions claimsPrincipalExtensions, IMemberService memberService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _claimsPrincipalExtensions = claimsPrincipalExtensions;
            _memberService = memberService;
        }

        public async Task<BaseResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var claimsPrincipal = _claimsPrincipalExtensions.GetTokenPrincipal(refreshTokenRequest.JwtToken);
            refreshTokenRequest.FeId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(refreshTokenRequest.FeId))
            {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
            }
            // get User Name
            string userName = (await _memberService.GetMemberByFeId(refreshTokenRequest.FeId)).UserName;
            if (string.IsNullOrEmpty(userName))
            {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
            }
            // get refresh token
            var refreshToken = await _refreshTokenRepository.GetRefreshToken(refreshTokenRequest);

            // Gen token & refresh token
            var jwtToken = _jwtProvider.GenerateToken(refreshToken.Fe);
            var refreshTokenString = _jwtProvider.GenerateRefreshToken();

            // Update token
            refreshToken.Token = refreshTokenString;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddHours(12);
            var result = await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
            if (!result)
            {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
            }

            var loginResponse = new LoginResponse
            {
                FeId = refreshTokenRequest.FeId,
                JwtToken = jwtToken,
                UserName = userName,
                Avatar = AvatarImage.GetImage(refreshToken.FeId),
                RefreshToken = refreshTokenString
            };
            return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, loginResponse);
        }
    }
}