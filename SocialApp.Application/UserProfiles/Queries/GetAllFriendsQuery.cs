using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.UserProfiles.Queries;

public class GetAllFriendsQuery : IRequest<Result<IReadOnlyList<FriendResponse>>>
{
    public required Guid UserId { get; set; }
}
