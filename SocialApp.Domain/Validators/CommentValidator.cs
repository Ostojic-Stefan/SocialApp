using FluentValidation;

namespace SocialApp.Domain.Validators;

internal class CommentValidator : AbstractValidator<Comment>
{
	public CommentValidator()
	{
        RuleFor(post => post.Contents)
            .NotNull().WithMessage("Contents should not be null")
            .NotEmpty().WithMessage("Contents should not be empty")
            .MaximumLength(200).WithMessage("Contents should be max 240 characters");
    }
}
