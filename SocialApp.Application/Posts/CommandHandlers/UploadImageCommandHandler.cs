using EfCoreHelpers;
using Microsoft.Extensions.Logging;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Services;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class UploadImageCommandHandler
    : DataContextRequestHandler<UploadImageCommand, Result<string>>
{
    private readonly ILogger<ImageService> _imgLogger;

    public UploadImageCommandHandler(IUnitOfWork unitOfWork, ILogger<ImageService> imgLogger) 
        : base(unitOfWork)
    {
        _imgLogger = imgLogger;
    }

    public override async Task<Result<string>> Handle(UploadImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<string>();
        try
        {
            var directoryService = new DirectoryService(request.DirPath);
            var imageService = new ImageService(directoryService, _imgLogger);
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
