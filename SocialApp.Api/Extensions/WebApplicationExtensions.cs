using SocialApp.Api.Middleware;

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

        //app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
        //{
        //    //builder.UseSpa(builder => builder.UseProxyToSpaDevelopmentServer("http://localhost:5173"));
        //    builder.UseSpa(builder => builder.UseProxyToSpaDevelopmentServer("http://localhost:3000"));
        //});

        app.Run();
    }
}
