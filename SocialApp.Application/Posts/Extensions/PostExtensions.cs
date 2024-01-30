using MediatR;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Extensions;

public static class PostExtensions
{
    public static PostResponse MapToPostReponse(this Post post, Guid currentUserId)
    {
        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Contents = post.Contents,
            UserInfo = post.UserProfile.MapToUserInfo(),
            Images = post.Images.Select(img => new ImageResponse
            {
                ImageId = img.Id,
                OriginalImagePath = img.OriginalImagePath,
                FullscreenImagePath = img.FullscreenImagePath,
                ThumbnailImagePath = img.ThumbnailImagePath
            }),
            NumComments = post.Comments.Count(),
            NumLikes = post.Likes.Count(),
            LikeInfo = post.Likes.Any(l => l.UserProfileId == currentUserId)
                    ? new PostLikeInfo
                    {
                        LikedByCurrentUser = true,
                        LikeId = post.Likes.First(l => l.UserProfileId == currentUserId).Id,
                    }
                    : new PostLikeInfo
                    {
                        LikedByCurrentUser = false,
                        LikeId = Guid.Empty,
                    },
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
        };
    }
}
