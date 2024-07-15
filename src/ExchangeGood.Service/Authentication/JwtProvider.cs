using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExchangeGood.Data.Models;
using ExchangeGood.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtConstants = Microsoft.IdentityModel.JsonWebTokens.JwtConstants;

namespace ExchangeGood.Service.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(Member member)
    {
        // create claims
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, member.FeId),
            new(ClaimTypes.Role, member.Role.RoleName)
        };
        // create sign credentials
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
            , SecurityAlgorithms.HmacSha512Signature);

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.ExpiryTimeFrame),
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(token);
        var tokenResult = tokenHandler.WriteToken(securityToken);

        return tokenResult;
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}