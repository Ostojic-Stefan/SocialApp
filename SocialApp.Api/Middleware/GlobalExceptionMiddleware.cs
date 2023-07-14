using SocialApp.Api.Contracts;
using System.Net;
using System.Text.Json;

namespace SocialApp.Api.Middleware;

public class GlobalExceptionMiddleware : IMiddleware
{
	private readonly IWebHostEnvironment _env;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(IWebHostEnvironment env,
		ILogger<GlobalExceptionMiddleware> logger)
	{
		_env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

			var error = new ErrorResponse
			{
				Title = "Internal Server Error",
				ErrorMessages = new string[] { _env.IsDevelopment() ? ex.ToString() : "Something went wrong" },
				StatusCode = HttpStatusCode.InternalServerError
			};

            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
		}
    }
}
