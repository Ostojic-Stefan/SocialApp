using EfCoreHelpers;
using Microsoft.AspNetCore.SignalR;
using SocialApp.Api.SignalR;
using SocialApp.Application.Interfaces;
using SocialApp.Domain;

namespace SocialApp.Api.BackgroundServices;

public class NotificationBackgroundService : BackgroundService
{
    private readonly INotificationMessenger _notificationMessenger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationBackgroundService(
        INotificationMessenger notificationMessenger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _notificationMessenger = notificationMessenger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _notificationMessenger.WaitForData(stoppingToken))
        {
            var message = await _notificationMessenger.GetNotificationAsync(stoppingToken);

            if (message.SenderUserId != message.Post.UserProfileId)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var notificationRepo = unitOfWork.CreateReadWriteRepository<Notification>();

                    Notification notification = message.Comment != null
                        ? Notification.CreateForComment(
                            message.SenderUserId,
                            message.Post.Id,
                            message.Post.UserProfileId,
                            message.Comment.Id)
                        : Notification.CreateForLike(
                            message.SenderUserId,
                            message.Post.Id,
                            message.Post.UserProfileId,
                            message.Like.Id);
                        
                    notificationRepo.Add(notification);
                    await unitOfWork.SaveAsync(stoppingToken);

                    var notificationHub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();
                    //await notificationHub.Clients.User(message.Post.UserProfile.Id.ToString()).ReceiveNotification("yee boi");
                        //.SendAsync("ReceiveNotification", "yee boi", stoppingToken);
                }
            }
        }
    }
}
