using SocialApp.Application.Files.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;
using System.Linq.Expressions;

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
                ImageId = userProfile.ProfileImage.Id,
                OriginalImagePath = userProfile.ProfileImage.OriginalImagePath,
                FullscreenImagePath = userProfile.ProfileImage.FullscreenImagePath,
                ThumbnailImagePath = userProfile.ProfileImage.ThumbnailImagePath
            }
        };
    }

    public static IQueryable<UserDetailsResponse> ToDetailsResponse(this IQueryable<UserProfile> userProfileQuery, Guid currentUserId)
    {
        return userProfileQuery.Select(userProfile => new UserDetailsResponse
        {
            UserInfo = userProfile.MapToUserInfo(),
            CreatedAt = userProfile.CreatedAt,
            UpdatedAt = userProfile.UpdatedAt,
            IsFriend = userProfile.Friends != null && userProfile.Friends.AsQueryable().Any(f => f.Id == currentUserId),
            NumFriends = userProfile.Friends.AsQueryable().Count(),
            NumLikes = userProfile.Posts.AsQueryable().SelectMany(p => p.Likes).Count(),
            NumPosts = userProfile.Posts.AsQueryable().Count()
        });
    }
}
