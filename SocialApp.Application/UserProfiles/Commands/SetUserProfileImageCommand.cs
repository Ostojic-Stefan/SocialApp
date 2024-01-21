using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Commands;

public class SetUserProfileImageCommand : IRequest<Result<bool>>
{
    public required Guid CurrentUserId { get; set; }
    public required Guid ImageId { get; set; }
}
