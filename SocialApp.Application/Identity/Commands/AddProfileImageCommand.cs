using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Commands;

public class AddProfileImageCommand : IRequest<Result<bool>>
{
    public required Guid UserProfileId { get; set; }
    public required string ImageUrl { get; set; }
}
