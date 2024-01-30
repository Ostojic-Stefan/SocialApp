using SocialApp.Application.Files.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.Extensions;

public static class UserProfileExtensions
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
            FriendStatus = GetFriendStatus(userProfile, currentUserId),
            NumFriends = userProfile.Friends.AsQueryable().Count(),
            NumLikes = userProfile.Posts.AsQueryable().SelectMany(p => p.Likes).Count(),
            NumPosts = userProfile.Posts.AsQueryable().Count()
        });
    }

    private static FriendStatus GetFriendStatus(UserProfile userProfile, Guid CurrUserId)
    {
        if (userProfile.Friends.Any(f => f.Id == CurrUserId))
            return FriendStatus.Friend;
        if (userProfile.ReceivedFriendRequests.Any(x => x.Status == FriendRequestStatus.Pending && x.SenderUserId == CurrUserId))
            return FriendStatus.WaitingApproval;
        if (userProfile.SentFriendRequests.Any(x => x.Status == FriendRequestStatus.Pending && x.ReceiverUserId == CurrUserId))
            return FriendStatus.WaitingAcceptance;
        return FriendStatus.NotFriend;
    }
}
