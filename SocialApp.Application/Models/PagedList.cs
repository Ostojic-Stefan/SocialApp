namespace SocialApp.Application.Models;

public class PagedList<T>
{
    public required IReadOnlyList<T> Items { get; set; }
    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
    public required int TotalCount { get; set; }
    public required int TotalPages { get; set; }
}