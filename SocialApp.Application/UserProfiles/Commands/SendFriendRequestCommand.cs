using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Commands;

public class SendFriendRequestCommand : IRequest<Result<bool>>
{
    public required Guid CurrentUser { get; set; }
    public required Guid RecieverUser { get; set; }
}
