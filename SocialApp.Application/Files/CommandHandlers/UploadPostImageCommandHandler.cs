using EfCoreHelpers;
using SocialApp.Application.Files.Commands;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services.FileUpload;

namespace SocialApp.Application.Files.CommandHandlers;

internal class UploadPostImageCommandHandler
    : DataContextRequestHandler<UploadPostImageCommand, Result<UploadPostImageResponse>>
{
    private readonly IFileUploadService _fileUploadService;

    public UploadPostImageCommandHandler(IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService) 
        : base(unitOfWork)
    {
        _fileUploadService = fileUploadService;
    }

    public override async Task<Result<UploadPostImageResponse>> Handle(UploadPostImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UploadPostImageResponse>();
        try
        {
            var path = await _fileUploadService.UploadFileAsync(request.ImageStream, "Posts", request.ImageName);
            result.Data = new UploadPostImageResponse{ ImagePath = path };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}