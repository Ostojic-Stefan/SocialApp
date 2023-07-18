﻿using SocialApp.Api.Middleware;

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

        app.UseCors(
            options => options.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
            .WithOrigins("http://localhost:5173/")
        );


        app.UseStaticFiles();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
