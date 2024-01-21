using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Queries;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.QueryHandlers;

internal class GetAllImagesQueryHandler 
    : DataContextRequestHandler<GetAllImagesQuery, Result<IReadOnlyList<ImageResponse>>>
{
    public GetAllImagesQueryHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<IReadOnlyList<ImageResponse>>> Handle(GetAllImagesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<ImageResponse>>();
        try
        {
            var imageRepo = _unitOfWork.CreateReadOnlyRepository<Image>();
            var images = await imageRepo
                .Query()
                .Where(i => i.User != null && i.User.Id == request.UserProfileId)
                .Select(img => new ImageResponse
                {
                    ImageId = img.Id,
                    FullscreenImagePath = img.FullscreenImagePath,
                    OriginalImagePath = img.OriginalImagePath,
                    ThumbnailImagePath = img.ThumbnailImagePath
                })
                .ToListAsync(cancellationToken);
            result.Data = images;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
