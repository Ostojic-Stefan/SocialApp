using System.Security.Claims;

namespace SocialApp.Api.Extensions;

public static class HttpContextExtension
{
    public static Guid GetUserProfileId(this HttpContext context)
    {
        var identity = context.User.FindFirstValue("UserProfileId");
        return Guid.Parse(identity);
    }
}
