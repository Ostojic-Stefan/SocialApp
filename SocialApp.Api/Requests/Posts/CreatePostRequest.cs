﻿using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.Posts;

public class CreatePostRequest
{
    [StringLength(200, ErrorMessage = "Image url cannot have more than 200 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Image url must contain some data")]
    public required string ImageName { get; set; }

    [StringLength(20, ErrorMessage = "Contents of a post cannot have more than 20 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Title of a post must contain some data")]
    public required string Title { get; set; }

    [StringLength(200, ErrorMessage = "Contents of a post cannot have more than 200 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Contents of a post must contain some data")]
    public required string Contents { get; set; }
}
