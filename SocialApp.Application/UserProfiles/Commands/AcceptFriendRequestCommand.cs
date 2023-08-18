using MediatR;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.Commands;

public class UpdateFriendRequestCommand : IRequest<Result<bool>>
{
    public required Guid CurrentUser { get; set; }
    public required Guid OtherUser { get; set; }
    public required FriendRequestUpdateStatus Status { get; set; }
}
