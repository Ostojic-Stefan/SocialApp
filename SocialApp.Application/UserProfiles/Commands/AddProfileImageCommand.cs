using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Commands;

public class AddProfileImageCommand : IRequest<Result<string>>
{
    public required Guid UserProfileId { get; set; }
    public required string ImageUrl { get; set; }
}
