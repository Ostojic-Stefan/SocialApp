using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Queries;

public class GetUserFriendsPostsQuery : IRequest<Result<IReadOnlyList<PostResponse>>>
{
    public required Guid CurrentUserId { get; set; }
}
