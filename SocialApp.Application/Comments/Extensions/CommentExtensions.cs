using SocialApp.Application.Comments.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Domain;

namespace SocialApp.Application.Comments.Extensions;

internal static class CommentExtensions
{
    public static CommentResponse MapToCommentResponse(this Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Contents = comment.Contents,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            UserInfo = comment.UserProfile.MapToUserInfo()
        };
    }
}
