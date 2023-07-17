using DataAccess;
using EfCoreHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialApp.Api.Middleware;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Services;
using SocialApp.Application.Settings;
using System.Text;

namespace SocialApp.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseWebRoot("wwwroot");

        builder.Services.AddTransient<GlobalExceptionMiddleware>();

        builder.AddSettings();
        builder.AddJwtService();
        builder.AddDbServices();
        builder.AddSwaggerConfiguration();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPostsQuery).Assembly));
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllPostsQuery));

        builder.Services.AddTransient<ITokenService, TokenService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
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
        builder.Configuration.AddJsonFile("./Settings/jwtSettings.json");
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    }

    private static void AddJwtService(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
        {
            var jwtSection = builder.Configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSection.Get<JwtSettings>();

            cfg.SaveToken = true;
            cfg.Audience = jwtSettings.Audiences[0];
            cfg.ClaimsIssuer = jwtSettings.Issuer;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudiences = jwtSettings.Audiences,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });
    }

    private static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SocialApp",
                Version = "v1",
            });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Provide a JWT Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
