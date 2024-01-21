using SocialApp.Application.Files.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.Extensions;

internal static class UserProfileExtensions
{
    public static UserInfo MapToUserInfo(this UserProfile userProfile)
    {
        return new UserInfo
        {
            UserProfileId = userProfile.Id,
            Username = userProfile.Username,
            Biography = userProfile.Biography,
            ProfileImage = new ImageResponse
            {
                OriginalImagePath = userProfile.ProfileImage.OriginalImagePath,
                FullscreenImagePath = userProfile.ProfileImage.FullscreenImagePath,
                ThumbnailImagePath = userProfile.ProfileImage.ThumbnailImagePath
            }
        };
    }

    public static UserDetailsResponse MapToUserDetailsResponse(this UserProfile userProfile, Guid currentUserId)
    {
        return new UserDetailsResponse
        {
            UserInfo = userProfile.MapToUserInfo(),
            CreatedAt = userProfile.CreatedAt,
            UpdatedAt = userProfile.UpdatedAt,
            IsFriend = userProfile.Friends != null && userProfile.Friends.Any(f => f.Id == currentUserId),
        };
    }
}
