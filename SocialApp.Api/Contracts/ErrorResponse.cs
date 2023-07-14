using System.Net;

namespace SocialApp.Api.Contracts;

public class ErrorResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Title { get; set; }
    public IList<string>? ErrorMessages { get; set; }
}
