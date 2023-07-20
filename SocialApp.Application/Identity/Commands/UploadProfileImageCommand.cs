using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Commands;

public class UploadProfileImageCommand : IRequest<Result<string>>
{
    public required Guid UserProfileId { get; set; }
    public required Stream ImageStream { get; set; }
    public required string ImageName { get; set; }
    public required string DirPath { get; set; }
}
