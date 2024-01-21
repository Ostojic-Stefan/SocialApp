namespace SocialApp.Application.Services;

public class FileHttpConverter
{
    private readonly string _rootPath;
    private readonly ServerUrlService _serverUrlService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileHttpConverter(string rootPath, ServerUrlService serverUrlService, IWebHostEnvironment webHostEnvironment)
    {
        _serverUrlService = serverUrlService;
        _webHostEnvironment = webHostEnvironment;
        _rootPath = rootPath;
    }

    public string ConvertFromHttpEndpoint(string httpEndpoint)
    {
        var serverUrl = _serverUrlService.GetServerUrl();
        return httpEndpoint.Replace(serverUrl, _webHostEnvironment.WebRootPath);
    }

    public string ConvertToHttpEndpoint(string filePath)
    {
        var normalizedPath = filePath.Replace("\\", "/");
        var relativePath = filePath.Replace(_rootPath, "").TrimStart('/');
        var httpEndPoint = $"{_serverUrlService.GetServerUrl()}/{relativePath}";
        return httpEndPoint;
    }
}