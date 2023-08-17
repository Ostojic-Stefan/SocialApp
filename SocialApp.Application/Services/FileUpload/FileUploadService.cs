using SocialApp.Application.Services.DirectoryService;

namespace SocialApp.Application.Services.FileUpload;

public class FileUploadService : IFileUploadService
{
    private readonly IDirectoryService _directoryService;

    public FileUploadService(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }

    public async Task<string> UploadFileAsync(Stream stream, string dirName, string fileName,
        CancellationToken cancellationToken = default)
    {
        if (stream.Length == 0)
            throw new ArgumentException("No File Provided");
        var dirPath = _directoryService.GenerateFilePath(dirName, fileName);
        using (var fStream = new FileStream(dirPath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fStream, cancellationToken);
        }
        return dirPath;
    }
}
