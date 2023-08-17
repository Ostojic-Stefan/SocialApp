using MediatR;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Files.Commands;

public class UploadPostImageCommand: IRequest<Result<UploadPostImageResponse>>
{
    public required Stream ImageStream { get; set; }
    public required string ImageName { get; set; }
}
