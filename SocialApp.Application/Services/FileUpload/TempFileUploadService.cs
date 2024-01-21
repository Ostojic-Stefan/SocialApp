using Microsoft.Extensions.Options;
using SocialApp.Application.Settings;

namespace SocialApp.Application.Services.FileUpload;

public class TempFileUploadService : ITempFileUploadService
{
    private readonly ImageFileStorageSettings _imageStorageSettings;

    public TempFileUploadService(IOptions<ImageFileStorageSettings> settings)
    {
        _imageStorageSettings = settings.Value;
    }
    public async Task<string> UploadFileAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        if (stream.Length == 0)
            throw new ArgumentException("No File Provided");

        var imgName = $"{DateTime.Now.Ticks}_{fileName}";

        if (!Directory.Exists(_imageStorageSettings.TempImageFileDir))
            Directory.CreateDirectory(_imageStorageSettings.TempImageFileDir);

        var path = Path.Combine(_imageStorageSettings.TempImageFileDir, imgName).Replace("\\", "/");

        using (var fStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fStream, cancellationToken);
        }
        return imgName;
    }

    public void RemoveFile(string path)
    {
        var imgToDelete = Path.Combine(_imageStorageSettings.TempImageFileDir, path).Replace("\\", "/");
        File.Delete(imgToDelete);
    }

}
