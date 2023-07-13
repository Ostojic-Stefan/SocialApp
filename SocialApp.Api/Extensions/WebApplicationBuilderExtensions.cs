using DataAccess;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Posts.Queries;

namespace SocialApp.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        DotNetEnv.Env.Load(".env");
        var connString = DotNetEnv.Env.GetString("CONNECTION_STRING");

        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseNpgsql(connString);
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(provider =>
        {
            return new UnitOfWork(provider.GetRequiredService<DataContext>());
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPostsQuery).Assembly));
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllPostsQuery));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}
