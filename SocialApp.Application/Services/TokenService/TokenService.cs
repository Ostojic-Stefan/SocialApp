using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Application.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialApp.Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    private JwtSecurityTokenHandler TokenHandler { get; set; } = new();

    public string GetToken(Claim[] claims)
    {
        var tokenDescriptor = GenerateTokenDescriptor(claims);
        var token = TokenHandler.CreateToken(tokenDescriptor);
        return TokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GenerateTokenDescriptor(Claim[] claims)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SigningKey)),
                SecurityAlgorithms.HmacSha512
            ),
            Audience = _jwtSettings.Audiences[0],
            Issuer = _jwtSettings.Issuer,
            Expires = DateTime.Now.AddHours(2)
        };
    }
}
