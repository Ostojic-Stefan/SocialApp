using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Commands;

public class AddUserImageCommand : IRequest<Result<bool>>
{
    public required Guid UserProfileId { get; set; }
    public required string ImageUrl { get; set; }
}
