using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Queries;

public class GetPostLikesQuery : IRequest<Result<GetLikesForAPostResponse>>
{
    public required Guid CurrentUser { get; set; }
    public required Guid PostId { get; set; }    
}