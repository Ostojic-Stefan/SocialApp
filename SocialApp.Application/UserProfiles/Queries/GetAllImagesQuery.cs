using MediatR;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.UserProfiles.Queries;

public class GetAllImagesQuery : IRequest<Result<IReadOnlyList<ImageResponse>>>
{
    public required Guid CurrentUserId { get; set; }
    public required Guid UserProfileId { get; set; }
}
