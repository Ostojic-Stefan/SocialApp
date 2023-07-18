using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.Posts.Commands;

public class UploadImageCommand : IRequest<Result<string>>
{
    public required Guid UserProfileId { get; set; }
    public required Stream ImageStream { get; set; }
    public required string ImageName { get; set; }
    public required string DirPath { get; set; }

}
