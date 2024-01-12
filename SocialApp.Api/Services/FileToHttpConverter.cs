namespace SocialApp.Application.Services;

public class FileToHttpConverter
{
    private readonly string _rootPath;
    private readonly ServerUrlService _serverUrlService;

    public FileToHttpConverter(string rootPath, ServerUrlService serverUrlService)
    {
        _serverUrlService = serverUrlService;
        _rootPath = rootPath;
    }

    public string ConvertToHttpEndpoint(string filePath)
    {
        //var normalizedPath = filePath.Replace("\\", "/");
        var relativePath = filePath.Replace(_rootPath, "").TrimStart('/');
        var httpEndPoint = $"{_serverUrlService.GetServerUrl()}/{relativePath}";
        return httpEndPoint;
    }
}