using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.Posts;

public class UpdatePost
{
    [StringLength(200, ErrorMessage = "Image url cannot have more than 200 characters")]
    public string? Contents { get; set; }

    [StringLength(200, ErrorMessage = "Contents of a post cannot have more than 200 characters")]
    public string? ImageUrl { get; set; }
}
