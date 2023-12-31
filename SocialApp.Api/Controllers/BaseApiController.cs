﻿using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Requests;
using SocialApp.Application.Models;
using System.Net;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("/api")]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleError(Tuple<AppErrorCode, List<string>> error)
    {
        var env = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
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
            AppErrorCode.DuplicateEntry => BadRequest(new ErrorResponse
            {
                Title = "Duplicate Entry",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.BadRequest,
            }),
            AppErrorCode.BadCredentials => Unauthorized(new ErrorResponse
            {
                Title = "Bad Credentials",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.Unauthorized,
            }),
            AppErrorCode.UnAuthorized => Unauthorized(new ErrorResponse
            {
                Title = "Unauthorized",
                ErrorMessages = error.Item2,
                StatusCode = HttpStatusCode.Unauthorized,
            }),
            AppErrorCode.ServerError => StatusCode(500, new ErrorResponse
            {
                Title = "Internal Server Error",
                ErrorMessages = env.IsDevelopment()
                    ? error.Item2 
                    : new string[] { "Something went wrong" },
                StatusCode = HttpStatusCode.InternalServerError,
            }),
            _ => throw new Exception($"Unsupported {nameof(AppErrorCode)} type")
        };
    }
}
