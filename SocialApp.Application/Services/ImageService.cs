using Microsoft.Extensions.Logging;

namespace SocialApp.Application.Services;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly IDirectoryService _directoryService;

    public ImageService(IDirectoryService directoryService, ILogger<ImageService> logger)
	{
        _logger = logger;
		_directoryService = directoryService;
    }

    public async Task<string> SaveImageAsync(string path, Stream stream)
    {
		try
		{
			string savePath = _directoryService.TryCreateDirectoryFor(path);
			await using FileStream fStream = new(savePath, FileMode.Create, FileAccess.Write);
			await stream.CopyToAsync(fStream);
			return savePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message, ex);
			throw;
		}
    }
}
