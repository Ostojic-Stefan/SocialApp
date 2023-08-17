namespace SocialApp.Application.Services.DirectoryService;

public class DirectoryService : IDirectoryService
{
    private readonly string _rootPath;

    public DirectoryService(string rootPath)
    {
        _rootPath = rootPath;
    }

    public string GenerateFilePath(string dirName, string fileName)
    {
        var directoryPath = Path.Combine(_rootPath, dirName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
        var filePath = Path.Combine(directoryPath, uniqueFileName); 
        return filePath;
    }
}
