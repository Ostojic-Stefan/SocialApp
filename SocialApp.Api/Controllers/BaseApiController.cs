using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Contracts;
using SocialApp.Application.Models;
using System.Net;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleError(Tuple<AppErrorCode, List<string>> error)
    {
        return error.Item1 switch
        {
            AppErrorCode.NotFound => NotFound(new ErrorResponse
            {
                Title = "Not Found",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.NotFound,
            }),
            AppErrorCode.ValidationError => BadRequest(new ErrorResponse
            {
                Title = "Validation Error",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.BadRequest,
            }),
            AppErrorCode.ServerError => StatusCode(500, new ErrorResponse
            {
                Title = "Internal Server Error",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.InternalServerError,
            }),
            _ => throw new Exception($"Unsupported {nameof(AppErrorCode)} type")
        };
    }
}
