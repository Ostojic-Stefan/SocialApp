namespace SocialApp.Application.Services;

public class ServerUrlService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _config;

    public ServerUrlService(IWebHostEnvironment environment, IConfiguration config)
    {
        _config = config;
        _environment = environment;
    }

    public string GetServerUrl()
    {
        var env = _config.GetSection("LaunchUrls");
        return _environment.IsProduction() 
            ? env["Production"]! 
            : env["Development"]!;
    }
}