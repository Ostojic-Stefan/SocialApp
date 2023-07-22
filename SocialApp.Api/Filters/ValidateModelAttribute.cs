using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialApp.Api.Requests;
using System.Net;

namespace SocialApp.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var error = new ErrorResponse
            {
                Title = "Invalid Model State",
                ErrorMessages = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                StatusCode = HttpStatusCode.BadRequest,
            };
            context.Result = new BadRequestObjectResult(error);
        }
    }
}
