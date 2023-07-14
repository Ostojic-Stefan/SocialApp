using FluentValidation;

namespace SocialApp.Domain.Validators;

internal class PostValidator : AbstractValidator<Post>
{
	public PostValidator()
	{
		RuleFor(post => post.ImageUrl)
			.NotNull().WithMessage("ImageUrl should not be null")
			.NotEmpty().WithMessage("ImageUrl should not be empty")
			.MaximumLength(200).WithMessage("ImageUrl should be max 200 characters");
		RuleFor(post => post.Contents)
            .NotNull().WithMessage("Contents should not be null")
            .NotEmpty().WithMessage("Contents should not be empty")
            .MaximumLength(200).WithMessage("Contents should be max 240 characters");
    }
}
