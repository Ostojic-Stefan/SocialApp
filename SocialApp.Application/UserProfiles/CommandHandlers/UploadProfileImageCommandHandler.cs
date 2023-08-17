using EfCoreHelpers;
using Microsoft.Extensions.Logging;
using SocialApp.Application.Models;
using SocialApp.Application.Services;
using SocialApp.Application.UserProfiles.Commands;

namespace SocialApp.Application.UserProfiles.CommandHandlers;
internal class UploadProfileImageCommandHandler
    : DataContextRequestHandler<UploadProfileImageCommand, Result<string>>
{
    public UploadProfileImageCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<Result<string>> Handle(UploadProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<string>();
        try
        {
            var directoryService = new DirectoryService(request.DirPath);
            var imageService = new ImageService(directoryService,
                new LoggerFactory().CreateLogger<ImageService>());
            var savePath = await imageService.SaveImageAsync(request.ImageName, request.ImageStream);
            result.Data = $"http://localhost:5167{savePath.Split("wwwroot")[1].Replace("\\", "/")}";
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
