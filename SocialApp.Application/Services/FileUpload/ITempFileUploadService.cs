namespace SocialApp.Application.Services.FileUpload;

public interface ITempFileUploadService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
    void RemoveFile(string path);
}
