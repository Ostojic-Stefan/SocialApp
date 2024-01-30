using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SocialApp.Application.Interfaces;
using SocialApp.Application.Services.FileUpload;
using SocialApp.Application.Settings;
using System.Threading.Channels;

namespace SocialApp.Api.BackgroundServices;

public sealed class ImageProcessingBackgroundService : IHostedService
{
    private readonly ILogger<ImageProcessingBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITempFileUploadService _tempFileUploadService;
    private readonly ChannelReader<ImageProcessingMessage> _channelReader;
    private readonly ImageFileStorageSettings _imageStorageSettings;
    private readonly ImageProcessingSettings _imageProcessingSettings;

    public ImageProcessingBackgroundService(
        ILogger<ImageProcessingBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        Channel<ImageProcessingMessage> channel,
        IOptions<ImageFileStorageSettings> imageFileStorageSettings,
        IOptions<ImageProcessingSettings> imageProcessingSettings,
        ITempFileUploadService tempFileUploadService
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _tempFileUploadService = tempFileUploadService;
        _imageStorageSettings = imageFileStorageSettings.Value;
        _imageProcessingSettings = imageProcessingSettings.Value;
        _channelReader = channel.Reader;
    }

    public async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        while (await _channelReader.WaitToReadAsync(cancellationToken))
        {
            var message = await _channelReader.ReadAsync(cancellationToken);
            try
            {
                using (var scopeService = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scopeService.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var imgRepo = unitOfWork.CreateReadWriteRepository<Domain.Image>();
                    var postRepo = unitOfWork.CreateReadWriteRepository<Domain.Post>();

                    var imgCount = await imgRepo.Query().CountAsync(cancellationToken);
                    var subDir = $"{imgCount % 100}";
                    var storagePath = Path.Combine(_imageStorageSettings.RootFileImageDir, subDir).Replace("\\", "/");
                    var newImgName = Guid.NewGuid() + ".jpg";

                    if (!Directory.Exists(storagePath))
                        Directory.CreateDirectory(storagePath);

                    var tempImage = Path.Combine(_imageStorageSettings.TempImageFileDir, message.FilePath);
                    using var image = await Image.LoadAsync(tempImage, cancellationToken);

                    var origPath = $"orig_{newImgName}";
                    var thumbPath = $"thumb_{newImgName}";
                    var fullPath = $"full_{newImgName}";

                    List<Task> tasks = new()
                    {
                        SaveImage(image, origPath, storagePath, image.Width, cancellationToken),
                        SaveImage(image, thumbPath, storagePath, _imageProcessingSettings.ThumbnailWidth, cancellationToken),
                        SaveImage(image, fullPath, storagePath, _imageProcessingSettings.FullscreenWidth, cancellationToken),
                    };

                    await Task.WhenAll(tasks);

                    var httpEndpoint = _imageStorageSettings.ImagesHttpEndpoint;

                    var domainImage = new Domain.Image
                    {
                        OriginalImagePath = $"{httpEndpoint}/{subDir}/{origPath}",
                        ThumbnailImagePath = $"{httpEndpoint}/{subDir}/{thumbPath}",
                        FullscreenImagePath = $"{httpEndpoint}/{subDir}/{fullPath}",
                    };

                    if (message.ImageFor == ImageFor.Post)
                    {
                        domainImage.PostId = message.ResourceId;
                        var post = await postRepo.QueryById(message.ResourceId).SingleAsync(cancellationToken);
                        post.DoneProcessing = true;
                    }
                    else if (message.ImageFor == ImageFor.User)
                    {
                        domainImage.UserProfileId = message.ResourceId;
                    }

                    imgRepo.Add(domainImage);

                    await unitOfWork.SaveAsync(cancellationToken);
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                _tempFileUploadService.RemoveFile(message.FilePath);
            }
        }
    }

    private async Task SaveImage(Image image, string imageName, string storagePath, int resizeWidth, CancellationToken cancellationToken)
    {
        var width = image.Width;
        var height = image.Height;

        if (image.Width > resizeWidth)
        {
            double resizeRatio = image.Width / resizeWidth;

            width = resizeWidth;
            height = (int) (height / resizeRatio);
        }
        using var transformed = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(width, height),
        }));
        await transformed.SaveAsJpegAsync($"{storagePath}/{imageName}", new JpegEncoder
        {
            Quality = _imageProcessingSettings.ImageQuality
        }, cancellationToken);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => DoWorkAsync(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ImageProcessing Background Service is stopping.");
        return Task.CompletedTask;
    }
}
