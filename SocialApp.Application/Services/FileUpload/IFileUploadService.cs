namespace SocialApp.Application.Services.FileUpload;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(Stream stream, string dirName, string fileName, CancellationToken cancellationToken = default);
}
