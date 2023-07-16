namespace SocialApp.Api.Requests;

public class PagedRequest
{
    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
}
