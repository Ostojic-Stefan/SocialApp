using SocialApp.Api.Middleware;
using SocialApp.Api.SignalR;
using SocialApp.Api.SignalR.Notification;
using SocialApp.Api.SignalR.Posts;

namespace SocialApp.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void AddPipelineComponents(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");

        app.UseStaticFiles();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<TestHub>("/hub");
        app.MapHub<NotificationHub>("/notification-hub");
        app.MapHub<PostHub>("/post-hub");

        app.Run();
    }
}
