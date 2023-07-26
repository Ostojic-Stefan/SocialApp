using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialApp.Api.Filters;

public class ValidateGuids : ActionFilterAttribute
{
    private readonly IReadOnlyList<string> _guids;

    public ValidateGuids(params string[] guids)
    {
        _guids = guids;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidateGuids>>();

        foreach (var currGuid in _guids)
        {
            var guidExists = context.ActionArguments.TryGetValue(currGuid, out var guid);
            if (!guidExists)
            {
                logger.LogWarning("{currGuid} does not exist on the request.", currGuid);
                continue;
            }

            // TODO: return ErrorResponse object
            if (!Guid.TryParse(guid!.ToString(), out var _))
                context.Result = new ObjectResult("Invalid Guid");
        }
    }
}
