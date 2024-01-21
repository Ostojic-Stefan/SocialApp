using EfCoreHelpers;
using SocialApp.Application.Files.Commands;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services.FileUpload;

namespace SocialApp.Application.Files.CommandHandlers;

internal class UploadPostImageCommandHandler
    : DataContextRequestHandler<UploadPostImageCommand, Result<UploadPostImageResponse>>
{
    private readonly ITempFileUploadService _fileUploadService;

    public UploadPostImageCommandHandler(IUnitOfWork unitOfWork,
        ITempFileUploadService fileUploadService) 
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
            var imageName = await _fileUploadService.UploadFileAsync(request.ImageStream, request.ImageName, cancellationToken);
            result.Data = new UploadPostImageResponse{ ImageName = imageName };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}