using EfCoreHelpers;
using Microsoft.Extensions.Logging;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Services;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class UploadImageCommandHandler
    : DataContextRequestHandler<UploadImageCommand, Result<bool>>
{
    public UploadImageCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(UploadImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await postRepo.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(
                    AppErrorCode.NotFound,
                    $"Post with id of {request.PostId} does not exist");
                return result;
            }
            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(
                       AppErrorCode.UnAuthorized,
                       "Only the creator of the post can update it");
                return result;
            }
            var directoryService = new DirectoryService(request.DirPath);
            var imageService = new ImageService(directoryService,
                new LoggerFactory().CreateLogger<ImageService>());

            var savePath = await imageService.SaveImageAsync(request.ImageName, request.ImageStream);

            post.UpdateImageUrl(savePath.Split("\\wwwroot\\")[1].Replace("\\", "/"));

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
