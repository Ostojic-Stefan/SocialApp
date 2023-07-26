using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Commands;

public class AcceptFriendRequestCommand : IRequest<Result<bool>>
{
    public required Guid CurrentUser { get; set; }
    public required Guid OtherUser { get; set; }
}
