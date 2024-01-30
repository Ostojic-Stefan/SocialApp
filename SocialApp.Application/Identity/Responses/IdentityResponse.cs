using System.Security.Claims;

namespace SocialApp.Application.Identity.Responses;

public class IdentityResponse
{
    //public required string AccessToken { get; set; }
    public required ClaimsIdentity ClaimsIdentity { get; set; }
}

