using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.Comments;

public class CreateComment
{
    public required Guid PostId { get; set; }

    [StringLength(240, ErrorMessage = "Contents of a comment cannot have more than 240 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Contents of a comment must contain some data")]
    public required string Contents { get; set; }
}
