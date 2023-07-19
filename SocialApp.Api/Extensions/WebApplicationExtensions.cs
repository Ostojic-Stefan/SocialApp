﻿using SocialApp.Api.Middleware;

namespace SocialApp.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void AddPipelineComponents(this WebApplication app)
    {
        //app.UseCors(
        //    options => options.AllowAnyHeader()
        //    .AllowAnyMethod()
        //    .SetIsOriginAllowed(origin => true)
        //    .AllowCredentials()
        //    .WithOrigins("http://localhost:5173/")
        //);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
        {
            builder.UseSpa(builder => builder.UseProxyToSpaDevelopmentServer("http://localhost:5173"));
        });

        app.Run();
    }
}
