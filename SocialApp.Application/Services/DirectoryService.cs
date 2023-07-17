namespace SocialApp.Application.Services;

public class DirectoryService : IDirectoryService
{
    private readonly string _rootPath;

    public string RootPath => _rootPath;

    public DirectoryService(string rootPath)
    {
        _rootPath = rootPath;

        if (!Directory.Exists(_rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    public string TryCreateDirectoryFor(string fileName)
    {
        var newFileName = Guid.NewGuid().ToString() + fileName;
        var filePath = Path.Combine(_rootPath, newFileName);
        return filePath;
    }
}
