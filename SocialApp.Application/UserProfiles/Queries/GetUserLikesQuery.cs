using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.QueryHandlers;

namespace SocialApp.Application.UserProfiles.Queries;

public class GetUserLikesQuery : IRequest<Result<LikesForUserResponse>>
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
}