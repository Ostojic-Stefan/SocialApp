using DataAccess;
using DotNetEnv;
using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialApp.Api.BackgroundServices;
using SocialApp.Api.Filters;
using SocialApp.Api.Middleware;
using SocialApp.Api.Requests;
using SocialApp.Api.SignalR;
using SocialApp.Api.SignalR.Notification;
using SocialApp.Api.SignalR.Posts;
using SocialApp.Application.Interfaces;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Services.FileUpload;
using SocialApp.Application.Settings;
using System.Net;
using System.Text.Json;
using System.Threading.Channels;

namespace SocialApp.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseWebRoot("wwwroot");
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
            options.AddPolicy(name: "AllowAll", policy =>
            {
                policy
                .WithOrigins("https://localhost:5173")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
            })
        );

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPostsQuery).Assembly));
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllPostsQuery));
        
        builder.AddAuthServices();
        builder.AddSettings();
        builder.AddDbServices();
        builder.AddHostedServices();

        builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
        builder.Services.AddTransient<GlobalExceptionMiddleware>();
        builder.Services.AddTransient<ITempFileUploadService, TempFileUploadService>();
        builder.Services.AddScoped<ValidateModelAttribute>();
        builder.Services.AddScoped<NotificationHubService>();
        builder.Services.AddScoped<PostHubCache>();

        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }

    private static void AddHostedServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<NotificationBackgroundService>();
        builder.Services.AddHostedService<ImageProcessingBackgroundService>();
        builder.Services.AddSingleton(_ => Channel.CreateUnbounded<ImageProcessingMessage>());
        builder.Services.AddSingleton<INotificationMessenger, NotificationMessenger>();
    }

    private static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication("Cookie")
            .AddCookie("Cookie", opt =>
            {
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = "session";
                opt.Events.OnRedirectToLogin = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    var error = new ErrorResponse
                    {
                        Title = "Unauthorized",
                        ErrorMessages = new string[] { "Unauthorized" },
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                    await context.Response.WriteAsJsonAsync(error);
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void AddDbServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseNpgsql(DotNetEnv.Env.GetString("CONNECTION_STRING"));
        });

        builder.Services.AddIdentityCore<IdentityUser>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<DataContext>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(provider =>
        {
            return new UnitOfWork(provider.GetRequiredService<DataContext>());
        });
    }

    private static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("./Settings/image_file_storage_settings.json");
        builder.Services.Configure<ImageFileStorageSettings>(builder.Configuration.GetSection("ImageFileStorageSettings"));

        builder.Configuration.AddJsonFile("./Settings/image_processing_settings.json");
        builder.Services.Configure<ImageProcessingSettings>(builder.Configuration.GetSection("ImageProcessingSettings"));
    }
}
