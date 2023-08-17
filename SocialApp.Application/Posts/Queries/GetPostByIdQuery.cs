using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Queries;

public class GetPostByIdQuery : IRequest<Result<PostResponse>>
{
    public required Guid CurrentUserId { get; set; }
    public Guid PostId { get; set; }
}
