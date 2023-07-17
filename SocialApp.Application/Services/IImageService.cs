namespace SocialApp.Application.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(string path, Stream stream);
}
