using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Queries;

public class GetPostDetailsQuery : IRequest<Result<PostDetailsResponse>>
{
    public required Guid PostId;
}