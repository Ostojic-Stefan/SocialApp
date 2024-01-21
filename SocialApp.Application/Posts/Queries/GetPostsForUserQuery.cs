using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Queries;

public class GetPostsForUserQuery : IRequest<Result<PostsForUserResponse>>
{
    public required Guid UserProfileId { get; set; }
}
