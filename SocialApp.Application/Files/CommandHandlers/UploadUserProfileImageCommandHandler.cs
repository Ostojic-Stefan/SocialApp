using EfCoreHelpers;
using SocialApp.Application.Files.Commands;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services.FileUpload;

namespace SocialApp.Application.Files.CommandHandlers;

internal class UploadUserProfileImageCommandHandler
    : DataContextRequestHandler<UploadUserProfileImageCommand, Result<UploadImageResponse>>
{
    private readonly ITempFileUploadService _fileUploadService;

    public UploadUserProfileImageCommandHandler(IUnitOfWork unitOfWork,
        ITempFileUploadService fileUploadService) 
        : base(unitOfWork)
    {
        _fileUploadService = fileUploadService;
    }

    public override async Task<Result<UploadImageResponse>> Handle(UploadUserProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UploadImageResponse>();
        try
        {
            var path = await _fileUploadService.UploadFileAsync(request.ImageStream, request.ImageName, cancellationToken);
            result.Data = new UploadImageResponse { ImageName = path };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}