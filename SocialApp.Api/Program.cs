using SocialApp.Api.Extensions;

namespace SocialApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        DotNetEnv.Env.Load(".env");
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServices();
        var app = builder.Build();
        app.AddPipelineComponents();
    }
}
