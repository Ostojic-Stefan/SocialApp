using System.Net;

namespace SocialApp.Api.Requests;

public class ErrorResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Title { get; set; }
    public IList<string>? ErrorMessages { get; set; }
}
