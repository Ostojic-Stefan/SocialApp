using EfCoreHelpers;
using SocialApp.Application.Files.Commands;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services.FileUpload;

namespace SocialApp.Application.Files.CommandHandlers;

internal class UploadUserProfileImageCommandHandler
    : DataContextRequestHandler<UploadUserProfileImageCommand, Result<UploadUserProfileImageResponse>>
{
    private readonly IFileUploadService _fileUploadService;

    public UploadUserProfileImageCommandHandler(IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService) 
        : base(unitOfWork)
    {
        _fileUploadService = fileUploadService;
    }

    public override async Task<Result<UploadUserProfileImageResponse>> Handle(UploadUserProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UploadUserProfileImageResponse>();
        try
        {
            var path = await _fileUploadService.UploadFileAsync(request.ImageStream, "Users", request.ImageName, cancellationToken);
            result.Data = new UploadUserProfileImageResponse{ AvatarUrl = path };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}