namespace SocialApp.Application.Services.DirectoryService;

public interface IDirectoryService
{
    string GenerateFilePath(string dirName, string fileName);
}
