using System.Security.Claims;

namespace SocialApp.Application.Services;

public interface ITokenService
{
    string GetToken(Claim[] claims);
}