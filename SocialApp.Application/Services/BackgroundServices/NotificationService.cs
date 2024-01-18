using EfCoreHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialApp.Domain;

namespace SocialApp.Application.Services.BackgroundService;

public sealed class NotificationService : IHostedService, IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly INotificationQueue _queue;
    private readonly ILogger<NotificationService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationService(
        INotificationQueue queue,
        ILogger<NotificationService> logger,
        IServiceScopeFactory serviceScopeFactory 
        )
    {
        _queue = queue;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => ProcessQueue(_cts.Token), _cts.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return Task.CompletedTask;
    }

    private async Task ProcessQueue(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var queueData = _queue.RemoveNotification();
            if (queueData != null)
            {
                if (queueData.SenderUserId != queueData.Post.UserProfileId)
                {
                    using IServiceScope scope = _serviceScopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var notificationRepo = unitOfWork.CreateReadWriteRepository<Notification>();

                    if (queueData.Comment is not null)
                    {
                        var notification = Notification.CreateNotification(
                            queueData.SenderUserId,
                            queueData.Post.UserProfileId,
                            queueData.Post.Id,
                            queueData.Comment.Id,
                            null);
                        notificationRepo.Add(notification);
                        await unitOfWork.SaveAsync(cancellationToken);
                    }
                    else if (queueData.Like is not null)
                    {
                        var notification = Notification.CreateNotification(
                            queueData.SenderUserId,
                            queueData.Post.UserProfileId,
                            queueData.Post.Id,
                            null,
                            queueData.Like.Id);
                        notificationRepo.Add(notification);
                        await unitOfWork.SaveAsync(cancellationToken);
                    }
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}
