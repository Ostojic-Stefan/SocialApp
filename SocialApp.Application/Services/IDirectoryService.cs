namespace SocialApp.Application.Services;

public interface IDirectoryService
{
    public string TryCreateDirectoryFor(string fileName);
}